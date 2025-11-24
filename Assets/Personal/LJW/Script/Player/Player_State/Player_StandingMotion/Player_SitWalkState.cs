using UnityEngine;

public class Player_SitWalkState : Player_GroundedState
{
    public Player_SitWalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 이동 시작할 때 앉은 상태 유지 
        player.SetSitCollider();

        // 앉은 걷기 애니 속도 보정 
        if (player.anim != null)
            player.anim.speed = 1f;
    }
    public override void Update()
    {
        base.Update();

        // GroundedState가 상태 바꿨으면 더 이상 이 상태 로직 진행 X 
        if (stateMachine.currentState != this)
            return;

        // 좌우 입력 없으면 그냥 앉기 상태로 돌아가기 
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.inputState);
            return;
        }

        // 좌우 이동 
        MoveHorizontally(player.sitWalkSpeed);

        // 일어나기 (W) 
        if (Input.GetKeyDown(KeyCode.W))
        {
            stateMachine.ChangeState(player.standState);
            return;
        }

        // 벽 감지 → 앉기 상태로 
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.inputState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 애니 속도 원복
        if (player.anim != null)
            player.anim.speed = 1f;
    }

}
