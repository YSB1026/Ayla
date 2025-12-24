using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        Debug.Log($"[Player State] {currentState.GetType().Name} -> {_newState.GetType().Name}");
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

}
