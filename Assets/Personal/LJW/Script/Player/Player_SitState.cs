using UnityEngine;

public class Player_SitState : PlayerState
{
    public Player_SitState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.W))
            stateMachine.ChangeState(player.standState);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.crawlState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
