using UnityEngine;

public class Player_WalkState : PlayerState
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

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
            PlayFootstepSound();
        }

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
        base.Exit();
    }
}
