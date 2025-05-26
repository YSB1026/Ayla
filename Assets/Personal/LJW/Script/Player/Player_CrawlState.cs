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

            // �ִϸ��̼� ���
            player.anim.speed = 1f;
        }
        else
        {
            player.SetZeroVelocity();
            player.anim.speed = 0f;
        }
        // W�� �ٽ� ������ stand�� ��ȯ
        if (Input.GetKeyDown(KeyCode.W))
        {
            // �ִϸ��̼� ��� �ӵ� ����
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
