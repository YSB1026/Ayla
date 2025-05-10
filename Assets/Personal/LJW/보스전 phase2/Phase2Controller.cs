using UnityEngine;

public class Phase2Controller : MonoBehaviour
{
    public GameObject teleportScriptObj;
    public Phase2_Manager phase2Manager;

    private Player_Phase2Controller teleportScript;

    private void Awake()
    {
        teleportScript = teleportScriptObj.GetComponent<Player_Phase2Controller>();
    }

    public void StartPhase2()
    {
        phase2Manager.StartPhase();
        teleportScriptObj.SetActive(true);  // �ڷ���Ʈ ��� ON
        teleportScript.ActivatePhase2();
    }

    public void EndPhase2()
    {
        teleportScript.DeactivatePhase2();
        teleportScriptObj.SetActive(false);  // �ڷ���Ʈ ��� OFF
        Debug.Log("Phase2Controller: Phase2 �����.");
    }
}
