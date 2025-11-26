using UnityEngine;

public class Player_AirState : PlayerState
{
	private Vector2 airStartPos;
	private Vector2 airEndPos;
	private float fallingHeight;
	private const float DEAD_HEIGHT = 8f;

	public Player_AirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
		: base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		airStartPos = player.transform.position;
	}

	public override void Update()
	{
		base.Update();

		if (xInput != 0)
			player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

		if (player.IsGroundDetected())
		{
			airEndPos = player.transform.position;

			fallingHeight = airStartPos.y - airEndPos.y;

			if (fallingHeight > DEAD_HEIGHT)
			{
				stateMachine.ChangeState(player.deadState);
			}
			else
				stateMachine.ChangeState(player.inputState);
		}
	}

	public override void Exit()
	{
		base.Exit();

	}
}
