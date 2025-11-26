using UnityEngine;

public class Ayla_RedState : AylaState
{
    private float duration = 0.4f;

    public Ayla_RedState(Ayla ayla, AylaStateMachine stateMachine)
        : base(ayla, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ayla Red ability activated");

        // 나중에 빨강 능력 로직
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
        // 능력 종료 처리 필요하면 여기
    }
}
