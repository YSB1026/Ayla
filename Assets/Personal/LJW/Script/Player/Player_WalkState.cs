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
        //else //�Ʒ� else if���̶� �ߺ� YSB �ڵ� ������.
        //    stateMachine.ChangeState(player.inputState);

        // YSB �ڵ� ���� ��.
        if (Input.GetKey(KeyCode.LeftShift)) //�޸���
            stateMachine.ChangeState(player.runState);
        else if (Input.GetKeyDown(KeyCode.Space)) //����
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

        if (footstepTimer <= 0f) // ������ ���� ���
        {
            SurfaceType surface = player.SurfaceType;
            // SoundManager.Instance.PlayFootstep(surface);
            footstepTimer = footstepInterval;
        }
    }
    #endregion
}
