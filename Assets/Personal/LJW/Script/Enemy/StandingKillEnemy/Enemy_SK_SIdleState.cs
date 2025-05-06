using UnityEngine;

public class Enemy_SK_SIdleState : EnemyState
{
    private Enemy_SK enemySK;

    public Enemy_SK_SIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK enemySK ) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = enemySK;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
