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



        if (boss.IsPlayerInCloseRange())    // attack2
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        if (boss.CanDetectLongRange && boss.IsPlayerInLongRange())  // attack1
        {
            stateMachine.ChangeState(boss.attack1State);
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
