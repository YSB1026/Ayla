using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : Entity
{
    [Header("�̵� ����")]
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

        // ���� �ӽ� �ν��Ͻ� ����
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

        // ���� ���� �� �ʱ� ���¸� ��� ����(inputState)�� ����
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
