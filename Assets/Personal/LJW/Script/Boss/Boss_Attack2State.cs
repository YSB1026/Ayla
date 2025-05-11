using UnityEngine;

public class Boss_Attack2State : BossState
{
    private bool hasAttacked = false;
    public Boss_Attack2State(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        boss.SetZeroVelocity();

        if (!hasAttacked)
        {
            Collider2D hit = Physics2D.OverlapBox(boss.closeRangeCheck.position, boss.closeRangeBoxSize, 0, boss.whatIsPlayer);
            if (hit != null)
            {
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    //player.stateMachine.ChangeState(player.deadState);
                    player.Die();
                    Debug.Log("Attack2 - 플레이어 명중");
                }

                hasAttacked = true;
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
