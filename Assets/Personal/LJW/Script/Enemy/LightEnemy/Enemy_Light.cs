using UnityEngine;

public class Enemy_Light : Enemy
{
    public LayerMask whatIsPlayer;

    [Header("추가 설정")]
    public float Range = 5f;         // 가속 거리
    public float speedMultiplier = 5f;    // 속도 증가 배율

    [Header("플레이어 탐지 info")]
    public Transform player;
    public Transform playerDetect;
    public Vector2 detectBoxSize = new Vector2(1.5f, 1f);

    [Header("탐지 안정화")]
    [SerializeField] private float detectStickTime = 0.6f; // 감지 유지 시간(초)
    private float lastDetectTime = -999f;

    [Header("공격 설정")]
    public float attackRange = 1.5f;  // 공격 사거리
    public Vector2 attackBoxSize = new Vector2(1f, 1f);  // 공격 범위 박스

    [Header("순찰 설정")]
    public float patrolTime = 3f;      // 순찰 시간
    public float idleTime = 2f;        // 대기 시간
    private float patrolTimer = 0f;
    private float idleTimer = 0f;
    private bool isPatrolling = true;

    [Header("추적 설정")]
    public float searchStopDistance = 0.2f;    // 마지막 본 위치까지 접근 판정 거리
    public Vector2 lastSeenPlayerPos;
    public bool hasLastSeenPlayerPos;

    public bool isInLight { get; private set; }

    // 적 상태
    public enum EnemyState
    {
        Patrol,      // 순찰
        Idle,        // 대기
        Chase,       // 추적
        Attack       // 공격
    }

    public EnemyState currentEnemyState { get; private set; }

