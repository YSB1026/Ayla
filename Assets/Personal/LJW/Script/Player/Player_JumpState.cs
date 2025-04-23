using UnityEngine;

public class Player_JumpState : PlayerState
{
    private bool isJumping = true;
    public Player_JumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
        isJumping = true; // ���� ����

        Debug.Log("Jump Started: force " + player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        // �����ϸ鼭 �����̴� ��
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

        // �ϰ�
        if (isJumping)
        {
            if (rb.linearVelocity.y < 0)
            {
                isJumping = false;
            }
            return; // isJumping�̸� ���� ���� ����
        }

        // �� üũ
        if (player.IsGroundDetected())
        {
            Debug.Log("���� ������");
            stateMachine.ChangeState(player.idleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
