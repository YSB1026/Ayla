using UnityEngine;

public class Phase1_StartTrigger : MonoBehaviour
{
    private bool triggered = false;

    public SwitchVision switchVision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Phase1 트리거 감지됨 -> Phase1 돌입");

            var manager = FindAnyObjectByType<Phase1_Manager>();
            var ayla = FindAnyObjectByType<Ayla>();

            manager?.StartPhase();

            // switchVision 활성화 + Phase1 진입 표시
            if (switchVision != null)
            {
                switchVision.isPhase1 = true;

                // Ayla 시점으로 전환
                switchVision.CCamera.Follow = ayla.transform;
                switchVision.CCamera.LookAt = ayla.transform;

                // 퍼즐 보이게 레이어 마스크 직접 설정
                switchVision.mainCamera.cullingMask = switchVision.aylaPhase1ViewMask;
            }
            triggered = true;
        }
    }
}
