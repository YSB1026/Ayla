using UnityEngine;

public class Player_StandState : PlayerState
{
    public Player_StandState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        var st = player.anim.GetCurrentAnimatorStateInfo(0);
        if (st.normalizedTime >= 0.95f)
            stateMachine.ChangeState(player.inputState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
