using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("기본 설정")]
    public float moveSpeed = 5f;
    public float grabSpeed = 3f;
    public float grabDistance = 1f; // 물체 잡는 거리
    public LayerMask whatIsObject;  // 잡을 수 있는 물체 레이어

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
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    // 그림자 활성화
    public void ActivateShadow(Vector3 _startPosition)
    {
        // 1. 오브젝트 켜기
        gameObject.SetActive(true);

        // 2. 플레이어 위치로 이동
        transform.position = _startPosition;

        // 3. 상태 초기화 (Idle부터 시작)
        if (stateMachine != null && idleState != null)
        {
            stateMachine.Initialize(idleState);
        }
    }

    // 그림자 비활성화
    public void DeactivateShadow()
    {
        gameObject.SetActive(false);
    }

    // 방향 전환 함수
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

    // 속도 제어 함수
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    // 멈추는 함수
    public void SetZeroVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // 물체 감지
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