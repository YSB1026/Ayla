using UnityEngine;

public class Player : Entity
{
    [SerializeField] private LayerMask whatIsTrap;

    [Header("Mirror 오브젝트")]
    [SerializeField] private GameObject mirror;

    [Header("이동 정보")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float crawlSpeed;
    public float sitWalkSpeed;
    public bool isInZone;

    [Header("잡기/밀기 설정")]
    public float grabSpeed = 3f; // 에러 잡는 변수
    public float grabDistance = 1f; // 물체 감지 거리
    public LayerMask whatIsObject; // 밀 수 있는 물체 레이어

    private bool canHide;
    private Transform currentHideAnchor;

    private CapsuleCollider2D col;

    #region ColliderSetting
    private Vector2 idleColOffset = new Vector2(0.05f, 0.1f);
    private Vector2 idleColSize = new Vector2(0.93f, 1.5f);
    private Vector2 sitColOffset = new Vector2(0f, -0.1f);
    private Vector2 sitColSize = new Vector2(0.9f, 1.1f);
    private Vector2 crawlColOffset = new Vector2(0f, -0.2f);
    private Vector2 crawlColSize = new Vector2(2f, 0.9f);
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public Player_InputState inputState { get; private set; }

    public Player_WalkState walkState { get; private set; }
    public Player_RunState runState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_CrawlState crawlState { get; private set; }
    public Player_SitState sitState { get; private set; }
    public Player_StandState standState { get; private set; }
    public Player_SitWalkState sitWalkState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_AirState airState { get; private set; }

    public Player_HideState hideState { get; private set; }

    public Player_DownState downState { get; private set; }
    public Player_UpState upState { get; private set; }

    public Player_ShadowState shadowState { get; private set; }
    #endregion

    public bool controlEnabled { get; private set; } = true;
    public bool IsHidden { get; private set; } = false;
    public void SetHidden(bool value) => IsHidden = value;

    [Header("그림자 능력 연결")]
    public Shadow shadowAbility;

    public void SetControlEnabled(bool isEnabled)
    {
        if (controlEnabled == isEnabled) return;
        controlEnabled = isEnabled;
        if (!isEnabled) stateMachine.ChangeState(inputState);
    }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        inputState = new Player_InputState(this, stateMachine, "Idle");

        walkState = new Player_WalkState(this, stateMachine, "Walk");
        runState = new Player_RunState(this, stateMachine, "Run");
        jumpState = new Player_JumpState(this, stateMachine, "Jump");
        crawlState = new Player_CrawlState(this, stateMachine, "Crawl");
        sitState = new Player_SitState(this, stateMachine, "Sit");
        standState = new Player_StandState(this, stateMachine, "Stand");
        sitWalkState = new Player_SitWalkState(this, stateMachine, "SitWalk");
        deadState = new Player_DeadState(this, stateMachine, "Die");
        airState = new Player_AirState(this, stateMachine, "Fall");

        hideState = new Player_HideState(this, stateMachine, "Hide");

        downState = new Player_DownState(this, stateMachine, "Down");
        upState = new Player_UpState(this, stateMachine, "Up");

        shadowState = new Player_ShadowState(this, stateMachine, "ShadowMode");
    }

    protected override void Start()
    {
        base.Start();

        col = GetComponent<CapsuleCollider2D>();

        stateMachine.Initialize(inputState);
    }

    protected override void Update()
    {
        if (!controlEnabled)
        {
            SetZeroVelocity();
            return;
        }

        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (mirror != null)
            {
                bool newState = !mirror.activeSelf;
                mirror.SetActive(newState);
            }
        }

        stateMachine.currentState.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsTrapDetected())
        {
            collision.gameObject.GetComponent<MovebleObject>().FreezeObject(false);
            collision.gameObject.GetComponent<MovebleObject>().SetTrigger(true);
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
        col.offset = crawlColOffset;
        col.size = crawlColSize;
    }

    // 물체가 앞에 있는지 확인하는 함수
    public bool IsObjectDetected()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, grabDistance, whatIsObject);
    }

    // 물체의 정보를 가져오는 함수 (InteractiveObject 접근용)
    public RaycastHit2D GetObjectHitInfo()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * facingDir, grabDistance, whatIsObject);
    }

    // 기즈모 그리기
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDir * grabDistance);
    }

    public void ForceSetControlEnabled(bool isEnabled)
    {
        controlEnabled = isEnabled;
    }

    public bool IsHidingSpotDetected() => canHide && currentHideAnchor != null;

    public void SetHidingSpotDetected(bool detected, Transform anchor)
    {
        canHide = detected;
        currentHideAnchor = anchor;
    }

    public Transform GetHideAnchor() => currentHideAnchor;

    public void SetControlActive(bool value)
    {
        controlEnabled = value;
    }

    public void Die()
    {
        if (stateMachine.currentState == deadState) return;

        SetControlEnabled(false);
        stateMachine.ChangeState(deadState);
        CustomSceneManager.Instance.ReloadScene();
    }
}
