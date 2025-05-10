using UnityEngine;

public class Player_Phase2Controller : MonoBehaviour
{
    /*[Header("텔레포트 포인트")]
    public Transform[] teleportPoints;

    [Header("설정")]
    public KeyCode teleportKey = KeyCode.T;
    public int currentTeleportIndex = 0;*/

    [Header("Phase2 전용")]
    public SwitchVision switchVision; // 인스펙터에 연결
    private bool isPhase2Active = false;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();  // 같은 오브젝트에 붙은 Player 스크립트 가져오기
    }

    private void Update()
    {
        /*if (!isPhase2Active) return;

        if (Input.GetKeyDown(teleportKey))
        {
            TryTeleport();
        }

        // 인덱스 변경 키 (예: 1~5번 키로 선택)
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
            Debug.Log($"텔레포트! 지점 {currentTeleportIndex}");
        }
        else
        {
            Debug.LogWarning("잘못된 텔레포트 인덱스");
        }
    }*/

    public void ActivatePhase2()
    {
        isPhase2Active = true;
        if (switchVision != null) switchVision.canSwitch = false;
        Debug.Log("[Phase2] 텔레포트 ON, Tab 시점 전환 OFF");
    }

    public void DeactivatePhase2()
    {
        isPhase2Active = false;
        if (switchVision != null) switchVision.canSwitch = true;
        Debug.Log("[Phase2] 텔레포트 OFF, Tab 시점 전환 ON");
    }
}
