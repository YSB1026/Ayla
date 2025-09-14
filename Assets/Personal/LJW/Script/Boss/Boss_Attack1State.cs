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
            ThrowObject();
        }

        if (hit && !hasEvaluated)
            return;

        if (hit && hasEvaluated)
        {
            if (hasAttacked)
            {
                Debug.Log("보스 runState로 전환!");
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
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        // 애니메이션만 idle로 수동 전환
        boss.anim.SetBool("Idle", true);
    }

    public void ThrowObject()
    {
        GameObject obj = GameObject.Instantiate(boss.throwObjectPrefab, boss.throwSpawnPoint.position, Quaternion.identity);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // 방향 계산 (살짝 위)
        Vector2 targetPos = player.GetComponent<Collider2D>().bounds.center;
        Vector2 dir = (targetPos - (Vector2)obj.transform.position).normalized;
        float launchForce = 15f;

        // 보스 전용 스크립트로 처리
        ThrowingObjects controller = obj.GetComponent<ThrowingObjects>();
        if (controller != null)
        {
            controller.Setup(dir, player.transform, launchForce);

            controller.onHitPlayerCallback = (bool hit) =>
            {
                Debug.Log("콜백 도착, hasEvaluated 처리 시작");
                boss.longRCoolTimer = boss.longRCoolTime;

                if (hit)
                {
                    Debug.Log("Boss_Attack1State: 플레이어 타격 성공!");
                    hasAttacked = true;
                }
                else
                {
                    Debug.Log("Boss_Attack1State: 타격 실패");
                    hasAttacked = false;
                }
                hasEvaluated = true;
            };
        }
    }
}
