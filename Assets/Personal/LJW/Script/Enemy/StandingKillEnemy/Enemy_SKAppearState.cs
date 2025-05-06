using UnityEngine;
using System.Collections;

public class Enemy_SKAppearState : EnemyState
{
    private Enemy_SK enemySK;
    public Enemy_SKAppearState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK _enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = _enemySK;
    }

    public override void Enter()
    {
        base.Enter();
        enemySK.transform.position = enemySK.player.position + Vector3.up * 1.5f;
    }

    public override void Update()
    {
        base.Update();

    }
    public override void AnimationEndTrigger()
    {
        stateMachine.ChangeState(enemySK.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
