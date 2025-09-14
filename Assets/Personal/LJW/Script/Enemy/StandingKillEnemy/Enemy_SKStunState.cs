using UnityEngine;

public class Enemy_SKStunState : EnemyState
{
	private Enemy_SK EnemySK;

	private float stunTimer = 0f;
	private float stunTime = 1f;

	public Enemy_SKStunState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK enemy) : base(_enemy, _stateMachine, _animBoolName)
	{
		EnemySK = enemy;
	}

	public override void AnimationEndTrigger()
	{
		base.AnimationEndTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		stunTime = EnemySK.stunTime;
	}

	public override void Update()
	{
		base.Update();

		stunTime = Time.deltaTime;

		if (stunTimer > stunTime)
			stateMachine.ChangeState(EnemySK.idleState);
	}

	public override void Exit()
	{
		base.Exit();
		stunTimer = 0f;
	}
}
