using UnityEngine;

public class Boss : Entity
{
    public Transform player;
    [Header("이동 정보")]
    public float moveSpeed = 1f;

    #region States
    public BossStateMachine stateMachine { get; private set; }
    public Boss_IdleState idleState {  get; private set; }
    public Boss_WalkState walkState { get; private set; }
    public Boss_RunState runState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();
        idleState = new Boss_IdleState(this, stateMachine, "Idle");
        walkState = new Boss_WalkState(this, stateMachine, "Walk");
        runState = new Boss_RunState(this, stateMachine, "Run");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
