using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : Entity
{
    [Header("이동 정보")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float crawlSpeed;
    public float sitWalkSpeed;

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
    #endregion

    public bool controlEnabled = true;

    public void SetControlEnabled(bool isEnabled)
    {
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
    }

    protected override void Start()
    {
        base.Start();

        // 게임 시작 시 초기 상태를 대기 상태(inputState)로 설정
        stateMachine.Initialize(inputState);
    }

    protected override void Update()
    {
        if (!controlEnabled)
            return;

        base.Update();

        stateMachine.currentState.Update();
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
