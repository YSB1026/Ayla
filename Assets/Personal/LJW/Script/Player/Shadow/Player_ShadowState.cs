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

        // 1. 플레이어를 멈추기
        player.SetZeroVelocity();

        // 2. 그림자 능력을 켜기
        if (player.shadowAbility != null)
        {
            // 플레이어의 현재 위치를 전달하며 키기
            player.shadowAbility.ActivateShadow(player.transform.position);
        }
    }

    public override void Update()
    {
        base.Update();

        // 3. 이동 코드 무시
        player.SetZeroVelocity();

        // 4. Q키를 다시 누르면 원래 상태(Idle)로 복귀
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.inputState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 5. 상태를 나갈 때 그림자 능력을 끄기
        if (player.shadowAbility != null)
        {
            player.shadowAbility.DeactivateShadow();
        }
    }
}