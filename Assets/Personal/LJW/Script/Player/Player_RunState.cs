using UnityEngine;

public class Player_RunState : PlayerState
{
    public Player_RunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.runSpeed, rb.linearVelocityY);

		if (!player.IsGroundDetected())
			stateMachine.ChangeState(player.airState);
		else if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);
        else if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.inputState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
