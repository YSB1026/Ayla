using NUnit.Framework.Interfaces;
using UnityEngine;

public class Enemy : Entity
{
    [Header("이동 정보")]
    private float moveSpeed = 1f;
    public float applySpeed;
    public float defaultMoveSpeed = 1f;

    public float stunTime { get; private set; } = 0f;

    public EnemyStateMachine stateMachine { get; private set; }

	public Enemy_StunState stunState { get; private set; }


	protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
		stunState = new Enemy_StunState(this, stateMachine, "Stun", this);

	}

	protected override void Start()
    {
        base.Start();
        applySpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void ApplySlow(float slowPercent)
    {
        applySpeed = moveSpeed / slowPercent;
    }

    public virtual void ClearSlow()
    {
        applySpeed = moveSpeed;
    }

    public virtual void ApplyStun(float time)
    {
        stunTime = time;
		stateMachine.ChangeState(stunState);
    }

    public virtual void HitPlayer()
    {
       
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationEndTrigger();
}
