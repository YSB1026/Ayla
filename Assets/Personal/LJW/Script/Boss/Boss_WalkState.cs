using UnityEngine;

public class Boss_WalkState : BossState
{
    private float currentSpeed;
    public Boss_WalkState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        currentSpeed = boss.moveSpeed;
    }

    public override void Update()
    {
        base.Update();

        // 1) Ÿ�� ã��: long range �켱, ������ �Ϲ� �þ� �ڽ�
        Transform target = null;

        if (boss.longRangeCheck != null)
        {
            var colLong = Physics2D.OverlapBox(
                boss.longRangeCheck.position,
                boss.longRangeBoxSize,
                0,
                boss.whatIsPlayer
            );

            if (colLong)
            {
                target = colLong.transform;
                currentSpeed = boss.runSpeed; // long range ���� �� �ӵ� �ν�Ʈ
            }
            else
            {
                currentSpeed = boss.moveSpeed;
            }
        }

        if (target == null && boss.playerDetect != null)
        {
            var colSight = Physics2D.OverlapBox(
                boss.playerDetect.position,
                boss.detectBoxSize,
                0,
                boss.whatIsPlayer
            );

            if (colSight)
            {
                target = colSight.transform;
                // �Ϲ� �þ߿����� ������� ����
                currentSpeed = boss.moveSpeed;
            }
        }

        // 2) Ÿ���� ���̸� �� �������θ� ȸ��
        if (target != null)
        {
            float dir = Mathf.Sign(target.position.x - boss.transform.position.x);
            if (dir != 0 && dir != boss.facingDir)
                boss.Flip();
        }

        // 3) �̵�
        boss.SetVelocity(currentSpeed * boss.facingDir, rb.linearVelocity.y);

        // 4) ���� ���� ������ ��Ʋ�� ��ȯ (���� ���� ��Ʋ���� ó��)
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.battleState);
            return;
        }

        // 5) �������� �� ���� ���� �� ���� �� idle
        if (!boss.IsGroundDetected())
        {
            boss.Flip();
            stateMachine.ChangeState(boss.idleState);
            return;
        }

        /* boss.SetVelocity(boss.moveSpeed * boss.facingDir, rb.linearVelocity.y);

         if (boss.CanDetectPlayer())
         {
             stateMachine.ChangeState(boss.battleState);
         }

         if (!boss.IsGroundDetected())
         {
             boss.Flip();
             stateMachine.ChangeState(boss.idleState);
         }*/

    }

    public override void Exit()
    {
        base.Exit();
    }
}
