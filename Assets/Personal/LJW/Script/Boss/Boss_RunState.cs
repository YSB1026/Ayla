using UnityEngine;

public class Boss_RunState : BossState
{
    public Transform player;

    public Boss_RunState(Boss _boss, BossStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            this.player = player.transform;
    }

    public override void Update()
    {
        base.Update();
        
        if (boss.IsPlayerInCloseRange())
        {
            stateMachine.ChangeState(boss.attack2State);
            return;
        }

        if (player == null) return;

        if ((player.position.x > boss.transform.position.x && boss.facingDir == -1) ||
            (player.position.x < boss.transform.position.x && boss.facingDir == 1))
        {
            boss.Flip();  // 반대 방향일 때만 회전
        }

        boss.SetVelocity(boss.runSpeed * boss.facingDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
