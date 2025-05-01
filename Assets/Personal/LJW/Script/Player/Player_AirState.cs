using UnityEngine;

public class Player_AirState : PlayerState
{
	private Vector2 airStartPos;
	private Vector2 airEndPos;
	private float fallingHeight;
	private const float DEAD_HEIGHT = 15f;

	private bool isDead = false;//없을시 Exit가 재귀함수가 되므로 스택오버플러우 일어남

	public Player_AirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
		: base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		airStartPos = player.transform.position;
		isDead = false;
	}

	public override void Update()
	{
		base.Update();

		if (xInput != 0)
			player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

		if (player.IsGroundDetected())
			stateMachine.ChangeState(player.inputState);
	}

	public override void Exit()
	{
		base.Exit();
		airEndPos = player.transform.position;

		fallingHeight = airStartPos.y - airEndPos.y;

		if (fallingHeight > DEAD_HEIGHT && !isDead)
		{
			isDead = true;
			stateMachine.ChangeState(player.deadState);
		}
	}
}
