using UnityEngine;

public class Player_JumpState : PlayerState
{
    private bool isJumping = true;
    public Player_JumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
        isJumping = true; // 점프 시작
    }

    public override void Update()
    {
        base.Update();

        // 점프하면서 움직이는 값
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.airState);
	}

    public override void Exit()
    {
        base.Exit();
    }
}
