using UnityEngine;

public class Boss_IdleState : BossState
{
    public Boss_IdleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Idle 상태 진입");
        stateTimer = boss.idleTime; // idle 유지 시간
    }

    public override void Update()
    {
        base.Update();

        if (boss.IsPlayerInAttackBox())
        {
            Debug.Log("플레이어 감지됨");
            stateMachine.ChangeState(boss.battleState);
            return;
        }

        if (stateTimer <= 0)
        {
            Debug.Log("Idle 시간 종료, Walk 상태로 전환");
            stateMachine.ChangeState(boss.walkState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
