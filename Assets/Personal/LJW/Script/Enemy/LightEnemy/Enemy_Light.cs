using UnityEngine;

public class Enemy_Light : Enemy
{
    public LayerMask whatIsPlayer;

    [Header("추가 정보")]
    public float Range = 5f;         // 감지 거리
    public float speedMultiplier = 5f;    // 속도 증가 배율

    [Header("플레이어 감지 info")]
    public Transform player;
    public Transform playerDetect;
    public Vector2 detectBoxSize = new Vector2(1.5f, 1f);  // 가로: 1.5, 세로: 1

    public bool isInLight { get; private set; }

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
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    private void FixedUpdate()
    {
        UpdateSpeedBasedOnPlayerDistance();
    }

    public void SetInLight(bool inLight)
    {
        isInLight = inLight;

        if (anim != null)
        {
            anim.speed = isInLight ? 0 : 1;
        }
    }

    // 일정 거리안에서는 속도 증가 함수
    private void UpdateSpeedBasedOnPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어가 범위 안에 들어오면 속도 증가, 아니면 기본 속도
        moveSpeed = distanceToPlayer <= Range ? defaultMoveSpeed * speedMultiplier : defaultMoveSpeed;
    }

    public bool IsPlayerInAttackBox()
    {
        return Physics2D.OverlapBox(playerDetect.position, detectBoxSize, 0, whatIsPlayer);
    }

    public override void HitPlayer()
    {
        bool success = false;

        Collider2D hit = Physics2D.OverlapBox(playerDetect.position, detectBoxSize, 0, whatIsPlayer);
        if (hit != null)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.stateMachine.ChangeState(player.deadState);
                // player.SetControlEnabled(false);
                Debug.Log("공격 성공: 플레이어 사망");
                success = true;
            }
        }
        else
        {
            Debug.Log("공격 실패");
        }

        // 현재 state가 AttackState일 때만 성공 여부 전달
        if (stateMachine.currentState is Enemy_Light_AttackState attackState)
        {
            attackState.SetAttackSuccess(success);
        }
    }

    // 플레이어 감지 기지모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetect.position, detectBoxSize);
    }

}
