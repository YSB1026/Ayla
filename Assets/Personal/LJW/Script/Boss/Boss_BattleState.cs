using UnityEngine;

public class Boss_BattleState : BossState
{
    private Transform player;
    private int moveDir;
    public Boss_BattleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // �÷��̾� ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            this.player = player.transform;
    }

    public override void Update()
    {
        base.Update();

        // 1) ���� ������ �ٷ� ����(Attack2)
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        // 2) �ָ����� ���̰ų�(��� �ڽ�) �Ϲ� �þ� �ڽ����� ������ ���� ����
        if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.walkState);
            return;
        }

        // ������ �� �Ǹ� idle�� ����
        if (!boss.IsPlayerInAttackBox())
        {
            Debug.Log("�÷��̾� ���� �ȵ�. Idle ��ȯ");
            stateMachine.ChangeState(boss.idleState);
            return;
        }

        // �� �ٿ� �� + ���� �ڽ� ������ �� run
        if (boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.runState);
            return;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

}
