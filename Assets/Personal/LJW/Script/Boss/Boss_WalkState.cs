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

        // 1) Ÿ�� ã��
        Transform target = null;

        if (boss.longRangeCheck != null)
        {
            if (boss.IsPlayerInLongRange())
            {
                currentSpeed = boss.runSpeed; // ���� ���� �÷��̾��� ���� ����
                var go = GameObject.FindGameObjectWithTag("Player");
                if (go != null)
                {
                    float dir = Mathf.Sign(go.transform.position.x - boss.transform.position.x);
                    if (dir != 0 && dir != boss.facingDir) boss.Flip();
                }
            }
            else
            {
                currentSpeed = boss.moveSpeed;
            }
        }

        if (target == null && boss.IsPlayerInAttackBox())
        {
            currentSpeed = boss.moveSpeed;
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                float dir = Mathf.Sign(go.transform.position.x - boss.transform.position.x);
                if (dir != 0 && dir != boss.facingDir) boss.Flip();
            }
        }

       /* // 2) Ÿ���� ���̸� �� �������θ� ȸ��
        if (target != null)
        {
            float dir = Mathf.Sign(target.position.x - boss.transform.position.x);
            if (dir != 0 && dir != boss.facingDir)
                boss.Flip();
        }*/

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
    }

    public override void Exit()
    {
        base.Exit();
    }
}
