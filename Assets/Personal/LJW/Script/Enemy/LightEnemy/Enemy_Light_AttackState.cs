using UnityEngine;

public class Enemy_Light_AttackState : EnemyState
{
    private Enemy_Light enemyLight;
    private bool attackSuccess = false;
    public Enemy_Light_AttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Light _enemylight) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyLight = _enemylight;
    }

    public override void Enter()
    {
        base.Enter();
        attackSuccess = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
    }

    public override void AnimationEndTrigger()
    {
        if (!attackSuccess)
        {
            // 아직도 공격 범위 안에 플레이어가 있다면 → 다시 공격
            if (enemyLight.IsPlayerInAttackBox())
            {
                stateMachine.ChangeState(enemyLight.attackState);
            }
            else
            {
                stateMachine.ChangeState(enemyLight.moveState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void SetAttackSuccess(bool success)
    {
        attackSuccess = success;
    }

}
