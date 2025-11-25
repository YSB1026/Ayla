using UnityEngine;

public enum AylaColor
{
    Red,
    Blue,
    Green
}

public class Ayla : MonoBehaviour
{
    [Header("따라갈 대상 (보통 Player)")]
    public Transform target;

    [Header("좌/우 위치 (빈 오브젝트 드래그)")]
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("둥둥 떠다니는 효과")]
    public float floatAmplitude = 0.04f;
    public float floatFrequency = 2f;

    [Header("따라가는 부드러움")]
    [Tooltip("값이 클수록 더 빠르게 따라감")]
    public float followSmooth = 10f;

    [Tooltip("true면 오른쪽 위치, false면 왼쪽 위치 사용")]
    public bool onRightSide = true;

    [Header("능력 색상")]
    public AylaColor currentColor = AylaColor.Red;

    // 에일라 전용 상태머신
    public AylaStateMachine stateMachine { get; private set; }
    public Ayla_IdleState idleState { get; private set; }
    public Ayla_RedState redState { get; private set; }
    public Ayla_BlueState blueState { get; private set; }
    public Ayla_GreenState greenState { get; private set; }

    private Vector3 targetPosition;    // FollowTarget에서 계산
    private float floatOffsetY;        // FloatOffset에서 계산

    private void Awake()
    {
        stateMachine = new AylaStateMachine();

        idleState = new Ayla_IdleState(this, stateMachine);
        redState = new Ayla_RedState(this, stateMachine);
        blueState = new Ayla_BlueState(this, stateMachine);
        greenState = new Ayla_GreenState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        if (target == null) return;
        if (leftPoint == null || rightPoint == null) return;

        FollowTarget();
        FloatOffset();
        ApplyMovement();

        // 에일라 상태 업데이트
        stateMachine.currentState.Update();
    }

    private void FollowTarget()
    {
        Transform p = onRightSide ? rightPoint : leftPoint;

        targetPosition = new Vector3(
            p.position.x,
            p.position.y,
            transform.position.z
        );
    }

    private void FloatOffset()
    {
        floatOffsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
    }

    private void ApplyMovement()
    {
        Vector3 finalPos = new Vector3(
            targetPosition.x,
            targetPosition.y + floatOffsetY,
            targetPosition.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            finalPos,
            followSmooth * Time.deltaTime
        );
    }

    public void SetSide(bool right)
    {
        onRightSide = right;
    }

    public void ToggleSide()
    {
        onRightSide = !onRightSide;
    }

    // 플레이어가 호출할 함수
    public void UseCurrentAbility()
    {
        switch (currentColor)
        {
            case AylaColor.Red:
                stateMachine.ChangeState(redState);
                break;
            case AylaColor.Blue:
                stateMachine.ChangeState(blueState);
                break;
            case AylaColor.Green:
                stateMachine.ChangeState(greenState);
                break;
        }
    }
}
