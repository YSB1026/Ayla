using UnityEngine;

public class Player_WalkState : PlayerState
{
    public Player_WalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);

        if (Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(player.runState);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.inputState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
