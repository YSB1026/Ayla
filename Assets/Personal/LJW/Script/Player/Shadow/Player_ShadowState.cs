using UnityEngine;

public class Player_ShadowState : PlayerState
{
    public Player_ShadowState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 1. 플레이어를 멈춥니다.
        player.SetZeroVelocity();

        // 2. 그림자 능력을 켭니다.
        if (player.shadowAbility != null)
        {
            //player.shadowAbility.ActivateShadow(true);
        }
    }

    public override void Update()
    {
        base.Update();

        // 3. 이 상태에서는 움직일 수 없으므로 이동 코드(xInput 등)를 무시하고 계속 멈춥니다.
        player.SetZeroVelocity();

        // 4. Q키를 다시 누르면 원래 상태(Idle)로 돌아갑니다.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.inputState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 5. 상태를 나갈 때 그림자 능력을 끕니다.
        if (player.shadowAbility != null)
        {
            //player.shadowAbility.ActivateShadow(false);
        }
    }
}