using UnityEngine;

public class Player_DownState : PlayerState
{
    public Player_DownState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("�÷��̾� Down ���� ����");

        player.ForceSetControlEnabled(false);
    }

    public override void Update()
    {
        base.Update();

        player.ForceSetControlEnabled(false);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
