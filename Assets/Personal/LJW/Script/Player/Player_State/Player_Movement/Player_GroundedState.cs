using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
    }

    /// <summary>
    /// 지상에서 좌우 이동을 공통으로 처리하는 유틸 함수
    /// </summary>
    protected void MoveHorizontally(float speed)
    {
        if (xInput != 0 && !IsBlockedByWall())
        {
            player.SetVelocity(xInput * speed, rb.linearVelocityY);
        }
        else
        {
            player.SetZeroVelocity();
        }
    }

    public override void Exit() 
    { 
        base.Exit(); 
    }
}
