using UnityEngine;
using UnityEngine.Windows;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if (xInput != 0)
            stateMachine.ChangeState(player.walkState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
