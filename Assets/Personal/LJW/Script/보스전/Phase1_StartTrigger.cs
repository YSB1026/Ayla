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
            Debug.Log("Phase1 Ʈ���� ������ -> Phase1 ����");

            var manager = FindAnyObjectByType<Phase1_Manager>();
            var ayla = FindAnyObjectByType<Ayla>();

            manager?.StartPhase();

            // switchVision Ȱ��ȭ + Phase1 ���� ǥ��
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
