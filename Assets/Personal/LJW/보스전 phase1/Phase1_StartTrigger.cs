/*using UnityEngine;

public class Phase1_StartTrigger : MonoBehaviour
{
    private bool triggered = false;

    public SwitchVision switchVision;
    public GameObject nextTrigger; // ✅ 다음 트리거를 연결 (Hierarchy에서 할당)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Phase1 첫 번째 트리거 작동 -> Phase1 시작");

            // Ayla 관련 스크립트 전환
            Ayla aylaScript = switchVision.ayla.GetComponent<Ayla>();
            AylaPhase1Controller phaseController = switchVision.ayla.GetComponent<AylaPhase1Controller>();

            if (aylaScript != null) aylaScript.enabled = false;
            if (phaseController != null) phaseController.enabled = true;

            // Phase1 매니저 호출
            var manager = FindAnyObjectByType<Phase1_Manager>();
            manager?.StartPhase();

            // SwitchVision 상태 설정
            if (switchVision != null)
            {
                switchVision.isPhase1 = true;
            }

            // ✅ 다음 트리거 활성화
            if (nextTrigger != null)
                nextTrigger.SetActive(true);

            // ✅ 자신 비활성화
            gameObject.SetActive(false);

            triggered = true;
        }
    }
}
*/