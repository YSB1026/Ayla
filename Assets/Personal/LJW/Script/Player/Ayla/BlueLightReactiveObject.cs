using UnityEngine;
using System.Collections;

public class BlueLightReactiveObject : MonoBehaviour, ILightReactive
{
    public bool IsInLight { get; set; } = false;

    [Header("설정")]
    [SerializeField] private float slowDuration = 3.0f;
    [SerializeField] private float slowFactor = 0.3f; // 30% 속도

    private ISlowable slowableTarget;
    private Coroutine slowCoroutine;

    private void Awake()
    {
        slowableTarget = GetComponent<ISlowable>();
    }

    public void ApplyLightReaction()
    {
        // [디버그용 로그] 현재 상태가 어떤지 출력
        Debug.Log($"[BlueCheck] Color: {GameManager.Instance.currentSave.pendantColor}, IsInLight: {IsInLight}");

        // 1. 색상 확인 (현재 펜던트가 Blue가 아니면 무시)
        if (GameManager.Instance.currentSave.pendantColor != ColorOption.Blue)
        {
            Debug.Log("실패: 현재 펜던트 색상이 Blue가 아닙니다.");
            return;
        }

        // 2. 빛 범위 안에 없으면 무시
        if (!IsInLight)
        {
            Debug.Log("실패: 적이 빛 범위(Collider) 안에 중심점이 들어오지 않았습니다.");
            return;
        }

        // 3. 타겟이 있으면 슬로우 로직 실행
        if (slowableTarget != null)
        {
            // 이미 느려진 상태라면 코루틴을 멈추고 시간을 리셋(갱신)함
            if (slowCoroutine != null)
            {
                StopCoroutine(slowCoroutine);
            }
            slowCoroutine = StartCoroutine(SlowRoutine());
        }
        else
        {
            Debug.Log("실패: ISlowable 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private IEnumerator SlowRoutine()
    {
        // 속도 감소 적용
        slowableTarget.SetSlowFactor(slowFactor);
        Debug.Log("Blue Light Hit: Speed Slowed!");

        // 3초 대기
        yield return new WaitForSeconds(slowDuration);

        // 속도 원상 복구
        slowableTarget.SetSlowFactor(1.0f);
        slowCoroutine = null; // 코루틴 종료 표시
        Debug.Log("Blue Light Effect Ended: Speed Restored.");
    }
}