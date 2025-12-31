using UnityEngine;

public class Enemy_Light_MoveState : EnemyState
{
    private Enemy_Light enemyLight;

    public Enemy_Light_MoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Light _enemylight) 
        : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyLight = _enemylight;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // 빛 안에 있으면 정지
        if (enemyLight.isInLight)
        {
            enemy.SetZeroVelocity();
            return;
        }

        // 공격 범위에 플레이어가 있으면 공격 상태로 전환
        if (enemyLight.IsPlayerInAttackRange())
        {
            // 공격 전환 시 즉시 멈춤
            enemy.rb.linearVelocity = Vector2.zero;
            stateMachine.ChangeState(enemyLight.attackState);
            return;
        }

        // 현재 상태에 따라 동작
        switch (enemyLight.currentEnemyState)
        {
            case Enemy_Light.EnemyState.Idle:
                // 대기 상태 - 제자리에 멈춤
                enemy.SetZeroVelocity();
                break;

            case Enemy_Light.EnemyState.Patrol:
            case Enemy_Light.EnemyState.Chase:
                // 순찰 또는 추적 상태 - Enemy_Light의 Update에서 이미 velocity 설정
                // 여기서는 벽/낭떠러지 체크만 수행
                if (enemyLight.currentEnemyState == Enemy_Light.EnemyState.Patrol)
                {
                    if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
                    {
                        enemy.Flip();
                        enemyLight.ResetPatrolTimer(); // 방향 전환 시 타이머 리셋
                    }
                }
                break;

            case Enemy_Light.EnemyState.Attack:
                // 공격 상태는 AttackState에서 처리
                enemy.SetZeroVelocity();
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}