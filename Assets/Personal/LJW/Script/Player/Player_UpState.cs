using UnityEngine;

public class Player_UpState : PlayerState
{
    public Player_UpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.stateMachine.ChangeState(player.inputState); // ���� ���� ���� ����
        player.SetControlEnabled(true); // ���� �ٽ� ���
    }

    public override void Exit()
    {
        base.Exit();
    }
}
