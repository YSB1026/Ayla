using UnityEngine;

public class Player_WalkState : PlayerState
{
    public Player_WalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.Instance.PlayFootstep(player.SurfaceType);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (xInput != 0)
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
        else
            stateMachine.ChangeState(player.inputState);

        if (Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(player.runState);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.inputState);
    }

    public override void Exit()
    {
        //SoundManager.Instance.PlayFootstep(player.SurfaceType);
        base.Exit();
    }
}
