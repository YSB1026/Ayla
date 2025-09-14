using UnityEngine;

public class Boss : Entity
{
    public LayerMask whatIsPlayer;

    [Header("이동 info")]
    public float moveSpeed = 1f;
    public float runSpeed = 5f;
    public float idleTime = 2f;

    [Header("플레이어 탐지 info")]
    [SerializeField]
    public Transform playerDetect;
    public Vector2 detectBoxSize = new Vector2(10f, 5f);

    [Header("원거리 공격(Attack1) info")]
    public Transform longRangeCheck;
    public Vector2 longRangeBoxSize = new Vector2(15f, 5f);

    [Header("근접 공격(Attack2) info")]
    public Transform closeRangeCheck;
    public Vector2 closeRangeBoxSize = new Vector2(2f, 3f);

    [Header("탐지 안정화")]
    [SerializeField] private float detectStickTime = 0.6f; // 감지 유지 시간(초)
    private float lastDetectTime = -999f;

    [Header("Long Range Cooldown")]
    public float longRCoolTime = 3f;
    public float longRCoolTimer = 0f;
    public bool CanDetectLongRange => longRCoolTimer <= 0f;

    [Header("기타")]
    public float battleTime;
    [SerializeField] private Transform graphics;

    // 마지막으로 본 플레이어 좌표와 플래그
    [Header("Search Settings")]
    public float searchStopDistance = 0.2f;    // 마지막 본 위치까지 접근 판정 거리
    public string findingAnimStateName = "Finding_Player";
    public float searchMaxWait = 3f;           // 애니메이션 이벤트 누락 대비 최대 대기
    /*    private bool wasSeeingPlayer = false;*/

    public Vector2 lastSeenPlayerPos;
    public bool hasLastSeenPlayerPos;

    #region States
    public BossStateMachine stateMachine { get; protected set; }
    public Boss_IdleState idleState { get; private set; }
    public Boss_WalkState walkState { get; private set; }
    public Boss_BattleState battleState { get; private set; }
    public Boss_RunState runState { get; private set; }
    public Boss_Attack2State attack2State { get; protected set; }

    public Boss_SearchState searchState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();
        idleState = new Boss_IdleState(this, stateMachine, "Idle");
        walkState = new Boss_WalkState(this, stateMachine, "Walk");
        battleState = new Boss_BattleState(this, stateMachine, "Battle");
        runState = new Boss_RunState(this, stateMachine, "Run");
        attack2State = new Boss_Attack2State(this, stateMachine, "Attack2");

        searchState = new Boss_SearchState(this, stateMachine, "Search");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        // 매 프레임 "보이는" 플레이어를 추적해서 마지막 좌표 갱신
        var vis = FindVisiblePlayer();
        bool seeing = (vis != null);

        // 보이는 동안엔 좌표만 갱신
        if (seeing)
        {
            lastDetectTime = Time.time;
            lastSeenPlayerPos = vis.transform.position;
            hasLastSeenPlayerPos = true;
        }
        else if (!seeing && hasLastSeenPlayerPos)
        {
            Vector2 to = lastSeenPlayerPos - (Vector2)transform.position;
            float dx = to.x;
            float ax = Mathf.Abs(dx);
            bool crossed = Mathf.Sign(dx) != Mathf.Sign(lastSeenPlayerPos.x - transform.position.x);

            // 바라보는 방향 정리
            if (dx != 0)
            {
                int dir = dx > 0 ? 1 : -1;
                if (dir != facingDir) Flip();
            }

            // 정지 후 찾기 애니메이션
            if (ax <= searchStopDistance || crossed)
            {
                Debug.Log($"ax : {ax}");
                rb.linearVelocity = Vector2.zero;
                stateMachine.ChangeState(searchState);
                hasLastSeenPlayerPos = false;
            }
            else
            {

                // 목표까지 전력 질주
                float dirX = dx > 0 ? 1f : -1f;
                rb.linearVelocity = new Vector2(dirX * runSpeed, rb.linearVelocity.y);
            }
        }

        // 나머지
        stateMachine.currentState.Update();
    }

    // 보이는 플레이어 찾기 (IsHidden이 false인 경우만)
    private Player FindVisiblePlayer()
    {
        // 장거리, 일반, 근거리 순으로 확인
        Player p;
        if (TryFindInBox(longRangeCheck, longRangeBoxSize, out p)) return p;
        if (TryFindInBox(playerDetect, detectBoxSize, out p)) return p;
        if (TryFindInBox(closeRangeCheck, closeRangeBoxSize, out p)) return p;
        return null;
    }

    private bool TryFindInBox(Transform t, Vector2 size, out Player p)
    {
        p = null;
        if (!t) return false;
        var hits = Physics2D.OverlapBoxAll(t.position, size, 0, whatIsPlayer);
        foreach (var h in hits)
        {
            var cand = h.GetComponentInParent<Player>();
            if (cand != null && !cand.IsHidden) // 숨으면 감지 제외됨
            {
                p = cand;
                return true;
            }
        }
        return false;
    }

    public override void Flip()
    {
        base.Flip();
        rb.linearVelocity = Vector2.zero; // 방향 전환 시 속도 제거 미끄러짐 방지
    }

    private bool OverlapBoxHasVisiblePlayer(Vector2 center, Vector2 size)
    {
        var hits = Physics2D.OverlapBoxAll(center, size, 0, whatIsPlayer);
        foreach (var h in hits)
        {
            var p = h.GetComponentInParent<Player>();
            if (p != null && !p.IsHidden)
                return true;
        }
        return false;
    }

    public bool IsPlayerInAttackBox()
    => playerDetect && OverlapBoxHasVisiblePlayer(playerDetect.position, detectBoxSize);

    public bool IsPlayerInLongRange()
        => longRangeCheck && OverlapBoxHasVisiblePlayer(longRangeCheck.position, longRangeBoxSize);

    public bool IsPlayerInCloseRange()
        => closeRangeCheck && OverlapBoxHasVisiblePlayer(closeRangeCheck.position, closeRangeBoxSize);


    public bool CanDetectPlayer()
    {
        // 공격 감지는 쿨다운 아닐 때만
        return IsPlayerInLongRange() || IsPlayerInAttackBox() || IsPlayerInCloseRange();
    }

    // 플레이어를 보다가 "방금 사라졌을 때" 탐색 상태로 전환할지 판단하는 헬퍼
    public bool ShouldEnterSearchOnLost()
    {
        // 최근에 본 적이 있어야 하고, 현재는 못 보고 있어야 함
        bool recentlySaw = (Time.time - lastDetectTime) < detectStickTime;
        return hasLastSeenPlayerPos && !CanDetectPlayer() && recentlySaw;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    // 노란 박스(장거리) 또는 일반 시야에 들어오면 Walk로 전환
    public bool ShouldEnterWalk()
    {
        bool now = IsPlayerInLongRange() || IsPlayerInAttackBox();
        if (now) lastDetectTime = Time.time;

        // 최근 detectStickTime초 동안은 계속 본 걸로 간주
        return now || (Time.time - lastDetectTime) < detectStickTime;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (playerDetect != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.TransformPoint(playerDetect.localPosition), detectBoxSize);
        }

        if (longRangeCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.TransformPoint(longRangeCheck.localPosition), longRangeBoxSize);
        }

        if (closeRangeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.TransformPoint(closeRangeCheck.localPosition), closeRangeBoxSize);
        }

        // 마지막 본 위치 시각화
        if (hasLastSeenPlayerPos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(lastSeenPlayerPos, 0.15f);
        }
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void HitTrigger() => stateMachine.currentState.HitTrigger();
}
