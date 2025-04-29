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
		player.col.offset = sitColOffset;
		player.col.size = sitColSize;
	}

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
            stateMachine.ChangeState(player.sitWalkState);

        if (Input.GetKeyDown(KeyCode.W))
            stateMachine.ChangeState(player.standState);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.crawlState);
    }

    public override void Exit()
    {
        base.Exit();
		player.col.offset = idleColOffset;
		player.col.size = idleColSize;
	}
}
