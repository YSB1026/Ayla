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

    private Vector3 targetPosition;    // FollowTarget에서 계산
    private float floatOffsetY;        // FloatOffset에서 계산

    private void Update()
    {
        if (target == null) return;
        if (leftPoint == null || rightPoint == null) return;

        FollowTarget();
        FloatOffset();
        ApplyMovement();

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

}
