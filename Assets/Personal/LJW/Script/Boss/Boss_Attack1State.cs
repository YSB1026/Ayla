using UnityEngine;

public class Boss_Attack1State : BossState
{
    private bool hasAttacked = false;
    private float attackTimer = 1f;
    public Boss_Attack1State(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
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
        hasAttacked = false;
        attackTimer = 1f;

        attackTimer -= Time.deltaTime;

        // 공격 끝났지만 명중 못했으면 idle로 복귀
        if (triggerCalled && !hasAttacked)
        {
            Debug.Log("Attack1 실패, idle로 전환");
            boss.longRCoolTimer = boss.longRCoolTime;
            stateMachine.ChangeState(boss.idleState);
        }

        // 공격 끝났고 명중했으면 run이나 다음 행동
        if (triggerCalled && hasAttacked)
        {
            stateMachine.ChangeState(boss.runState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

}
