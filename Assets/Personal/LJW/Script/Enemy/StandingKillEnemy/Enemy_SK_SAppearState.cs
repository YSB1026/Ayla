using UnityEngine;

public class Enemy_SK_SAppearState : EnemyState
{
    private Enemy_SK enemySK;
    public Enemy_SK_SAppearState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = enemySK;
    }

    public override void Enter()
    {
        base.Enter();

        enemySK.isDead = false;
        enemySK.transform.position = enemySK.spawnPoint.position;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationEndTrigger()
    {
        stateMachine.ChangeState(enemySK.sidleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
