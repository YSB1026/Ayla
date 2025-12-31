using UnityEngine;
using System.Collections.Generic;

public class Shadow : MonoBehaviour
{
    [Header("기본 설정")]
    public float moveSpeed = 5f;
    public float grabSpeed = 3f;
    public float grabDistance = 1f;
    public LayerMask whatIsObject;

    [Header("통과 가능한 벽 설정")]
    public LayerMask passableWallLayer;

    [Header("들고 있는 오브젝트 관리")]
    private List<FollowableObject> heldObjects = new List<FollowableObject>();
    public int maxHoldCount = 1; // 최대로 들 수 있는 오브젝트 수

    [Header("상태 머신")]
    public ShadowStateMachine stateMachine { get; private set; }
    public Shadow_IdleState idleState { get; private set; }
    public Shadow_WalkState walkState { get; private set; }
    public Shadow_GrabState grabState { get; private set; }
    public Shadow_PushState pushState { get; private set; }
    public Shadow_PullState pullState { get; private set; }

    [Header("컴포넌트")]
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public int facingDir { get; private set; } = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        stateMachine = new ShadowStateMachine();

        idleState = new Shadow_IdleState(this, stateMachine, "Idle");
        walkState = new Shadow_WalkState(this, stateMachine, "Walk");
        grabState = new Shadow_GrabState(this, stateMachine, "Grab");
        pushState = new Shadow_PushState(this, stateMachine, "Push");
        pullState = new Shadow_PullState(this, stateMachine, "Pull");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
        SetupPassableWallCollision();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    private void SetupPassableWallCollision()
    {
        int shadowLayer = gameObject.layer;
        int passableLayer = LayerMaskToLayer(passableWallLayer);
        Physics2D.IgnoreLayerCollision(shadowLayer, passableLayer, true);
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 1)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber;
    }

    public void ActivateShadow(Vector3 _startPosition)
    {
        gameObject.SetActive(true);
        transform.position = _startPosition;

        if (stateMachine != null && idleState != null)
        {
            stateMachine.Initialize(idleState);
        }

        // 그림자 카메라로 전환
        if (CameraSwapManager.Instance != null)
        {
            CameraSwapManager.Instance.SwitchToShadow();
        }
    }

    public void DeactivateShadow()
    {
        // Shadow 비활성화 시 모든 들고 있는 오브젝트 내려놓기
        DropAllHeldObjects();
        gameObject.SetActive(false);

        // 플레이어 카메라로 전환
        if (CameraSwapManager.Instance != null)
        {
            CameraSwapManager.Instance.SwitchToPlayer();
        }
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && facingDir == -1)
            Flip();
        else if (_x < 0 && facingDir == 1)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsObjectDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, grabDistance, whatIsObject);
    }

    public RaycastHit2D GetObjectHitInfo()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, grabDistance, whatIsObject);
    }

    // ========== 들고 있는 오브젝트 관리 ==========

    // 오브젝트를 들기
    public bool AddHeldObject(FollowableObject obj)
    {
        // 이미 최대 개수만큼 들고 있으면 실패
        if (heldObjects.Count >= maxHoldCount)
        {
            Debug.Log("더 이상 들 수 없습니다!");
            return false;
        }

        if (!heldObjects.Contains(obj))
        {
            heldObjects.Add(obj);
            Debug.Log($"{obj.gameObject.name}을(를) 들었습니다. ({heldObjects.Count}/{maxHoldCount})");
            return true;
        }

        return false;
    }

    // 오브젝트를 내려놓기
    public void RemoveHeldObject(FollowableObject obj)
    {
        if (heldObjects.Contains(obj))
        {
            heldObjects.Remove(obj);
            Debug.Log($"{obj.gameObject.name}을(를) 내려놓았습니다. ({heldObjects.Count}/{maxHoldCount})");
        }
    }

    // 모든 오브젝트 내려놓기
    public void DropAllHeldObjects()
    {
        // 리스트 복사 (Drop 호출 시 리스트가 수정되므로)
        var objectsToDrop = new List<FollowableObject>(heldObjects);

        foreach (var obj in objectsToDrop)
        {
            if (obj != null)
            {
                obj.Drop();
            }
        }

        heldObjects.Clear();
    }

    // 들고 있는 오브젝트 수 확인
    public int GetHeldObjectCount()
    {
        return heldObjects.Count;
    }

    // 오브젝트를 들고 있는지 확인
    public bool IsHoldingObject()
    {
        return heldObjects.Count > 0;
    }

    // 더 들 수 있는지 확인
    public bool CanHoldMore()
    {
        return heldObjects.Count < maxHoldCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDir * grabDistance);

        // 들고 있는 오브젝트들 표시
        if (heldObjects != null && heldObjects.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (var obj in heldObjects)
            {
                if (obj != null)
                {
                    Gizmos.DrawLine(transform.position, obj.transform.position);
                    Gizmos.DrawWireSphere(obj.transform.position, 0.3f);
                }
            }
        }
    }


}