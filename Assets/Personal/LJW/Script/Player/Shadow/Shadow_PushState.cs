using UnityEngine;

public class Shadow_PushState : ShadowState
{
    public Shadow_PushState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
        : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hit = shadow.GetObjectHitInfo();

        // 물체의 고정 풀기
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(false);
    }

    public override void Update()
    {
        base.Update();

        // 1. 물체 이동
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.MoveObject(shadow.facingDir);

        // 2. 그림자(본체) 이동
        shadow.transform.position += new Vector3(shadow.grabSpeed * shadow.facingDir * Time.deltaTime, 0, 0);

        // 3. 상태 전환 로직
        if (Input.GetMouseButtonDown(0) || !shadow.IsObjectDetected())
            stateMachine.ChangeState(shadow.idleState);
        else if (xInput < 0 && shadow.facingDir == 1)
            stateMachine.ChangeState(shadow.pullState);
        else if (xInput > 0 && shadow.facingDir == -1)
            stateMachine.ChangeState(shadow.pullState);
        else if (xInput == 0)
            stateMachine.ChangeState(shadow.grabState);
    }

    public override void Exit()
    {
        base.Exit();
        // 멈추고 물체 다시 고정
        shadow.SetZeroVelocity();
        hit.collider.gameObject.GetComponent<InteractiveObject>()?.FreezeObject(true);
    }
}