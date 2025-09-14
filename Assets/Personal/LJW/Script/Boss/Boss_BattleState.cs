using UnityEngine;

public class Boss_BattleState : BossState
{
    private Transform player;
    private int moveDir;

    private bool isFinding = false;
    private bool findAnimDone = false;

    private Animator anim;

    public Boss_BattleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (anim == null) anim = boss.GetComponentInChildren<Animator>();

        // �÷��̾� ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            this.player = player.transform;

        isFinding = false;
        findAnimDone = false;
    }

    public override void Update()
    {
        base.Update();

        /* // 1) ���� ������ �ٷ� ����(Attack2)
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
         }*/

        // ���� ���� �� ó��
        if (isFinding)
        {
            // ���� �� �÷��̾ �ٽ� ���̸� ��� ����/����
            if (boss.IsPlayerInCloseRange())
            {
                stateMachine.ChangeState(boss.attack2State);
                return;
            }
            if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
            {
                isFinding = false;
                stateMachine.ChangeState(boss.walkState); // �ٽ� ����
                return;
            }

            // �ִϸ��̼��� �����ٸ� ��ȸ��
            if (findAnimDone)
            {
                stateMachine.ChangeState(boss.walkState);
                return;
            }

            // ���� �߿��� ���ڸ�(�Ǵ� �װ� ���ϸ� �ణ �̵�)
            boss.SetVelocity(0f, rb.linearVelocity.y);
            return;
        }

        // ��� ��Ʋ ����
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.walkState);
            return;
        }

        // ���⼭���ʹ� '�̹� �����ӿ� �÷��̾ �� ����' = ������ ���ɼ�
        isFinding = true;
        findAnimDone = false;
        boss.SetVelocity(0f, rb.linearVelocity.y);
        if (anim != null) anim.Play("Finding_Player", 0, 0f);

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        findAnimDone = true;
    }


    public override void Exit()
    {
        base.Exit();
    }

}
