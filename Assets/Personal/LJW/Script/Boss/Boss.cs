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

    [Header("원거리 무기 info")]
    public GameObject throwObjectPrefab;
    public Transform throwSpawnPoint;

    [Header("원거리 공격(Attack1) info")]
    public Transform longRangeCheck;
    public Vector2 longRangeBoxSize = new Vector2(15f, 5f);

    [Header("근접 공격(Attack2) info")]
    public Transform closeRangeCheck;
    public Vector2 closeRangeBoxSize = new Vector2(2f, 3f);

    [Header("Long Range Cooldown")]
    public float longRCoolTime = 3f;
    public float longRCoolTimer = 0f;

    public bool CanDetectLongRange => longRCoolTimer <= 0f;

    public float battleTime;

    [SerializeField] private Transform graphics;

    #region States
    public BossStateMachine stateMachine { get; private set; }
    public Boss_IdleState idleState { get; private set; }
    public Boss_WalkState walkState { get; private set; }
    public Boss_BattleState battleState { get; private set; }
    public Boss_RunState runState { get; private set; }
    public Boss_Attack1State attack1State { get; private set; }
    public Boss_Attack2State attack2State { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();
        idleState = new Boss_IdleState(this, stateMachine, "Idle");
        walkState = new Boss_WalkState(this, stateMachine, "Walk");
        battleState = new Boss_BattleState(this, stateMachine, "Battle");
        runState = new Boss_RunState(this, stateMachine, "Run");
        attack1State = new Boss_Attack1State(this, stateMachine, "Attack1");
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

        if (longRCoolTimer > 0f)
            longRCoolTimer -= Time.deltaTime;
    }
    public override void Flip()
    {
        base.Flip(); 
        rb.linearVelocity = Vector2.zero; // 방향 전환 시 속도 제거 미끄러짐 방지
    }

    public bool IsPlayerInAttackBox()
    {
        return Physics2D.OverlapBox(playerDetect.position, detectBoxSize, 0, whatIsPlayer);
    }

    public bool IsPlayerInLongRange()
    {
        return Physics2D.OverlapBox(longRangeCheck.position, longRangeBoxSize, 0, whatIsPlayer);
    }

    public bool IsPlayerInCloseRange()
    {
        return Physics2D.OverlapBox(closeRangeCheck.position, closeRangeBoxSize, 0, whatIsPlayer);
    }

    public bool CanDetectPlayer()
    {
        // 공격 감지는 쿨다운 아닐 때만
        return IsPlayerInAttackBox() || (CanDetectLongRange && IsPlayerInLongRange());
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
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
}
