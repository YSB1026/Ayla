using UnityEngine;

public class Shadow_WalkState : ShadowState
{
    public Shadow_WalkState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
        : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // 속도 적용
        shadow.SetVelocity(xInput * shadow.moveSpeed, shadow.rb.linearVelocity.y);

        // 입력이 멈추면 Idle로 전환
        if (xInput == 0)
        {
            stateMachine.ChangeState(shadow.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}