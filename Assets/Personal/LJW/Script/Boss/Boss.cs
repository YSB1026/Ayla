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

    #region States
    public BossStateMachine stateMachine { get; protected set; }
    public Boss_IdleState idleState { get; private set; }
    public Boss_WalkState walkState { get; private set; }
    public Boss_BattleState battleState { get; private set; }
    public Boss_RunState runState { get; private set; }

    public Boss_Attack2State attack2State { get; protected set; }
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
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
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

    // 기존 감지 함수들 교체
    public bool IsPlayerInLongRange()
        => longRangeCheck && OverlapBoxHasVisiblePlayer(longRangeCheck.position, longRangeBoxSize);

    public bool IsPlayerInAttackBox()
        => playerDetect && OverlapBoxHasVisiblePlayer(playerDetect.position, detectBoxSize);

    public bool IsPlayerInCloseRange()
        => closeRangeCheck && OverlapBoxHasVisiblePlayer(closeRangeCheck.position, closeRangeBoxSize);

    /*public bool IsPlayerInAttackBox()
    => playerDetect && OverlapBoxHasVisiblePlayer(playerDetect.position, detectBoxSize);

    public bool IsPlayerInLongRange()
        => longRangeCheck && OverlapBoxHasVisiblePlayer(longRangeCheck.position, longRangeBoxSize);

    public bool IsPlayerInCloseRange()
        => closeRangeCheck && OverlapBoxHasVisiblePlayer(closeRangeCheck.position, closeRangeBoxSize);*/


    public bool CanDetectPlayer()
    {
        // 공격 감지는 쿨다운 아닐 때만
        return IsPlayerInLongRange() || IsPlayerInAttackBox() || IsPlayerInCloseRange();
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

    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void HitTrigger() => stateMachine.currentState.HitTrigger();
}
