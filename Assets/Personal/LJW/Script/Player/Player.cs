using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private LayerMask whatIsTrap;

    [Header("이동 정보")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float crawlSpeed;
    public float sitWalkSpeed;
    public float grabSpeed;
    public bool isInZone;

    private CapsuleCollider2D col;

    #region ColliderSetting
    private Vector2 idleColOffset = new Vector2(0f, 0f);
    private Vector2 idleColSize = new Vector2(0.9f, 1.3f);
    private Vector2 sitColOffset = new Vector2(0f, -0.1f);
    private Vector2 sitColSize = new Vector2(0.9f, 1.1f);
    private Vector2 crawColOffset = new Vector2(0f, -0.2f);
    private Vector2 crawColSize = new Vector2(2f, 0.9f);
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public Player_InputState inputState { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_WalkState walkState { get; private set; }
    public Player_RunState runState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_CrawlState crawlState { get; private set; }
    public Player_SitState sitState { get; private set; }
    public Player_StandState standState { get; private set; }
    public Player_SitWalkState sitWalkState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_GrabState grabState { get; private set; }
    public Player_PullState pullState { get; private set; }
    public Player_PushState pushState { get; private set; }
    public Player_AirState airState { get; private set; }

    public Player_DownState downState { get; private set; }
    public Player_UpState upState { get; private set; }

    #endregion

    public bool controlEnabled { get; private set; } = true;

    public void SetControlEnabled(bool isEnabled)
    {
        if (controlEnabled == isEnabled) return;
        if (!isEnabled) stateMachine.ChangeState(inputState);

        controlEnabled = isEnabled;
    }

    protected override void Awake()
    {
        base.Awake();

        // 상태 머신 인스턴스 생성
        stateMachine = new PlayerStateMachine();
        inputState = new Player_InputState(this, stateMachine, "Idle");

        idleState = new Player_IdleState(this, stateMachine, "Idle");
        walkState = new Player_WalkState(this, stateMachine, "Walk");
        runState = new Player_RunState(this, stateMachine, "Run");
        jumpState = new Player_JumpState(this, stateMachine, "Jump");
        crawlState = new Player_CrawlState(this, stateMachine, "Crawl");
        sitState = new Player_SitState(this, stateMachine, "Sit");
        standState = new Player_StandState(this, stateMachine, "Stand");
        sitWalkState = new Player_SitWalkState(this, stateMachine, "SitWalk");
        deadState = new Player_DeadState(this, stateMachine, "Die");
        grabState = new Player_GrabState(this, stateMachine, "Grab");
        pullState = new Player_PullState(this, stateMachine, "Pull");
        pushState = new Player_PushState(this, stateMachine, "Push");
        airState = new Player_AirState(this, stateMachine, "Fall");

        downState = new Player_DownState(this, stateMachine, "Down");
        upState = new Player_UpState(this, stateMachine, "Up");
    }

    protected override void Start()
    {
        base.Start();

        col = GetComponent<CapsuleCollider2D>();

        // 게임 시작 시 초기 상태를 대기 상태(inputState)로 설정
        stateMachine.Initialize(inputState);
    }

    protected override void Update()
    {
        if (!controlEnabled) return;

        base.Update();

        stateMachine.currentState.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsTrapDetected())
        {
            collision.gameObject.GetComponent<InteractiveObject>().FreezeObject(false);
            collision.gameObject.GetComponent<InteractiveObject>().SetTrigger(true);
        }
    }

    public virtual bool IsTrapDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsTrap);


    private SurfaceType GetSurfaceTypeUnderPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        if (hit.collider != null)
        {
            Surface surface = hit.collider.GetComponent<Surface>();
            if (surface != null)
            {
                return surface.surfaceType;
            }
        }
        return SurfaceType.None;
    }
    public SurfaceType SurfaceType => GetSurfaceTypeUnderPlayer();
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void PlayFootstepSound() => stateMachine.currentState.PlayFootstepSound();
    public void PlayCrawlingSound() => stateMachine.currentState.PlayCrawlingSound();

    public void SetIdleCollider()
    {
        col.direction = CapsuleDirection2D.Vertical;
        col.offset = idleColOffset;
        col.size = idleColSize;
    }

    public void SetSitCollider()
    {
        col.direction = CapsuleDirection2D.Vertical;
        col.offset = sitColOffset;
        col.size = sitColSize;
    }

    public void SetCrawCollider()
    {
        col.direction = CapsuleDirection2D.Horizontal;
        col.offset = crawColOffset;
        col.size = crawColSize;
    }


    public void ForceSetControlEnabled(bool isEnabled)
    {
        controlEnabled = isEnabled;
    }

    public void Die()
    {
        if (stateMachine.currentState == deadState) return;

        SetControlEnabled(false);
        stateMachine.ChangeState(deadState);
    }


}
