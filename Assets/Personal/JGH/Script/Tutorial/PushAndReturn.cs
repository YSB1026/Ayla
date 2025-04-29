using UnityEngine;
using System.Collections;

public class PushAndReturn2D : MonoBehaviour
{
    public float pushDistance = 0.5f; // 앞으로 이동 거리 (X 방향 기준)
    public float duration = 0.3f;     // 천천히 이동하는 시간
    public float repeatDelay = 1.0f;  // 반복 주기 (초)

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(RepeatBackAndReturn());
    }

    IEnumerator RepeatBackAndReturn()
    {
        while (true)
        {
            yield return MoveBackAndReturn();
            yield return new WaitForSeconds(repeatDelay); // 다음 반복까지 대기
        }
    }

    IEnumerator MoveBackAndReturn()
    {
        Vector3 targetPosition = originalPosition + Vector3.right * pushDistance;
        float elapsed = 0f;

        // 천천히 뒤로 이동
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 즉시 복귀
        transform.position = originalPosition;
    }
}
