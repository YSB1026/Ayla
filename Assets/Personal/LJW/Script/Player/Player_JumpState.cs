using UnityEngine;

public class Player_JumpState : PlayerState
{
    public Player_JumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