    #region States
    public Enemy_Light_MoveState moveState { get; private set; }
    public Enemy_Light_AttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        moveState = new Enemy_Light_MoveState(this, stateMachine, "Move", this);
        attackState = new Enemy_Light_AttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
        currentEnemyState = EnemyState.Patrol;
        patrolTimer = patrolTime;
    }

    protected override void Update()
    {
        base.Update();

        // 보이는 플레이어 추적
        var visiblePlayer = FindVisiblePlayer();
        bool seeingPlayer = (visiblePlayer != null);

        if (seeingPlayer)
        {
            // 플레이어 발견 시
            lastDetectTime = Time.time;
            lastSeenPlayerPos = visiblePlayer.transform.position;
            hasLastSeenPlayerPos = true;

            float distanceToPlayer = Vector2.Distance(transform.position, visiblePlayer.transform.position);

            // 공격 사거리 내에 있으면 공격 상태
            if (distanceToPlayer <= attackRange)
            {
                currentEnemyState = EnemyState.Attack;
                rb.linearVelocity = Vector2.zero; // 완전히 멈춤
                
                // 공격 상태로 전환
                if (stateMachine.currentState != attackState)
                {
                    stateMachine.ChangeState(attackState);
                }
            }
            else
            {
                // 공격 범위 밖이면 추적
                currentEnemyState = EnemyState.Chase;
                ChasePlayer(visiblePlayer.transform.position);
                
                // 이동 상태로 전환
                if (stateMachine.currentState != moveState)
                {
                    stateMachine.ChangeState(moveState);
                }
            }
        }
        else if (hasLastSeenPlayerPos)
        {
            // 플레이어를 잃어버린 경우 마지막 위치로 이동
            currentEnemyState = EnemyState.Chase;
            
            Vector2 toTarget = lastSeenPlayerPos - (Vector2)transform.position;
            float distance = toTarget.magnitude;

            // 목표 지점에 도착했으면 순찰로 복귀
            if (distance <= searchStopDistance)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                hasLastSeenPlayerPos = false;
                currentEnemyState = EnemyState.Patrol;
                patrolTimer = patrolTime;
            }
            else
            {
                ChasePlayer(lastSeenPlayerPos);
            }
            
            if (stateMachine.currentState != moveState)
            {
                stateMachine.ChangeState(moveState);
            }
        }
        else
        {
            // 플레이어가 없으면 순찰/대기 상태
            HandlePatrolAndIdle();
        }

        // 애니메이션 설정
        UpdateAnimation();

        stateMachine.currentState.Update();
    }

    private void FixedUpdate()
    {
        UpdateSpeedBasedOnPlayerDistance();
    }

    // 순찰과 대기 처리
    private void HandlePatrolAndIdle()
    {
        if (isPatrolling)
        {
            currentEnemyState = EnemyState.Patrol;
            patrolTimer -= Time.deltaTime;

            // 순찰 이동
            rb.linearVelocity = new Vector2(facingDir * applySpeed, rb.linearVelocity.y);

            if (patrolTimer <= 0)
            {
                // 대기 상태로 전환
                isPatrolling = false;
                idleTimer = idleTime;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            currentEnemyState = EnemyState.Idle;
            idleTimer -= Time.deltaTime;

            // 제자리 대기
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (idleTimer <= 0)
            {
                // 순찰 상태로 전환 및 방향 전환
                isPatrolling = true;
                patrolTimer = patrolTime;
                Flip();
            }
        }

        if (stateMachine.currentState != moveState)
        {
            stateMachine.ChangeState(moveState);
        }
    }

    // 순찰 타이머 리셋 (벽 감지 시 사용)
    public void ResetPatrolTimer()
    {
        patrolTimer = patrolTime;
    }

    // 플레이어 추적
    private void ChasePlayer(Vector2 targetPos)
    {
        Vector2 toTarget = targetPos - (Vector2)transform.position;

        // 방향 설정
        if (toTarget.x != 0)
        {
            int targetDir = toTarget.x > 0 ? 1 : -1;
            if (targetDir != facingDir)
            {
                Flip();
            }
        }

        // 이동
        float dirX = toTarget.x > 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(dirX * applySpeed, rb.linearVelocity.y);
    }

    // 애니메이션 업데이트
    private void UpdateAnimation()
    {
        if (anim == null) return;

        // 모든 bool 파라미터를 false로 초기화
        anim.SetBool("Move", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Attack", false);

        switch (currentEnemyState)
        {
            case EnemyState.Patrol:
            case EnemyState.Chase:
                anim.SetBool("Move", true);
                break;

            case EnemyState.Idle:
                anim.SetBool("Idle", true);
                break;

            case EnemyState.Attack:
                anim.SetBool("Attack", true);
                break;
        }
    }

    public void SetInLight(bool inLight)
    {
        isInLight = inLight;

        if (anim != null)
        {
            anim.speed = isInLight ? 0 : 1;
        }
    }

    // 플레이어 거리에 따른 속도 조절
    private void UpdateSpeedBasedOnPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어 범위 안에 있으면 속도 증가
        applySpeed = distanceToPlayer <= Range ? defaultMoveSpeed * speedMultiplier : defaultMoveSpeed;
    }

    // 보이는 플레이어 찾기 (IsHidden이 false인 경우만)
    private Player FindVisiblePlayer()
    {
        if (!playerDetect) return null;

        var hits = Physics2D.OverlapBoxAll(playerDetect.position, detectBoxSize, 0, whatIsPlayer);
        foreach (var hit in hits)
        {
            var p = hit.GetComponentInParent<Player>();
            if (p != null && !p.IsHidden) // 숨으면 감지 제외
            {
                return p;
            }
        }
        return null;
    }

    // 보이는 플레이어가 공격 범위에 있는지 확인
    public bool IsPlayerInAttackRange()
    {
        if (player == null) return false;
        
        Player targetPlayer = player.GetComponent<Player>();
        if (targetPlayer == null) return false;
        
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= attackRange && !targetPlayer.IsHidden;
    }

    // 보이는 플레이어가 감지 범위에 있는지 확인
    public bool IsPlayerInDetectBox()
    {
        if (!playerDetect) return false;

        var hits = Physics2D.OverlapBoxAll(playerDetect.position, detectBoxSize, 0, whatIsPlayer);
        foreach (var hit in hits)
        {
            var p = hit.GetComponentInParent<Player>();
            if (p != null && !p.IsHidden)
            {
                return true;
            }
        }
        return false;
    }

    // 플레이어를 감지할 수 있는지 (안정화 적용)
    public bool CanDetectPlayer()
    {
        bool nowDetecting = IsPlayerInDetectBox();
        
        if (nowDetecting)
        {
            lastDetectTime = Time.time;
        }

        // 최근 detectStickTime초 동안은 계속 감지된 것으로 간주
        return nowDetecting || (Time.time - lastDetectTime) < detectStickTime;
    }

    public override void Flip()
    {
        base.Flip();
        // 방향 전환 시 속도 제거로 미끄러짐 방지
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void HitPlayer()
    {
        bool success = false;

        // 공격 범위 내의 보이는 플레이어만 공격
        if (player != null)
        {
            Player targetPlayer = player.GetComponent<Player>();
            if (targetPlayer != null && !targetPlayer.IsHidden)
            {
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance <= attackRange)
                {
                    targetPlayer.stateMachine.ChangeState(targetPlayer.deadState);
                    Debug.Log("공격 성공: 플레이어 사망");
                    success = true;
                }
            }
        }

        if (!success)
        {
            Debug.Log("공격 실패");
        }

        // 현재 state가 AttackState인 경우 공격 결과 전달
        if (stateMachine.currentState is Enemy_Light_AttackState attackState)
        {
            attackState.SetAttackSuccess(success);
        }
    }

    // 디버그용 기즈모
    private void OnDrawGizmosSelected()
    {
        // 가속 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 탐지 범위
        if (playerDetect != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(playerDetect.position, detectBoxSize);
        }

        // 마지막 본 위치
        if (hasLastSeenPlayerPos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(lastSeenPlayerPos, 0.15f);
        }
    }

    public override void ApplyStun(float time)
    {
        throw new System.NotImplementedException();
    }
}