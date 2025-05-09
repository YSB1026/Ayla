using UnityEngine;

public class Phase1_Manager : MonoBehaviour
{
    [Header("Phase1 설정")]
    public Player player;
    public Ayla ayla;
    public SwitchVision switchVision;
    public Transform aylaPuzzleStartPoint;
    public Transform playerDownCameraTarget;

    [Header("천장 관련")]
    public Transform ceiling;
    public float descendSpeed = 1f;

    private bool phaseStarted = false;
    private bool puzzleSolved = false;
    private bool phaseStopped = false;

    // 퍼즐 UI GameObject (LockPattern을 포함한 오브젝트)
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
        Debug.Log("[Phase1] 시작됨");

        phaseStarted = true;

        // 1. 플레이어 조작 막고 쓰러뜨리기
        if (player != null)
        {
            player.stateMachine.ChangeState(player.downState);
            player.SetControlEnabled(false);
        }

        // 2. Ayla 조작
        if (ayla != null && aylaPuzzleStartPoint != null)
        {
            ayla.SetControlEnabled(false);
            ayla.isCurrentlyControlled = false;
        }

        // 시네머신 끄기
        if (switchVision != null && switchVision.CCamera != null)
        {
            switchVision.CCamera.gameObject.SetActive(false);
        }

        // 4. 메인 카메라 위치를 다운된 플레이어 연출용으로 이동
        if (switchVision != null && switchVision.mainCamera != null && playerDownCameraTarget != null)
        {
            Vector3 targetPos = playerDownCameraTarget.position;
            Vector3 camPos = new Vector3(targetPos.x, targetPos.y, switchVision.mainCamera.transform.position.z);
            switchVision.mainCamera.transform.position = camPos;
        }

        // 5. 플레이어 시야 유지
        if (switchVision != null && switchVision.mainCamera != null)
        {
            switchVision.mainCamera.cullingMask = switchVision.playerViewMask;
        }
    }

    public void SolvePuzzle()
    {
        puzzleSolved = true;
        Debug.Log("[Phase1] 퍼즐 해결 완료");

        // 시네머신 카메라 다시 켜기
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

        ayla.SetControlEnabled(true); // 다시 따라가기 허용
    }

    public void StopCeiling()
    {
        phaseStopped = true;
        Debug.Log("[Phase1] 천장 멈춤");
    }

    public bool IsPhaseActive => phaseStarted && !phaseStopped && !puzzleSolved;
}
