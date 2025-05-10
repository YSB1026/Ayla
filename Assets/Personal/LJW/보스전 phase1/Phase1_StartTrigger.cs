using UnityEngine;

public class Phase1_StartTrigger : MonoBehaviour
{
    private bool triggered = false;

    public SwitchVision switchVision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        Ayla aylaScript = switchVision.ayla.GetComponent<Ayla>();
        AylaPhase1Controller phaseController = switchVision.ayla.GetComponent<AylaPhase1Controller>();

        if (aylaScript != null) aylaScript.enabled = false;
        if (phaseController != null) phaseController.enabled = true;

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
            }
            triggered = true;
        }
    }
}
