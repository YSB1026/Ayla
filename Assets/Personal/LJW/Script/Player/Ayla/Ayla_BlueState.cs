using UnityEngine;

public class Ayla_BlueState : AylaState
{
    private float duration = 0.6f;

    public Ayla_BlueState(Ayla ayla, AylaStateMachine stateMachine)
        : base(ayla, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ayla Blue ability activated");

        // 정지 로직
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer >= duration)
        {
            stateMachine.ChangeState(ayla.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
