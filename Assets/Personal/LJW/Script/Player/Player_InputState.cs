using UnityEngine;

public class Player_InputState : PlayerState
{
    public Player_InputState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();   // 입력 대기 상태
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);

        else if (Input.GetKey(KeyCode.LeftShift) && xInput != 0)
            stateMachine.ChangeState(player.runState);

        else if (Input.GetKeyDown(KeyCode.S))
            stateMachine.ChangeState(player.sitState);

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            stateMachine.ChangeState(player.walkState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
