using System;
using UnityEngine;

public class Ayla : Entity
{
    [Header("��ġ ����")]
    public Transform player;          // �÷��̾�
    public Transform followPointLeft;       // �÷��̾� ����
    public Transform followPointRight;      // �÷��̾� ������

    [Header("�̵� ����")]
    public float followSmoothTime = 0.3f;   // ���󰡴� �ε巯�� ����
    public float floatSpeed = 2f;           // ���Ʒ� ���ٴϴ� �ӵ�
    public float floatAmplitude = 0.15f;    // ���ٴϴ� ����
    public float moveSpeed = 3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 followBasePosition;     // ���� ���� ��ġ

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer aylaSpriteRenderer;

    private bool controlEnabled = false;  // Ű ���� ����
    private bool isFixed = false;         // R Ű�� ���� ����
    public bool isCurrentlyControlled;  // ���� ���۵ǰ� �ִ��� ����

    [SerializeField] private float holdTime = 0f;
    [SerializeField] private float holdDuration = 2f;

    [Header("���϶� ������")]
    [SerializeField] private float aylaGauge = 100f;

    #region States
    public AylaStateMachine stateMachine { get; private set; }

    #endregion

    private Transform overrideTargetPoint = null;

    public void SetOverrideTarget(Transform target)
    {
        overrideTargetPoint = target;
    }

    public void SetControlEnabled(bool isEnabled)
    {
        controlEnabled = isEnabled;
    }

    protected override void Awake()
    {
        base.Awake();

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        aylaSpriteRenderer = GetComponent<SpriteRenderer>();

        // ���� �ӽ� �ν��Ͻ� ����
        stateMachine = new AylaStateMachine();
    }

    protected override void Start()
    {
        base.Start();
        followBasePosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState?.Update();

        FixedKey();

        if (isFixed)
        {
            Float();
            return;
        }

        Vector2 inputDir = Vector2.zero;

        if (controlEnabled)
        {
            // WASD ����
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            inputDir = new Vector2(x, y);

            followBasePosition += new Vector3(inputDir.x, inputDir.y, 0) * moveSpeed * Time.deltaTime;
        }
        else
        {
            Follow();
        }
        Float();
    }

    // �÷��̾� ����ٴϴ� ����
    private void Follow()
    {
        Transform targetPoint;

        if (overrideTargetPoint != null)
        {
            targetPoint = overrideTargetPoint;
        }
        else
        {
            targetPoint = playerSpriteRenderer.flipX ? followPointLeft : followPointRight;
        }

        if (targetPoint == null) return;

        followBasePosition = Vector3.SmoothDamp(followBasePosition, targetPoint.position, ref velocity, followSmoothTime);
        transform.position = followBasePosition;
    }

    // �յ� �ߴ� ȿ��
    private void Float()
    {
        // �սǵս� ȿ��
        Vector3 floatOffset = new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
        transform.position = followBasePosition + floatOffset;
    }

    // R Ű�� ���� ��Ű��
    private void FixedKey()
    {
        if (!isCurrentlyControlled)
            return;

        // R Ű�� ����
        if (Input.GetKey(KeyCode.R))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdDuration)
            {
                isFixed = !isFixed;         // ���� ���� ���
                controlEnabled = !isFixed;  // ���� ��Ȱ��ȭ

                if (isFixed)    // ����
                {
                    followBasePosition = transform.position;
                }

                holdTime = 0f;             // ���� (�� ���� ��۵ǵ���)
            }
        }
        else
        {
            holdTime = 0f;  // R ������ ���� ������ �ʱ�ȭ
        }
    }

    public void ToggleSide()
    {
        // ���� ���󰡷��� ��ǥ�� ���� ���(override�� ������ �װ�, ������ �⺻ ����)
        Transform currentTarget = overrideTargetPoint != null
            ? overrideTargetPoint
            : (playerSpriteRenderer.flipX ? followPointLeft : followPointRight);

        // �ݴ������� ��ȯ
        overrideTargetPoint = (currentTarget == followPointLeft) ? followPointRight : followPointLeft;
    }

    public void SetFollowBasePosition(Vector3 newPos)
    {
        followBasePosition = newPos;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
