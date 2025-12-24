using UnityEngine;

public class Shadow_PullState : ShadowState
{
    public Shadow_PullState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
        : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hit = shadow.GetObjectHitInfo();
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(false);
    }

    public override void Update()
    {
        base.Update();

        // 1. 물체 당기기 
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.MoveObject(-shadow.facingDir);

        // 2. 그림자 이동
        shadow.transform.position += new Vector3(shadow.grabSpeed * -shadow.facingDir * Time.deltaTime, 0, 0);

        // 3. 상태 전환
        if (Input.GetMouseButtonDown(0) || !shadow.IsObjectDetected())
            stateMachine.ChangeState(shadow.idleState);
        else if (xInput > 0 && shadow.facingDir == 1)
            stateMachine.ChangeState(shadow.pushState);
        else if (xInput < 0 && shadow.facingDir == -1)
            stateMachine.ChangeState(shadow.pushState);
        else if (xInput == 0)
            stateMachine.ChangeState(shadow.grabState);
    }

    public override void Exit()
    {
        base.Exit();
        shadow.SetZeroVelocity();
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(true);
    }
}