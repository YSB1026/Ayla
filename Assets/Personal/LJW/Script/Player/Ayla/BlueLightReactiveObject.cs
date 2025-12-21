using UnityEngine;
using System.Collections;

// 이 스크립트는 적(Enemy) 프리팹에 붙입니다.
public class BlueLightReactiveObject : MonoBehaviour, ILightReactive
{
    public bool IsInLight { get; set; } = false;

    [Header("설정")]
    [SerializeField] private float slowDuration = 3.0f;
    [SerializeField] private float slowFactor = 0.3f; // 30% 속도로 감소

    private ISlowable slowableTarget; // 구체적인 클래스 대신 인터페이스 저장
    private Coroutine slowCoroutine;

    private void Awake()
    {
        // ISlowable을 상속받은 녀석을 찾아서 가져옴
        slowableTarget = GetComponent<ISlowable>();

        if (slowableTarget == null)
        {
            Debug.LogWarning($"{gameObject.name}에 ISlowable을 상속받은 이동 스크립트가 없습니다!");
        }
    }

    public void ApplyLightReaction()
    {
        // 1. 색상 확인 (Blue)
        if (GameManager.Instance.currentSave.pendantColor != ColorOption.Blue) return;

        // 2. 타겟이 있고 빛 안에 있는지 확인
        if (IsInLight && slowableTarget != null)
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SlowRoutine());
        }
    }

    private IEnumerator SlowRoutine()
    {
        // 인터페이스 함수 호출
        slowableTarget.SetSlowFactor(slowFactor);
        Debug.Log("Enemy Slowed via Interface!");

        yield return new WaitForSeconds(slowDuration);

        // 원상 복구
        slowableTarget.SetSlowFactor(1.0f);
        Debug.Log("Enemy Speed Restored.");
    }
}