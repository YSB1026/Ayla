using UnityEngine;

public class Ayla_IdleState : AylaState
{
    public Ayla_IdleState(Ayla ayla, AylaStateMachine stateMachine)
        : base(ayla, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 필요하면 기본 색, 기본 이펙트 리셋 등
        Debug.Log("Ayla IdleState Enter");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
