using UnityEngine;

public class Boss_BattleState : BossState
{
    private Transform player;
    private int moveDir;

    private bool isFinding = false;
    private bool findAnimDone = false;

    private Animator anim;

    public Boss_BattleState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (anim == null) anim = boss.GetComponentInChildren<Animator>();

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            this.player = player.transform;

        isFinding = false;
        findAnimDone = false;
    }

    public override void Update()
    {
        base.Update();

        /* // 1) 근접 범위면 바로 공격(Attack2)
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
         }*/

        // 수색 중일 때 처리
        if (isFinding)
        {
            // 수색 중 플레이어가 다시 보이면 즉시 추적/공격
            if (boss.IsPlayerInCloseRange())
            {
                stateMachine.ChangeState(boss.attack2State);
                return;
            }
            if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
            {
                isFinding = false;
                stateMachine.ChangeState(boss.walkState); // 다시 추적
                return;
            }

            // 애니메이션이 끝났다면 배회로
            if (findAnimDone)
            {
                stateMachine.ChangeState(boss.walkState);
                return;
            }

            // 수색 중에는 제자리(또는 네가 원하면 약간 이동)
            boss.SetVelocity(0f, rb.linearVelocity.y);
            return;
        }

        // 평소 배틀 로직
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        if (boss.IsPlayerInLongRange() || boss.IsPlayerInAttackBox())
        {
            stateMachine.ChangeState(boss.walkState);
            return;
        }

        // 여기서부터는 '이번 프레임에 플레이어가 안 보임' = 숨었을 가능성
        isFinding = true;
        findAnimDone = false;
        boss.SetVelocity(0f, rb.linearVelocity.y);
        if (anim != null) anim.Play("Finding_Player", 0, 0f);

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        findAnimDone = true;
    }


    public override void Exit()
    {
        base.Exit();
    }

}
