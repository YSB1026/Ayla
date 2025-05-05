using UnityEngine;

public class Enemy_Light_MoveState : EnemyState
{
    private Transform player;
    private Enemy_Light enemyLight;
    private int Enemy_moveDir;

    public Enemy_Light_MoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Light _enemylight) : base(_enemy, _stateMachine, _animBoolName)
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

        // ∫˚ æ»ø° ¿÷¿∏∏È ∏ÿ√„
        if (enemyLight.isInLight)
        {
            enemy.SetZeroVelocity();
            return;
        }

        if (enemyLight.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(enemyLight.attackState);
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
