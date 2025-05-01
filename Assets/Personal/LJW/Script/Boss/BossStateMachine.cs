using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; private set; }

    public void Initialize(BossState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BossState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
