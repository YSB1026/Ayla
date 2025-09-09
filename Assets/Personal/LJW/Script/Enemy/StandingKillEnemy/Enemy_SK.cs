using UnityEngine;
using System.Collections;

public class Enemy_SK : Enemy
{
    [Header("Player 감지")]
    public Transform player;

    [Tooltip("이 값 이하로 움직이면 '가만히 있음'으로 간주")]
    public float positionTolerance = 0.25f;

    [Header("복귀")]
    public Transform spawnPoint; // 처음 자리 저장용

    [Header("등장 조건")]
    public float stayTimeToTrigger = 2f;

    // 내부 상태
    public float stayTimer = 0f;
    private Vector2 lastPlayerPosition;
    private Vector2 appearAnchorPos;   // 등장한 "그 순간"의 플레이어 위치 스냅샷
    private bool isPresent = false;    // 지금 플레이어 머리 위에 떠 있는가
    private EnemyState prevState;  // 전 프레임 상태 추적

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
       
        var cur = stateMachine.currentState;

        // 등장 상태로 막 진입한 순간 감지
        if (!isPresent && (cur is Enemy_SKAppearState || cur is Enemy_SKIdleState || cur is Enemy_SKAttackState))
        {
            if (player != null) appearAnchorPos = player.position;
            isPresent = true;
            // 등장 이후에는 lastPlayerPosition을 갱신하지 않도록 주의(아래에서 처리)
        }

        // 사라짐(Death)으로 진입하면 체류 종료 플래그
        if (cur is Enemy_SKDeathState && !(prevState is Enemy_SKDeathState))
        {
            isPresent = false;
        }

        DetectPlayerStillness();
        DetectPlayerMovement();
    }

    public void OnAppearEnter()
    {
        if (player != null) appearAnchorPos = player.position;
        isPresent = true;
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
       // 등장 중일 때는 스냅샷 기준
        float movedDistance = isPresent
            ? Vector2.Distance(player.position, appearAnchorPos)
            : Vector2.Distance(player.position, lastPlayerPosition);

        bool shouldDieNow = stateMachine.currentState is Enemy_SKAppearState
                         || stateMachine.currentState is Enemy_SKIdleState
                         || stateMachine.currentState is Enemy_SKAttackState;

        if (isPresent && shouldDieNow && movedDistance >= positionTolerance)
        {
            stateMachine.ChangeState(deathState);
        }

        // 등장 전(감시 단계)에서만 프레임 기준 위치 갱신
        if (!(stateMachine.currentState is Enemy_SKDeathState) && !isPresent)
        {
            lastPlayerPosition = player.position;
        }
    }

    public bool IsPlayerMoving()
    {
        if (player == null) return false;

        /*if (!isPresent)
        {
            // 등장 전: 프레임 간 이동량으로 판단 + 기준 갱신
            float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);
            lastPlayerPosition = player.position;
            return movedDistance > positionTolerance;
        }
        else
        {
            // 등장 후: '스냅샷'과의 거리로만 판단(기준 갱신 금지)
            float movedSinceAppear = Vector2.Distance(player.position, appearAnchorPos);
            return movedSinceAppear > positionTolerance;
        }*/
        if (isPresent)
        {
            // 등장 후: 스냅샷과 비교 (기준 갱신 금지)
            return Vector2.Distance(player.position, appearAnchorPos) > positionTolerance;
        }
        else
        {
            // 등장 전: 프레임 간 비교 (기준 갱신)
            float d = Vector2.Distance(player.position, lastPlayerPosition);
            lastPlayerPosition = player.position;
            return d > positionTolerance;
        }
    }

    public override void HitPlayer()
    {
        if (player == null) return;

        // 등장 상태에서만 의미 있음. 움직였으면 타격 안 함.
        if (isPresent)
        {
            float movedSinceAppear = Vector2.Distance(player.position, appearAnchorPos);
            if (movedSinceAppear > positionTolerance)
            {
                // 여기서는 타격하지 않음. 너의 상태 스크립트에서 바로 사라짐 상태로 넘겨.
                return;
            }
        }

        // 원래 하던 타격
        attackState.HitPlayer();
    }

    public void OnVanishComplete()
    {
        isPresent = false;
        if (player != null) lastPlayerPosition = player.position;
        if (spawnPoint != null) transform.position = spawnPoint.position;
    }

}
