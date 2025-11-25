using UnityEngine;

public class AylaStateMachine
{
    public AylaState currentState { get; private set; }

    public void Initialize(AylaState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(AylaState newState)
    {
        if (currentState == newState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
