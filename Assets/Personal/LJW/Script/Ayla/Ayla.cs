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
    public float moveSpeed = 3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 followBasePosition;     // 따라갈 기준 위치

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer aylaSpriteRenderer;

    private bool controlEnabled = false;  // 키 조작 여부
    private bool isFixed = false;         // R 키로 고정 여부
    public bool isCurrentlyControlled;  // 현재 조작되고 있는지 감지

    [SerializeField] private float holdTime = 0f;
    [SerializeField] private float holdDuration = 2f;

    [Header("에일란 게이지")]
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

        // 상태 머신 인스턴스 생성
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
            // WASD 조작
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

    // 플레이어 따라다니는 로직
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

    // 둥둥 뜨는 효과
    private void Float()
    {
        // 둥실둥실 효과
        Vector3 floatOffset = new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
        transform.position = followBasePosition + floatOffset;
    }

    // R 키로 고정 시키기
    private void FixedKey()
    {
        if (!isCurrentlyControlled)
            return;

        // R 키로 고정
        if (Input.GetKey(KeyCode.R))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdDuration)
            {
                isFixed = !isFixed;         // 고정 상태 토글
                controlEnabled = !isFixed;  // 조작 비활성화

                if (isFixed)    // 고정
                {
                    followBasePosition = transform.position;
                }

                holdTime = 0f;             // 리셋 (한 번만 토글되도록)
            }
        }
        else
        {
            holdTime = 0f;  // R 누르고 있지 않으면 초기화
        }
    }

    public void ToggleSide()
    {
        // 지금 따라가려는 목표가 뭔지 계산(override가 있으면 그걸, 없으면 기본 로직)
        Transform currentTarget = overrideTargetPoint != null
            ? overrideTargetPoint
            : (playerSpriteRenderer.flipX ? followPointLeft : followPointRight);

        // 반대편으로 전환
        overrideTargetPoint = (currentTarget == followPointLeft) ? followPointRight : followPointLeft;
    }

    public void SetFollowBasePosition(Vector3 newPos)
    {
        followBasePosition = newPos;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
