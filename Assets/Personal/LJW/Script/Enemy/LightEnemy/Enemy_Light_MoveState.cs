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

        // �� �ȿ� ������ ����
        if (enemyLight.isInLight)
        {
            enemy.SetZeroVelocity();
            return;
        }

        // �÷��̾� �������� �̵�
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
