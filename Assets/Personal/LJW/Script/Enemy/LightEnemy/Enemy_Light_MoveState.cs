using UnityEngine;

public class Enemy_Light_MoveState : EnemyState
{
    private Enemy_Light enemyLight;

    public Enemy_Light_MoveState(Enemy_Light _enemyLight, EnemyStateMachine stateMachine, string animBoolName)
        : base(_enemyLight, stateMachine, animBoolName)
    {
        this.enemyLight = _enemyLight;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        // 빛 안에 있으면 멈춤
        if (enemyLight.isInLight)
        {
            enemy.SetZeroVelocity();
            return;
        }

        // 플레이어 방향으로 이동
        MoveTowardsPlayer();

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            // stateMachine.ChangeState(enemy.idleState);
        }
    }

    private void MoveTowardsPlayer()
    {
        if (enemyLight.player == null) return;

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
    }
}
