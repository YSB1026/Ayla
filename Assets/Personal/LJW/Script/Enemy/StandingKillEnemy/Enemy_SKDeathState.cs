using UnityEngine;
using System.Collections;

public class Enemy_SKDeathState : EnemyState
{
    private Enemy_SK enemySK;
    public Enemy_SKDeathState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK _enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = _enemySK;
    }
    public override void Enter()
    {
        base.Enter();

        enemySK.isDead = true;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationEndTrigger()
    {
        stateMachine.ChangeState(enemySK.sappearState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
