using UnityEngine;
using System.Collections;

public class Enemy_SK : Enemy
{
    [Header("Player ����")]
    public Transform player;

    [Tooltip("�� �� ���Ϸ� �����̸� '������ ����'���� ����")]
    public float positionTolerance = 0.25f;

    [Header("����")]
    public Transform spawnPoint; // ó�� �ڸ� �����

    [Header("���� ����")]
    public float stayTimeToTrigger = 2f;

    // ���� ����
    public float stayTimer = 0f;
    private Vector2 lastPlayerPosition;
    private Vector2 appearAnchorPos;   // ������ "�� ����"�� �÷��̾� ��ġ ������
    private bool isPresent = false;    // ���� �÷��̾� �Ӹ� ���� �� �ִ°�
    private EnemyState prevState;  // �� ������ ���� ����

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
        base.Update(); // �θ� ȣ��
       
        var cur = stateMachine.currentState;

        // ���� ���·� �� ������ ���� ����
        if (!isPresent && (cur is Enemy_SKAppearState || cur is Enemy_SKIdleState || cur is Enemy_SKAttackState))
        {
            if (player != null) appearAnchorPos = player.position;
            isPresent = true;
            // ���� ���Ŀ��� lastPlayerPosition�� �������� �ʵ��� ����(�Ʒ����� ó��)
        }

        // �����(Death)���� �����ϸ� ü�� ���� �÷���
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

        if (isDead) return; // �̹� �׾����� �ƹ��͵� ���� ����
    }

    public void DetectPlayerMovement()
    {
       // ���� ���� ���� ������ ����
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

        // ���� ��(���� �ܰ�)������ ������ ���� ��ġ ����
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
            // ���� ��: ������ �� �̵������� �Ǵ� + ���� ����
            float movedDistance = Vector2.Distance(player.position, lastPlayerPosition);
            lastPlayerPosition = player.position;
            return movedDistance > positionTolerance;
        }
        else
        {
            // ���� ��: '������'���� �Ÿ��θ� �Ǵ�(���� ���� ����)
            float movedSinceAppear = Vector2.Distance(player.position, appearAnchorPos);
            return movedSinceAppear > positionTolerance;
        }*/
        if (isPresent)
        {
            // ���� ��: �������� �� (���� ���� ����)
            return Vector2.Distance(player.position, appearAnchorPos) > positionTolerance;
        }
        else
        {
            // ���� ��: ������ �� �� (���� ����)
            float d = Vector2.Distance(player.position, lastPlayerPosition);
            lastPlayerPosition = player.position;
            return d > positionTolerance;
        }
    }

    public override void HitPlayer()
    {
        if (player == null) return;

        // ���� ���¿����� �ǹ� ����. ���������� Ÿ�� �� ��.
        if (isPresent)
        {
            float movedSinceAppear = Vector2.Distance(player.position, appearAnchorPos);
            if (movedSinceAppear > positionTolerance)
            {
                // ���⼭�� Ÿ������ ����. ���� ���� ��ũ��Ʈ���� �ٷ� ����� ���·� �Ѱ�.
                return;
            }
        }

        // ���� �ϴ� Ÿ��
        attackState.HitPlayer();
    }

    public void OnVanishComplete()
    {
        isPresent = false;
        if (player != null) lastPlayerPosition = player.position;
        if (spawnPoint != null) transform.position = spawnPoint.position;
    }

}
