using UnityEngine;

public class Enemy_Light_MoveState : EnemyState
{
    private Transform player;
    private Enemy_Light enemyLight;
    private int Enemy_moveDir;

    public Enemy_Light_MoveState(Enemy_Light _enemyLight, EnemyStateMachine stateMachine, string animBoolName)
        : base(_enemyLight, stateMachine, animBoolName)
    {
        this.enemyLight = _enemyLight;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        // ∫˚ æ»ø° ¿÷¿∏∏È ∏ÿ√„
        if (enemyLight.isInLight)
        {
            enemy.SetZeroVelocity();
            return;
        }

        Vector2 dirToPlayer = (enemyLight.player.position - enemy.transform.position).normalized;
        enemy.SetVelocity(dirToPlayer.x * enemy.moveSpeed, rb.linearVelocity.y);
        enemy.FlipController(dirToPlayer.x);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            // stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
