using UnityEngine;

public class Shadow_GrabState : ShadowState
{
    public Shadow_GrabState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
        : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hit = shadow.GetObjectHitInfo();

        if (hit.collider != null) Debug.Log(hit.collider.gameObject.name);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) || !shadow.IsObjectDetected())
            stateMachine.ChangeState(shadow.idleState);

        if (xInput > 0 && shadow.facingDir == 1)
            stateMachine.ChangeState(shadow.pushState);

        if (xInput > 0 && shadow.facingDir == -1)
            stateMachine.ChangeState(shadow.pullState);

        if (xInput < 0 && shadow.facingDir == 1)
            stateMachine.ChangeState(shadow.pullState);

        if (xInput < 0 && shadow.facingDir == -1)
            stateMachine.ChangeState(shadow.pushState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}