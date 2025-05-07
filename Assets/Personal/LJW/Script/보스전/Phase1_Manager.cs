using UnityEngine;

public class Phase1_Manager : MonoBehaviour
{
    public Transform ceiling;
    public float descendSpeed = 0.5f;

    private bool phaseStarted = false;
    private bool puzzleSolved = false;
    private bool phaseStopped = false;

    private Player player;
    private Ayla ayla;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        ayla = GameObject.FindAnyObjectByType<Ayla>();
    }

    void Update()
    {
        if (!phaseStarted || phaseStopped || puzzleSolved)
            return;

        if (ceiling != null)
        {
            ceiling.position += Vector3.down * descendSpeed * Time.deltaTime;
        }
        else
        {
            Debug.LogError("ceiling ���� �� ��!");
        }
    }

    public void StartPhase()
    {
        Debug.Log("Phase1 ����!");
        phaseStarted = true;

        if (player != null)
            player.SetControlEnabled(false); // �÷��̾� ���� ����

        if (ayla != null)
            ayla.SetControlEnabled(true);    // ���϶� ���� ���
    }

    public void SolvePuzzle()
    {
        Debug.Log("���� �ذ��!");
        puzzleSolved = true;

        if (player != null)
            player.SetControlEnabled(true); // �÷��̾� �ٽ� ���� ����

        if (ayla != null)
            ayla.SetControlEnabled(false);  // ���϶� ���󰡱� ��ȯ

        // �ʿ��ϴٸ� ���� Phase�� ��ȯ�ϰų� �� ���� ȣ��
    }
    public void StopCeiling()
    {
        phaseStopped = true;
        Debug.Log("õ�� ����");
    }
}
