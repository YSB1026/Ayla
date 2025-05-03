using UnityEngine;

public class Boss_IdleState : BossState
{
    public Boss_IdleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = boss.idleTime; // idle 유지 시간
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(boss.walkState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
