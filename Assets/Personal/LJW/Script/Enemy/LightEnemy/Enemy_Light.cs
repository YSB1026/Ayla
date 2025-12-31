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
    public Vector2 detectBoxSize = new Vector2(5f, 2f);

    [Header("탐지 안정화")]
    [SerializeField] private float detectStickTime = 0.6f; // 감지 유지 시간(초)
    private float lastDetectTime = -999f;

    [Header("공격 설정")]
    public float attackRange = 1.5f;  // 공격 사거리

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
    public float maxChaseDistance = 50f; // 이 거리 안에서는 박스를 벗어나도 계속 쫓음

    // 로직용 타겟 변수
    private Player currentTarget;

    public float searchWaitTime = 2.0f; // 도착 후 대기 시간
    private float currentSearchTime = 0f; // 타이머 계산용 변수

    // 원래 자리로 돌아가기 위한 변수 추가
    private Vector2 spawnPosition;
    private bool isReturningToStart = false;

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

        // 태어난 위치 저장
        spawnPosition = transform.position;

        stateMachine.Initialize(moveState);
        currentEnemyState = EnemyState.Patrol;
        patrolTimer = patrolTime;
    }

    protected override void Update()
    {
        base.Update();

        // 1. 이미 쫓고 있는 타겟이 있는지 검증
        if (currentTarget != null)
        {
            float dist = Vector2.Distance(transform.position, currentTarget.transform.position);

            // 추격 중단 조건: 플레이어가 숨었거나(IsHidden), 너무 멀어졌거나(maxChaseDistance)
            if (currentTarget.IsHidden || dist > maxChaseDistance)
            {
                Debug.Log("타겟 놓침! (숨음/거리초과)");

                lastDetectTime = Time.time;
                lastSeenPlayerPos = currentTarget.transform.position;
                hasLastSeenPlayerPos = true;

                currentTarget = null; // 타겟 해제
                player = null;
                // 타겟을 놓친 직후, 타이머 초기화
                currentSearchTime = 0f;
            }
        }

        // 2. 신규 탐지
        if (currentTarget == null)
        {
            currentTarget = FindVisiblePlayer();

            if (currentTarget != null)
            {
                Debug.Log("플레이어 발견! 추격 시작");
                player = currentTarget.transform;
            }
        }

        // 3. seeingPlayer 판정 : currentTarget 유무로 결정
        bool seeingPlayer = (currentTarget != null);

        // --- 상태 분기 로직 ---
        if (seeingPlayer)
        {
            // 플레이어 발견 및 추격 중
            isReturningToStart = false;
            hasLastSeenPlayerPos = true;
            currentSearchTime = 0f;

            lastDetectTime = Time.time;
            lastSeenPlayerPos = currentTarget.transform.position;

            float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                currentEnemyState = EnemyState.Attack;
                rb.linearVelocity = Vector2.zero;

                if (stateMachine.currentState != attackState)
                {
                    stateMachine.ChangeState(attackState);
                }
            }
            else
            {
                currentEnemyState = EnemyState.Chase;
                ChasePlayer(currentTarget.transform.position); // 타겟 위치로 이동

                if (stateMachine.currentState != moveState)
                {
                    stateMachine.ChangeState(moveState);
                }
            }
        }
        else if (hasLastSeenPlayerPos)
        {
            // 놓친 위치로 이동 및 대기
            Vector2 toTarget = lastSeenPlayerPos - (Vector2)transform.position;
            float xDistance = Mathf.Abs(toTarget.x);

            float arrivalThreshold = Mathf.Max(searchStopDistance, 1.0f);

            // 목표 지점에 도착했으면 순찰로 복귀
            if (xDistance <= arrivalThreshold)
            {
                // 1. 상태 강제 변경 (Chase -> Idle)
                currentEnemyState = EnemyState.Idle;

                // 2. 완전 정지
                rb.linearVelocity = Vector2.zero;

                // 3. 두리번거리는 시간 체크
                currentSearchTime += Time.deltaTime;

                // 대기 시간이 지났다면
                if (currentSearchTime >= searchWaitTime)
                {
                    Debug.Log("탐색 포기. 집으로 복귀합니다.");
                    hasLastSeenPlayerPos = false; // 탐색 종료
                    currentSearchTime = 0f;

                    // '복귀' 모드로 전환
                    isReturningToStart = true;
                }
            }
            else
            {
                // 아직 도착 못했으면 이동
                currentEnemyState = EnemyState.Chase;
                ChasePlayer(lastSeenPlayerPos);
                currentSearchTime = 0f;
            }

            if (stateMachine.currentState != moveState)
            {
                stateMachine.ChangeState(moveState);
            }

        }

        else if (isReturningToStart)
        {
            // 원래 위치(spawnPosition)로 복귀
            float distToHomeX = Mathf.Abs(transform.position.x - spawnPosition.x);

            // 도착 판정 범위
            float homeArrivalThreshold = Mathf.Max(searchStopDistance, 1.0f);

            if (distToHomeX <= homeArrivalThreshold)
            {
                // 1. 물리적 정지 및 상태 Idle로 변경
                rb.linearVelocity = Vector2.zero;
                currentEnemyState = EnemyState.Idle;

                // 2. 타이머 체크 (집에서 잠시 숨 고르기)
                if (currentSearchTime == 0f)
                {
                    Debug.Log("집 도착! 잠시 휴식 중...");
                }

                currentSearchTime += Time.deltaTime;

                // 3. 쉬는 시간(searchWaitTime)이 끝나면 순찰 시작
                if (currentSearchTime >= searchWaitTime)
                {
                    Debug.Log("휴식 끝. 다시 순찰 시작.");

                    isReturningToStart = false; // 복귀 모드 종료
                    currentSearchTime = 0f;     // 타이머 초기화

                    // 순찰 상태로 전환
                    currentEnemyState = EnemyState.Patrol;
                    patrolTimer = patrolTime;
                    isPatrolling = true;
                }

            }
            else
            {
                // 집으로 이동
                currentEnemyState = EnemyState.Chase; // 이동 모션
                ChasePlayer(spawnPosition);

                // 이동 중엔 타이머 0으로 유지
                currentSearchTime = 0f;
            }

            if (stateMachine.currentState != moveState) stateMachine.ChangeState(moveState);
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
        // 1순위: 현재 추격 중인 타겟이 있으면 그걸 기준으로 가속
        if (currentTarget != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.transform.position);

            // 범위 안이면 가속, 아니면 기본 속도
            if (distanceToPlayer <= Range)
            {
                applySpeed = defaultMoveSpeed * speedMultiplier;
            }
            else
            {
                applySpeed = defaultMoveSpeed;
            }
        }
        // 2순위: 혹시 Inspector에 등록된 player 변수가 있다면 그것도 체크 (안전장치)
        else if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            applySpeed = distanceToPlayer <= Range ? defaultMoveSpeed * speedMultiplier : defaultMoveSpeed;
        }
        // 3순위: 아무것도 없으면 기본 속도
        else
        {
            applySpeed = defaultMoveSpeed;
        }
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

    // 디버그용 UI
    /*private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40; // 글씨 크게
        style.normal.textColor = Color.red;

        string targetInfo = (currentTarget != null) ? "타겟: " + currentTarget.name : "타겟: 없음";

        // 화면 좌측 상단에 상태 표시
        GUI.Label(new Rect(50, 50, 400, 100), $"상태: {currentEnemyState}\n{targetInfo}", style);
    }*/
}