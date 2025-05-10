using UnityEngine;

public class Enemy_SKAttackState : EnemyState
{
    private Enemy_SK enemySK;
    public Enemy_SKAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK _enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = _enemySK;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

    }

    public void HitPlayer()
    {
        Player player = enemySK.player.GetComponent<Player>();

        if (player == null)
        {
            return;
        }
        //player.SetControlEnabled(false);
        player.stateMachine.ChangeState(player.deadState);
    }


    public override void Exit()
    {
        base.Exit();
    }
}
