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

    private Vector3 velocity = Vector3.zero;
    private Vector3 followBasePosition;     // ���� ���� ��ġ

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer aylaSpriteRenderer;

    #region States
    public AylaStateMachine stateMachine { get; private set; }
    // public AylaIdleState idleState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        aylaSpriteRenderer = GetComponent<SpriteRenderer>();

        // ���� �ӽ� �ν��Ͻ� ����
        stateMachine = new AylaStateMachine();
        // idleState = new AylaIdleState(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();
        // stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState?.Update();

        FollowAndFloat();
    }

    private void FollowAndFloat()
    {
        // �÷��̾ ���� �ִ� ���⿡ ���� ���� ����Ʈ ����
        Transform targetPoint = playerSpriteRenderer.flipX ? followPointLeft : followPointRight;

        // ���� ��ġ���� �ε巴�� �̵�
        followBasePosition = Vector3.SmoothDamp(followBasePosition, targetPoint.position, ref velocity, followSmoothTime);

        // �սǵս� ȿ��
        Vector3 floatOffset = new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
        transform.position = followBasePosition + floatOffset;

        // ���϶� ���� ����: ���ʿ� ������ ������ ����, �����ʿ� ������ ���� ��
        aylaSpriteRenderer.flipX = (targetPoint == followPointLeft) ? false : true;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
