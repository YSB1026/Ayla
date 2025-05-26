using UnityEngine;

public class Player_CrawlState : PlayerState
{
    public Player_CrawlState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetCrawCollider();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.crawlSpeed, rb.linearVelocity.y);

            // 애니메이션 재생
            player.anim.speed = 1f;
        }
        else
        {
            player.SetZeroVelocity();
            player.anim.speed = 0f;
        }
        // W을 다시 누르면 stand로 전환
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 애니메이션 재생 속도 복원
            player.anim.speed = 1f;

            stateMachine.ChangeState(player.sitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetIdleCollider();
	}
}
