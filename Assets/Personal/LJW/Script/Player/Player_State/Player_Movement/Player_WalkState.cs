using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_WalkState : Player_GroundedState
{
    public Player_WalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != this)
            return;

        // 공통 지상 이동 처리
        MoveHorizontally(player.moveSpeed);

        if (Input.GetKey(KeyCode.LeftShift))            // 달리기
            stateMachine.ChangeState(player.runState);
        else if (Input.GetKeyDown(KeyCode.Space))       // 점프
            stateMachine.ChangeState(player.jumpState);
        else if (xInput == 0 || player.IsWallDetected())// input
            stateMachine.ChangeState(player.inputState);
        /*else if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);*/
    }

    public override void Exit()
    {
        base.Exit();
    }
}
