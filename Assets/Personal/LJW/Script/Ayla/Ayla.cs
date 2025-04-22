using System;
using UnityEngine;

public class Ayla : Entity
{
    [Header("위치 정보")]
    public Transform player;          // 플레이어
    public Transform followPointLeft;       // 플레이어 왼쪽
    public Transform followPointRight;      // 플레이어 오른쪽

    [Header("이동 정보")]
    public float followSmoothTime = 0.3f;   // 따라가는 부드러움 정도
    public float floatSpeed = 2f;           // 위아래 떠다니는 속도
    public float floatAmplitude = 0.15f;    // 떠다니는 높이

    private Vector3 velocity = Vector3.zero;
    private Vector3 followBasePosition;     // 따라갈 기준 위치

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

        // 상태 머신 인스턴스 생성
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
        // 플레이어가 보고 있는 방향에 따라 따라갈 포인트 결정
        Transform targetPoint = playerSpriteRenderer.flipX ? followPointLeft : followPointRight;

        // 기준 위치까지 부드럽게 이동
        followBasePosition = Vector3.SmoothDamp(followBasePosition, targetPoint.position, ref velocity, followSmoothTime);

        // 둥실둥실 효과
        Vector3 floatOffset = new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
        transform.position = followBasePosition + floatOffset;

        // 아일라 방향 반전: 왼쪽에 있으면 오른쪽 보고, 오른쪽에 있으면 왼쪽 봄
        aylaSpriteRenderer.flipX = (targetPoint == followPointLeft) ? false : true;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
