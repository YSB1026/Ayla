using UnityEngine;

public class Boss_Attack1State : BossState
{
    private bool hasAttacked = false;
    private bool hasThrown = false;
    private bool hasEvaluated = false;
    public Boss_Attack1State(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasAttacked = false;
        hasThrown = false;
        hasEvaluated = false;
    }

    public override void Update()
    {
        base.Update();

        boss.SetZeroVelocity();

        if (!hasThrown)
        {
            hasThrown = true;
            //ThrowObject();
        }

        if (hit && !hasEvaluated)
            return;

        if (hit && hasEvaluated)
        {
            if (hasAttacked)
            {
                Debug.Log("���� runState�� ��ȯ!");
                stateMachine.ChangeState(boss.runState);
            }
            else
            {
                Debug.Log("Attack1 ����, idle�� ��ȯ");
                stateMachine.ChangeState(boss.idleState);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        // �ִϸ��̼Ǹ� idle�� ���� ��ȯ
        boss.anim.SetBool("Idle", true);
    }

    // public void ThrowObject()
    // {
    //     GameObject obj = GameObject.Instantiate(boss.throwObjectPrefab, boss.throwSpawnPoint.position, Quaternion.identity);

    //     GameObject player = GameObject.FindGameObjectWithTag("Player");
    //     if (player == null) return;

    //     // ���� ��� (��¦ ��)
    //     Vector2 targetPos = player.GetComponent<Collider2D>().bounds.center;
    //     Vector2 dir = (targetPos - (Vector2)obj.transform.position).normalized;
    //     float launchForce = 15f;

    //     // ���� ���� ��ũ��Ʈ�� ó��
    //     ThrowingObjects controller = obj.GetComponent<ThrowingObjects>();
    //     if (controller != null)
    //     {
    //         controller.Setup(dir, player.transform, launchForce);

    //         controller.onHitPlayerCallback = (bool hit) =>
    //         {
    //             Debug.Log("�ݹ� ����, hasEvaluated ó�� ����");
    //             boss.longRCoolTimer = boss.longRCoolTime;

    //             if (hit)
    //             {
    //                 Debug.Log("Boss_Attack1State: �÷��̾� Ÿ�� ����!");
    //                 hasAttacked = true;
    //             }
    //             else
    //             {
    //                 Debug.Log("Boss_Attack1State: Ÿ�� ����");
    //                 hasAttacked = false;
    //             }
    //             hasEvaluated = true;
    //         };
    //     }
    // }
}
