using UnityEngine;

public class Boss_Attack1State : BossState
{
    private bool hasAttacked = false;
    private float attackTimer = 1f;
    public Boss_Attack1State(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        boss.SetZeroVelocity();
        hasAttacked = false;
        attackTimer = 1f;

        attackTimer -= Time.deltaTime;

        // ���� �������� ���� �������� idle�� ����
        if (triggerCalled && !hasAttacked)
        {
            Debug.Log("Attack1 ����, idle�� ��ȯ");
            boss.longRCoolTimer = boss.longRCoolTime;
            stateMachine.ChangeState(boss.idleState);
        }

        // ���� ������ ���������� run�̳� ���� �ൿ
        if (triggerCalled && hasAttacked)
        {
            stateMachine.ChangeState(boss.runState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

}
