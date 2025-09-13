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

        // 1) 타깃 찾기: long range 우선, 없으면 일반 시야 박스
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
                currentSpeed = boss.runSpeed; // long range 감지 시 속도 부스트
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
                // 일반 시야에서는 평속으로 접근
                currentSpeed = boss.moveSpeed;
            }
        }

        // 2) 타깃이 보이면 그 방향으로만 회전
        if (target != null)
        {
            float dir = Mathf.Sign(target.position.x - boss.transform.position.x);
            if (dir != 0 && dir != boss.facingDir)
                boss.Flip();
        }

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
