using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
