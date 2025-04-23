using UnityEngine;

public class Player_CrawlState : PlayerState
{
    public Player_CrawlState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.crawlSpeed, rb.linearVelocity.y);
        }
        else
        {
            player.SetZeroVelocity();

            // 키 떼면 sitState로 전환
            if (!Input.GetKeyDown(KeyCode.LeftControl))
                stateMachine.ChangeState(player.sitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
