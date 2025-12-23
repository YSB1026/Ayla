using UnityEngine;

public class Player_GrabState : PlayerState
{
	public Player_GrabState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
		: base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		hit = player.GetObjectHitInfo();
		Debug.Log(hit.collider.gameObject.name);
	}

	public override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(KeyCode.F) || !player.IsObjectDetected())
			stateMachine.ChangeState(player.inputState);

		if (xInput > 0 && player.facingDir == 1)
			stateMachine.ChangeState(player.pushState);

		if (xInput > 0 && player.facingDir == -1)
			stateMachine.ChangeState(player.pullState);

		if (xInput < 0 && player.facingDir == 1)
			stateMachine.ChangeState(player.pullState);

		if (xInput < 0 && player.facingDir == -1)
			stateMachine.ChangeState(player.pushState);
	}

	public override void Exit()
	{
		base.Exit();
	}
}
