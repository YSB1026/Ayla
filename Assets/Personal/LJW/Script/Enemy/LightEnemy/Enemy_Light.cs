using UnityEngine;

public class Enemy_Light : Enemy
{
    [Header("추가 정보")]
    public Transform player;
    public float Range = 5f;         // 감지 거리
    public float speedMultiplier = 5f;    // 속도 증가 배율

    public bool isInLight { get; private set; }

    #region States
    public Enemy_Light_MoveState moveState { get; private set; }
    public Enemy_Light_AttackState attackState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        moveState = new Enemy_Light_MoveState(this, stateMachine, "Move");
        attackState = new Enemy_Light_AttackState(this, stateMachine, "Attack");
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

    public void SetInLight(bool inLight)
    {
        isInLight = inLight;

        if (anim != null)
        {
            anim.speed = isInLight ? 0 : 1;
        }
    }

    private void FixedUpdate()
    {
        UpdateSpeedBasedOnPlayerDistance();
    }

    private void UpdateSpeedBasedOnPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어가 범위 안에 들어오면 속도 증가, 아니면 기본 속도
        if (distanceToPlayer <= Range)
        {
            moveSpeed = defaultMoveSpeed * speedMultiplier;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

}
