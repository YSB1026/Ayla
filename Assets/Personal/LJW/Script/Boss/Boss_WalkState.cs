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

        // 1) 타깃 찾기
        Transform target = null;

        if (boss.longRangeCheck != null)
        {
            if (boss.IsPlayerInLongRange())
            {
                currentSpeed = boss.runSpeed; // 숨지 않은 플레이어일 때만 가속
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

       /* // 2) 타깃이 보이면 그 방향으로만 회전
        if (target != null)
        {
            float dir = Mathf.Sign(target.position.x - boss.transform.position.x);
            if (dir != 0 && dir != boss.facingDir)
                boss.Flip();
        }*/

        // 3) 이동
        boss.SetVelocity(currentSpeed * boss.facingDir, rb.linearVelocity.y);

        // 4) 근접 범위 들어오면 배틀로 전환 (공격 등은 배틀에서 처리)
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.battleState);
            return;
        }

        // 5) 낭떠러지 등 발판 없음 → 반전 후 idle
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
