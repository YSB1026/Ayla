using UnityEngine;

public class Shadow_IdleState : ShadowState
{
    public Shadow_IdleState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
        : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        shadow.SetZeroVelocity(); // 멈춤
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            stateMachine.ChangeState(shadow.walkState);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (shadow.IsObjectDetected())
                stateMachine.ChangeState(shadow.grabState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}