using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : Entity
{
    [Header("이동 정보")]
    public float moveSpeed = 12f;
    public float jumpForce;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerRunState runState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // 상태 머신 인스턴스 생성
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        runState = new PlayerRunState(this, stateMachine, "Run");
    }

    protected override void Start()
    {
        base.Start();

        // 게임 시작 시 초기 상태를 대기 상태(idleState)로 설정
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
