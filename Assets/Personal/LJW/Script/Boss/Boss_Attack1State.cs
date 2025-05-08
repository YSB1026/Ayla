using UnityEngine;

public class Boss_Attack1State : BossState
{
    private bool hasAttacked = false;
    private bool hasThrown = false;
    public Boss_Attack1State(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        hasAttacked = false;
        hasThrown = false;
    }

    public override void Update()
    {
        base.Update();

        boss.SetZeroVelocity();

        if (!hasThrown)
        {
            hasThrown = true;
            ThrowObject();
        }

        if (triggerCalled)
        {
            boss.longRCoolTimer = boss.longRCoolTime;

            if (hasAttacked)
            {
                stateMachine.ChangeState(boss.runState);
            }
            else
            {
                Debug.Log("Attack1 실패, idle로 전환");
                stateMachine.ChangeState(boss.idleState);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ThrowObject()
    {
        GameObject obj = GameObject.Instantiate(boss.throwObjectPrefab, boss.throwSpawnPoint.position, Quaternion.identity);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // ★ 정확한 방향 계산 (플레이어 중심을 향하게)
        Vector2 targetPos = player.transform.position + Vector3.up * 0.5f;
        Vector2 dir = (targetPos - (Vector2)obj.transform.position).normalized;

        float launchForce = 15f;
        rb.gravityScale = 0f; // 던지는 동안 중력 X
        rb.linearVelocity = dir * launchForce;

        ThrowingObjects controller = obj.GetComponent<ThrowingObjects>();
        if (controller != null)
        {
            controller.SetPlayer(player);
        }
    }
}
