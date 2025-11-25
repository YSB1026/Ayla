using UnityEngine;

public abstract class AylaState
{
    protected Ayla ayla;
    protected AylaStateMachine stateMachine;

    protected float stateTimer;

    protected AylaState(Ayla ayla, AylaStateMachine stateMachine)
    {
        this.ayla = ayla;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        stateTimer = 0f;
    }

    public virtual void Update()
    {
        stateTimer += Time.deltaTime;
    }

    public virtual void Exit()
    {
    }
}
