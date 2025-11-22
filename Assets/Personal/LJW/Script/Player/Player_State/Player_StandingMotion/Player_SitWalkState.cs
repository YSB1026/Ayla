using UnityEngine;

//코드 안 쓸 예정
public class Player_SitWalkState : PlayerState
{
    public Player_SitWalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.sitWalkSpeed, rb.linearVelocityY);

            // 애니메이션 재생
            player.anim.speed = 1f;
        }
        else
        {
            player.SetZeroVelocity();

            // 애니메이션 정지
            player.anim.speed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // 애니메이션 재생 속도 복원
            player.anim.speed = 1f;

            stateMachine.ChangeState(player.inputState);
        }

        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //    stateMachine.ChangeState(player.crawlState);

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    // 애니메이션 재생 속도 복원
        //    player.anim.speed = 1f;

        //    stateMachine.ChangeState(player.standState);
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }

}
