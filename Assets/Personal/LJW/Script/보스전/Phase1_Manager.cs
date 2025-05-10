using UnityEngine;

public class Phase1_Manager : MonoBehaviour
{
    [Header("Phase1 ����")]
    public Player player;
    public Ayla ayla;
    public SwitchVision switchVision;
    public Transform aylaPuzzleStartPoint;
    public Transform playerDownCameraTarget;

    [Header("õ�� ����")]
    public Transform ceiling;
    public float descendSpeed = 1f;

    private bool phaseStarted = false;
    private bool puzzleSolved = false;
    private bool phaseStopped = false;

    // ���� UI GameObject (LockPattern�� ������ ������Ʈ)
    [SerializeField] private GameObject puzzleUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        ayla = GameObject.FindAnyObjectByType<Ayla>();

        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    void Update()
    {
        if (!phaseStarted || phaseStopped || puzzleSolved)
            return;

        ceiling.position += Vector3.down * descendSpeed * Time.deltaTime;
    }

    public void StartPhase()
    {
        Debug.Log("[Phase1] ���۵�");

        phaseStarted = true;

        // 1. �÷��̾� ���� ���� �����߸���
        if (player != null)
        {
            player.stateMachine.ChangeState(player.downState);
            player.SetControlEnabled(false);
        }

        // 2. Ayla ����
        if (ayla != null && aylaPuzzleStartPoint != null)
        {
            ayla.SetControlEnabled(false);
            ayla.isCurrentlyControlled = false;
        }

        // �ó׸ӽ� ����
        if (switchVision != null && switchVision.CCamera != null)
        {
            switchVision.CCamera.gameObject.SetActive(false);
        }

        // 4. ���� ī�޶� ��ġ�� �ٿ�� �÷��̾� ��������� �̵�
        if (switchVision != null && switchVision.mainCamera != null && playerDownCameraTarget != null)
        {
            Vector3 targetPos = playerDownCameraTarget.position;
            Vector3 camPos = new Vector3(targetPos.x, targetPos.y, switchVision.mainCamera.transform.position.z);
            switchVision.mainCamera.transform.position = camPos;
        }

        // 5. �÷��̾� �þ� ����
        if (switchVision != null && switchVision.mainCamera != null)
        {
            switchVision.mainCamera.cullingMask = switchVision.playerViewMask;
        }
    }

    public void SolvePuzzle()
    {
        puzzleSolved = true;
        Debug.Log("[Phase1] ���� �ذ� �Ϸ�");

        // �ó׸ӽ� ī�޶� �ٽ� �ѱ�
        if (switchVision != null && switchVision.CCamera != null)
        {
            switchVision.CCamera.gameObject.SetActive(true);
            switchVision.CCamera.Follow = player.transform;
            switchVision.CCamera.LookAt = player.transform;
            switchVision.mainCamera.cullingMask = switchVision.playerViewMask;
        }

        Ayla aylaScript = ayla.GetComponent<Ayla>();
        AylaPhase1Controller phaseController = ayla.GetComponent<AylaPhase1Controller>();

        if (phaseController != null) phaseController.enabled = false;
        if (aylaScript != null) aylaScript.enabled = true;

        ayla.SetControlEnabled(true); // �ٽ� ���󰡱� ���
    }

    public void StopCeiling()
    {
        phaseStopped = true;
        Debug.Log("[Phase1] õ�� ����");
    }

    public bool IsPhaseActive => phaseStarted && !phaseStopped && !puzzleSolved;
}
