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

            // �ִϸ��̼� ���
            player.anim.speed = 1f;
        }
        else
        {
            player.SetZeroVelocity();

            // �ִϸ��̼� ����
            player.anim.speed = 0f;
        }

        // LeftControl�� �ٽ� ������ sit���� ��ȯ
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // �ִϸ��̼� ��� �ӵ� ����
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
