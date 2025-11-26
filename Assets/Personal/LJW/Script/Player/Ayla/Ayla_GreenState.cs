using UnityEngine;

public class Ayla_GreenState : AylaState
{
    private float duration = 0.5f;

    public Ayla_GreenState(Ayla ayla, AylaStateMachine stateMachine)
        : base(ayla, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ayla Green ability activated");

        // 되돌리기 로직
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
