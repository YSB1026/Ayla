using UnityEngine;

public class Player_WalkState : PlayerState
{
    private float footstepTimer;
    private float footstepInterval = 0.45f;

    public Player_WalkState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        footstepTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    stateMachine.ChangeState(player.jumpState);
        //    return;
        //}

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
            PlayFootstepSound();
        }
        //else //아래 else if문이랑 중복 YSB 코드 수정함.
        //    stateMachine.ChangeState(player.inputState);

        // YSB 코드 수정 함.
        if (Input.GetKey(KeyCode.LeftShift)) //달리기
            stateMachine.ChangeState(player.runState);
        else if (Input.GetKeyDown(KeyCode.Space)) //점프
            stateMachine.ChangeState(player.jumpState);
        else if (xInput == 0 || player.IsWallDetected())//idle(input)
            stateMachine.ChangeState(player.inputState);
    }

    public override void Exit()
    {
        //SoundManager.Instance.PlayFootstep(player.SurfaceType);
        base.Exit();
        footstepTimer = 0f;
    }

    #region FootStep Sound
    private void PlayFootstepSound()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f) // 움직일 때만 재생
        {
            SurfaceType surface = player.SurfaceType;
            // SoundManager.Instance.PlayFootstep(surface);
            footstepTimer = footstepInterval;
        }
    }
    #endregion
}
