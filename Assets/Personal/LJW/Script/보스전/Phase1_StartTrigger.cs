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
                switchVision.CCamera.Follow = ayla.transform;
                switchVision.CCamera.LookAt = ayla.transform;
                switchVision.CCamera.cullingMask = switchVision.aylaViewMask;
            }
            triggered = true;
        }
    }
}
