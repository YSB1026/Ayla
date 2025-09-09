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

        // 1) 플레이어가 움직였는지 '항상' 먼저 본다
        if (enemySK.IsPlayerMoving())
        {
            // 바로 사라짐 애니(원위치 복귀)로 전환
            stateMachine.ChangeState(enemySK.deathState);
            return;
        }

        // 2) 안 움직였을 때만 대기 시간 누적 및 공격 준비 판단
        idleTimer += Time.deltaTime;

        if (!attackPrepared && idleTimer >= 10f)
        {
            attackPrepared = true;
            stateMachine.ChangeState(enemySK.attackState);
            return;
        }

        /*idleTimer += Time.deltaTime;

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
        }*/

    }

    public override void Exit()
    {
        base.Exit();
    }
}
