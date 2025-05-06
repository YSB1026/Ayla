using UnityEngine;
using System.Collections;

public class Enemy_SK : Enemy
{
    [Header("Player 감지")]
    public Transform player;
    public float positionTolerance = 0.5f; // 움직이지 않았다고 간주할 허용 오차값

    public Transform spawnPoint; // 처음 자리 저장용

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
        base.Update(); // 부모 호출
        stateMachine.currentState.Update();

        DetectPlayerStillness();    // appear 조건
        DetectPlayerMovement(); // 움직이면 적 사라짐
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

        if (isDead) return; // 이미 죽었으면 아무것도 하지 않음
    }

    public void DetectPlayerMovement()
    {
        float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);

        bool shouldDieNow = stateMachine.currentState is Enemy_SKAppearState
                 || stateMachine.currentState is Enemy_SKIdleState
                 || stateMachine.currentState is Enemy_SKAttackState;

        // Death 조건
        if (movedDistance >= positionTolerance && shouldDieNow)
        {
            stateMachine.ChangeState(deathState);
        }

        // lastPlayerPosition은 상태가 등장 이후일 때만 갱신
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
