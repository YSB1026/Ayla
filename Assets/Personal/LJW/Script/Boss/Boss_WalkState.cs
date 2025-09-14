using UnityEngine;

public class Boss_WalkState : BossState
{
    public Boss_WalkState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        boss.SetVelocity(boss.moveSpeed * boss.facingDir, rb.linearVelocity.y);

        if (boss.CanDetectPlayer())
        {
            stateMachine.ChangeState(boss.battleState);
        }

        if (!boss.IsGroundDetected())
        {
            boss.Flip();
            stateMachine.ChangeState(boss.idleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
