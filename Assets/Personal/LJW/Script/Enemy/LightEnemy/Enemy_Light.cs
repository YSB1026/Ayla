using UnityEngine;

public class Enemy_Light : Enemy
{
    [Header("�߰� ����")]
    public Transform player;
    public float Range = 5f;         // ���� �Ÿ�
    public float speedMultiplier = 5f;    // �ӵ� ���� ����

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

        // �÷��̾ ���� �ȿ� ������ �ӵ� ����, �ƴϸ� �⺻ �ӵ�
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
