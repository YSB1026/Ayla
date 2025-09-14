using UnityEngine;

public class Player_InputState : PlayerState
{
    public Player_InputState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();   // �Է� ��� ����
    }
    public override void Update()
    {
        if (!player.controlEnabled)
            return;
        base.Update();


        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        else if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);
        else if (Input.GetKeyDown(KeyCode.F) && player.IsHidingSpotDetected())
            stateMachine.ChangeState(player.hideState);
        else if (Input.GetKeyDown(KeyCode.F) && player.IsObjectDetected())
            stateMachine.ChangeState(player.grabState);
        else if (Input.GetKeyDown(KeyCode.S))
            stateMachine.ChangeState(player.sitState);
        else if (!IsBlockedByWall() && Input.GetKey(KeyCode.LeftShift) && xInput != 0)
            stateMachine.ChangeState(player.runState);
        else if (!IsBlockedByWall() && xInput != 0)
            stateMachine.ChangeState(player.walkState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
