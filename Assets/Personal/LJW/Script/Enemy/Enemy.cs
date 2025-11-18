using NUnit.Framework.Interfaces;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("이동 정보")]
    private float moveSpeed = 1f;
    public float applySpeed;
    public float defaultMoveSpeed = 1f;

    public float stunTime { get; protected set; } = 0f;

	public EnemyStateMachine stateMachine { get; private set; }


	protected override void Awake()
    {
        base.Awake();

		stateMachine = new EnemyStateMachine();
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

    public abstract void ApplyStun(float time);

    public virtual void HitPlayer()
    {
       
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationEndTrigger();
}
