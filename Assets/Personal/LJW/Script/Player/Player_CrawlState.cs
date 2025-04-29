using UnityEngine;

public class Player_CrawlState : PlayerState
{
    public Player_CrawlState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.col.direction = CapsuleDirection2D.Horizontal;
		player.col.offset = crawColOffset;
		player.col.size = crawColSize;
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

            // 애니메이션 정지
            player.anim.speed = 0f;
        }

        // LeftControl을 다시 누르면 sit으로 전환
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // 애니메이션 재생 속도 복원
            player.anim.speed = 1f;

            stateMachine.ChangeState(player.sitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.col.direction = CapsuleDirection2D.Vertical;
		player.col.offset = idleColOffset;
		player.col.size = idleColSize;
	}
}
