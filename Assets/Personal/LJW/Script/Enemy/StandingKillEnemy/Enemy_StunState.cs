using UnityEngine;

public class Enemy_StunState : EnemyState
{
	private Enemy thisEnemy;

	private float stunTimer = 0f;
	private float stunTime = 1f;

	public Enemy_StunState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy enemy) : base(_enemy, _stateMachine, _animBoolName)
	{
		thisEnemy = enemy;
	}

	public override void AnimationEndTrigger()
	{
		base.AnimationEndTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		stunTime = thisEnemy.stunTime;
	}

	public override void Update()
	{
		base.Update();

		stunTime = Time.deltaTime;

		if (stunTimer > stunTime)
			stateMachine.ChangeState(thisEnemy.idleState);
	}

	public override void Exit()
	{
		base.Exit();
		stunTimer = 0f;
	}
}
