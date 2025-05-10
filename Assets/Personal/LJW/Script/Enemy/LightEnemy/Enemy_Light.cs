using UnityEngine;

public class Enemy_Light : Enemy
{
    public LayerMask whatIsPlayer;

    [Header("�߰� ����")]
    public float Range = 5f;         // ���� �Ÿ�
    public float speedMultiplier = 5f;    // �ӵ� ���� ����

    [Header("�÷��̾� ���� info")]
    public Transform player;
    public Transform playerDetect;
    public Vector2 detectBoxSize = new Vector2(1.5f, 1f);  // ����: 1.5, ����: 1

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

    // ���� �Ÿ��ȿ����� �ӵ� ���� �Լ�
    private void UpdateSpeedBasedOnPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // �÷��̾ ���� �ȿ� ������ �ӵ� ����, �ƴϸ� �⺻ �ӵ�
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
                Debug.Log("���� ����: �÷��̾� ���");
                success = true;
            }
        }
        else
        {
            Debug.Log("���� ����");
        }

        // ���� state�� AttackState�� ���� ���� ���� ����
        if (stateMachine.currentState is Enemy_Light_AttackState attackState)
        {
            attackState.SetAttackSuccess(success);
        }
    }

    // �÷��̾� ���� ������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerDetect.position, detectBoxSize);
    }

}
