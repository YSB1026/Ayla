using UnityEngine;

public class Boss_BattleState : BossState
{
    private Transform player;
    private int moveDir;
    public Boss_BattleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            this.player = player.transform;
    }

    public override void Update()
    {
        base.Update();

        // 1) 근접 범위면 바로 공격(Attack2)
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        // 2) 멀리서만 보이거나(노란 박스) 일반 시야 박스에만 닿으면 추적 시작
        if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.walkState);
            return;
        }

        // 감지가 안 되면 idle로 복귀
        if (!boss.IsPlayerInAttackBox())
        {
            Debug.Log("플레이어 감지 안됨. Idle 전환");
            stateMachine.ChangeState(boss.idleState);
            return;
        }

        // 쿨 다운 중 + 빨간 박스 감지됨 → run
        if (boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.runState);
            return;
        }

        if (boss.ShouldEnterSearchOnLost())
        {
            stateMachine.ChangeState(boss.searchState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
