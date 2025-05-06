using UnityEngine;

public class Enemy_SKIdleState : EnemyState
{
    private Enemy_SK enemySK;

    private float idleTimer;
    private bool attackPrepared;
    public Enemy_SKIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_SK _enemySK) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySK = _enemySK;
    }

    public override void Enter()
    {
        base.Enter();

        idleTimer = 0f;
        attackPrepared = false;

        // 어둠 시작
        GameObject.Find("DarknessOverlay").GetComponent<ScreenDarkener>().FadeToDark(6f);
    }

    public override void Update()
    {
        base.Update();

        idleTimer += Time.deltaTime;

        // 플레이어가 움직이면 밝게 만들고 상태 전환
        if (enemySK.IsPlayerMoving() && !attackPrepared)
        {
            GameObject.Find("DarknessOverlay").GetComponent<ScreenDarkener>().FadeToClear(0.01f);

            stateMachine.ChangeState(enemySK.deathState);
            return;
        }

        // 10초간 가만히 있었다면 공격 준비
        if (idleTimer >= 10f && !attackPrepared)
        {
            attackPrepared = true;

            // 공격 시작
            stateMachine.ChangeState(enemySK.attackState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
