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



        if (boss.IsPlayerInCloseRange())    // attack2
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        if (boss.CanDetectLongRange && boss.IsPlayerInLongRange())  // attack1
        {
            stateMachine.ChangeState(boss.attack1State);
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

    }

    public override void Exit()
    {
        base.Exit();
    }

}
