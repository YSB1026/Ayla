using UnityEngine;

public class Player_InputState : PlayerState
{
    public Player_InputState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();   // 입력 대기 상태에서 속도 항상 초기화
    }
    public override void Update()
    {
        if (!player.controlEnabled)
            return;

        base.Update();

        // 공중 전환
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        // 점프
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        // 숨기 진입
        else if (Input.GetMouseButtonDown(0) && player.IsHidingSpotDetected())
        {
            stateMachine.ChangeState(player.hideState);
        }
        // 앉기
        else if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.ChangeState(player.sitState);
        }
        // 달리기
        else if (!IsBlockedByWall() && Input.GetKey(KeyCode.LeftShift) && xInput != 0)
        {
            stateMachine.ChangeState(player.runState);
        }
        // 걷기 
        else if (!IsBlockedByWall() && xInput != 0)
        {
            stateMachine.ChangeState(player.walkState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
