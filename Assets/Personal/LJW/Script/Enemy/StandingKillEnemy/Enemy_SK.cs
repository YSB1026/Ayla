using UnityEngine;
using System.Collections;

public class Enemy_SK : Enemy
{
    [Header("Player ����")]
    public Transform player;
    public float positionTolerance = 0.5f; // �������� �ʾҴٰ� ������ ��� ������

    public Transform spawnPoint; // ó�� �ڸ� �����

    public float stayTimeToTrigger = 2f;
    public float stayTimer = 0f;
    private Vector2 lastPlayerPosition;

    public bool isDead = false;

    private SpriteRenderer sr;

    #region States
    public Enemy_SKIdleState idleState { get; private set; }
    public Enemy_SKAppearState appearState { get; private set; }
    public Enemy_SKAttackState attackState { get; private set; }
    public Enemy_SKDeathState deathState { get; private set; }

    public Enemy_SK_SAppearState sappearState { get; private set; }
    public Enemy_SK_SIdleState sidleState { get; private set; }
    public Enemy_SK_SDisappearState sdisappearState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_SKIdleState(this, stateMachine, "Idle", this);
        appearState = new Enemy_SKAppearState(this, stateMachine, "Appear", this);
        attackState = new Enemy_SKAttackState(this, stateMachine, "Attack", this);
        deathState = new Enemy_SKDeathState(this, stateMachine, "Die", this);

        sappearState = new Enemy_SK_SAppearState(this, stateMachine, "SAppear", this);
        sidleState = new Enemy_SK_SIdleState(this, stateMachine, "SIdle", this);
        sdisappearState = new Enemy_SK_SDisappearState(this, stateMachine, "SDisappear", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(sidleState);
    }

    protected override void Update()
    {
        base.Update(); // �θ� ȣ��
        stateMachine.currentState.Update();

        DetectPlayerStillness();    // appear ����
        DetectPlayerMovement(); // �����̸� �� �����
    }

    public void DetectPlayerStillness()
    {
        float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);

        if (movedDistance < positionTolerance)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= stayTimeToTrigger)
            {
                if (stateMachine.currentState is Enemy_SK_SIdleState)
                {
                    stateMachine.ChangeState(sdisappearState);
                }
                
            }
        }
        else
        {
            stayTimer = 0f;
        }

        if (isDead) return; // �̹� �׾����� �ƹ��͵� ���� ����
    }

    public void DetectPlayerMovement()
    {
        float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);

        bool shouldDieNow = stateMachine.currentState is Enemy_SKAppearState
                 || stateMachine.currentState is Enemy_SKIdleState
                 || stateMachine.currentState is Enemy_SKAttackState;

        // Death ����
        if (movedDistance >= positionTolerance && shouldDieNow)
        {
            stateMachine.ChangeState(deathState);
        }

        // lastPlayerPosition�� ���°� ���� ������ ���� ����
        if (!(stateMachine.currentState is Enemy_SKDeathState))
        {
            lastPlayerPosition = player.position;
        }
    }

    public bool IsPlayerMoving()
    {
        float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);
        lastPlayerPosition = player.position;
        return movedDistance > positionTolerance;
    }

    public override void HitPlayer()
    {
        attackState.HitPlayer();
    }
}
