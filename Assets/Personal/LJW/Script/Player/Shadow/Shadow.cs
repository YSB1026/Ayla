using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("기본 설정")]
    public float moveSpeed = 5f;
    public float grabSpeed = 3f;
    public float grabDistance = 1f;
    public LayerMask whatIsObject;

    [Header("통과 가능한 벽 설정")]
    public LayerMask passableWallLayer; // 통과할 벽 레이어
    
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
        
        // 통과 가능한 벽과의 충돌 무시 설정
        SetupPassableWallCollision();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    // 통과 가능한 벽과의 충돌 무시 설정
    private void SetupPassableWallCollision()
    {
        // Shadow의 Layer와 통과 가능한 벽 Layer 간의 충돌 무시
        int shadowLayer = gameObject.layer;
        int passableLayer = LayerMaskToLayer(passableWallLayer);
        
        Physics2D.IgnoreLayerCollision(shadowLayer, passableLayer, true);
    }

    // LayerMask를 Layer 번호로 변환
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
    }

    public void DeactivateShadow()
    {
        gameObject.SetActive(false);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDir * grabDistance);
    }

    public RaycastHit2D GetObjectHitInfo()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, grabDistance, whatIsObject);
    }
}