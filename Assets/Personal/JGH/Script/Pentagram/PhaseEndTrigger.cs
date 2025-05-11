using UnityEngine;

public class PhaseEndTrigger : MonoBehaviour
{
    public Material targetMaterial;               // 감시할 머터리얼
    public string progressProperty = "_Progress"; // 감시할 속성 이름
    public float checkInterval = 0.011111f;       // 체크 주기
    public bool triggered = false;

    void Start()
    {
        // targetMaterial이 인스펙터에서 지정되지 않았으면 Renderer에서 가져옴
        if (targetMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                targetMaterial = renderer.material;
                Debug.Log("[PhaseEndTrigger] Renderer에서 머터리얼 가져옴: " + targetMaterial.name);
            }
            else
            {
                Debug.LogWarning("[PhaseEndTrigger] Renderer가 없어서 머터리얼을 가져올 수 없음.");
            }
        }
        else
        {
            Debug.Log("[PhaseEndTrigger] 인스펙터에서 설정된 머터리얼: " + targetMaterial.name);
        }

        if (targetMaterial == null)
        {
            Debug.LogError("[PhaseEndTrigger] targetMaterial이 null입니다. 감시 루틴을 시작할 수 없습니다.");
            return;
        }

        StartCoroutine(CheckProgressRoutine());
    }

    System.Collections.IEnumerator CheckProgressRoutine()
    {
        Debug.Log("[PhaseEndTrigger] CheckProgressRoutine 시작됨");

        while (!triggered)
        {
            float progress = targetMaterial.GetFloat(progressProperty);
            Debug.Log($"[PhaseEndTrigger] 현재 _Progress 값: {progress}");

            if (progress >= 0.99f) // 또는 Mathf.Approximately(progress, 1f)
            {
                Debug.Log("[PhaseEndTrigger] _Progress가 1에 도달했거나 매우 가까움. 트리거 실행!");

                triggered = true;

                if (CustomSceneManager.Instance != null)
                {
                    Debug.Log("[PhaseEndTrigger] CustomSceneManager.Instance가 유효함. Unload 실행.");
                    CustomSceneManager.Instance.UnloadAdditiveScene();
                }
                else
                {
                    Debug.LogError("[PhaseEndTrigger] CustomSceneManager.Instance가 null입니다!");
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }

        Debug.Log("[PhaseEndTrigger] CheckProgressRoutine 종료됨 (이미 트리거됨)");
    }
}
