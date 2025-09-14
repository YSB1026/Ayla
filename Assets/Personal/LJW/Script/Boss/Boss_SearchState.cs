using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Boss_SearchState : BossState
{
    private Boss boss;

    public Boss_SearchState(Boss boss, BossStateMachine sm, string animBoolName) : base(boss, sm, animBoolName)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Search 상태 진입");
    }

    public override void Update()
    {
        base.Update();
        TransitionToIdle();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    private void TransitionToIdle()
    {
        if (boss.CanDetectPlayer())
        {
            stateMachine.ChangeState(boss.runState);
        }
        else
        {
            if (triggerCalled)
                stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.rb.linearVelocity = Vector2.zero;
    }
}
