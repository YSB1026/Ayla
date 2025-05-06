using UnityEngine;

public class Enemy_SK_SDisappearState : EnemyState
{
    private Enemy_SK enemySK;
    public Enemy_SK_SDisappearState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = enemySK;
    }

    public override void Enter()
    {
        base.Enter();
        enemySK.transform.position = enemySK.spawnPoint.position;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationEndTrigger()
    {
        stateMachine.ChangeState(enemySK.appearState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
