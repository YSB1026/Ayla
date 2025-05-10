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

        player.stateMachine.ChangeState(player.inputState); // 조작 가능 상태 복귀
        player.SetControlEnabled(true); // 조작 다시 허용
    }

    public override void Exit()
    {
        base.Exit();
    }
}
