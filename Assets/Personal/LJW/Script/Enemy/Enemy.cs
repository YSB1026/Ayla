using UnityEngine;

public class Enemy : Entity
{
    [Header("이동 정보")]
    public float moveSpeed = 1f;
    public float defaultMoveSpeed = 1f;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void HitPlayer()
    {
       
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationEndTrigger();
}
