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

        // ��� ����
        GameObject.Find("DarknessOverlay").GetComponent<ScreenDarkener>().FadeToDark(6f);
    }

    public override void Update()
    {
        base.Update();

        idleTimer += Time.deltaTime;

        // �÷��̾ �����̸� ��� ����� ���� ��ȯ
        if (enemySK.IsPlayerMoving() && !attackPrepared)
        {
            GameObject.Find("DarknessOverlay").GetComponent<ScreenDarkener>().FadeToClear(0.01f);

            stateMachine.ChangeState(enemySK.deathState);
            return;
        }

        // 10�ʰ� ������ �־��ٸ� ���� �غ�
        if (idleTimer >= 10f && !attackPrepared)
        {
            attackPrepared = true;

            // ���� ����
            stateMachine.ChangeState(enemySK.attackState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
