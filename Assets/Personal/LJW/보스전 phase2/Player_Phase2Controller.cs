using UnityEngine;

public class Player_Phase2Controller : MonoBehaviour
{
    /*[Header("�ڷ���Ʈ ����Ʈ")]
    public Transform[] teleportPoints;

    [Header("����")]
    public KeyCode teleportKey = KeyCode.T;
    public int currentTeleportIndex = 0;*/

    [Header("Phase2 ����")]
    public SwitchVision switchVision; // �ν����Ϳ� ����
    private bool isPhase2Active = false;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();  // ���� ������Ʈ�� ���� Player ��ũ��Ʈ ��������
    }

    private void Update()
    {
        /*if (!isPhase2Active) return;

        if (Input.GetKeyDown(teleportKey))
        {
            TryTeleport();
        }

        // �ε��� ���� Ű (��: 1~5�� Ű�� ����)
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentTeleportIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentTeleportIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentTeleportIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentTeleportIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentTeleportIndex = 4;*/
    }

    /*private void TryTeleport()
    {
        if (teleportPoints.Length == 0) return;

        if (currentTeleportIndex >= 0 && currentTeleportIndex < teleportPoints.Length)
        {
            transform.position = teleportPoints[currentTeleportIndex].position;
            Debug.Log($"�ڷ���Ʈ! ���� {currentTeleportIndex}");
        }
        else
        {
            Debug.LogWarning("�߸��� �ڷ���Ʈ �ε���");
        }
    }*/

    public void ActivatePhase2()
    {
        isPhase2Active = true;
        if (switchVision != null) switchVision.canSwitch = false;
        Debug.Log("[Phase2] �ڷ���Ʈ ON, Tab ���� ��ȯ OFF");
    }

    public void DeactivatePhase2()
    {
        isPhase2Active = false;
        if (switchVision != null) switchVision.canSwitch = true;
        Debug.Log("[Phase2] �ڷ���Ʈ OFF, Tab ���� ��ȯ ON");
    }
}
