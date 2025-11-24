/*using UnityEngine;

public class Phase1_Manager : MonoBehaviour
{
    public Player player;
    public Ayla ayla;
    public SwitchVision switchVision;
    public Transform aylaPuzzleStartPoint;
    public Transform playerDownCameraTarget;
    [SerializeField] private GameObject puzzleUI;

    private bool phaseStarted = false;
    private bool puzzleSolved = false;

    public int currentPuzzleIndex = 0;
    public int totalPuzzleCount = 4;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        ayla = GameObject.FindAnyObjectByType<Ayla>();

        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    public void StartPhase()
    {
        Debug.Log("[Phase1] 시작됨");

        phaseStarted = true;

        if (player != null)
            player.stateMachine.ChangeState(player.downState);

        if (ayla != null && aylaPuzzleStartPoint != null)
        {
            ayla.SetControlEnabled(false);
            ayla.isCurrentlyControlled = false;
        }

        if (switchVision != null && switchVision.CCamera != null)
            switchVision.CCamera.gameObject.SetActive(false);

        if (switchVision != null && switchVision.mainCamera != null && playerDownCameraTarget != null)
        {
            Vector3 targetPos = playerDownCameraTarget.position;
            Vector3 camPos = new Vector3(targetPos.x, targetPos.y, switchVision.mainCamera.transform.position.z);
            switchVision.mainCamera.transform.position = camPos;
        }

        if (switchVision != null && switchVision.mainCamera != null)
            switchVision.mainCamera.cullingMask = switchVision.playerViewMask;
    }

    public void SolvePuzzle()
    {
        if (puzzleSolved) return;

        puzzleSolved = true;
        currentPuzzleIndex++;

        Debug.Log($"[Phase1] 퍼즐 {currentPuzzleIndex}번 해결됨");

        if (switchVision != null && switchVision.CCamera != null)
        {
            switchVision.CCamera.gameObject.SetActive(true);
            switchVision.CCamera.Follow = player.transform;
            switchVision.CCamera.LookAt = player.transform;
            switchVision.mainCamera.cullingMask = switchVision.playerViewMask;
        }

        var aylaScript = ayla.GetComponent<Ayla>();
        var phaseController = ayla.GetComponent<AylaPhase1Controller>();
        if (phaseController != null) phaseController.enabled = false;
        if (aylaScript != null) aylaScript.enabled = true;

        ayla.SetControlEnabled(true);

        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (player != null)
            player.stateMachine.ChangeState(player.upState);
    }

    public bool IsPhaseActive => phaseStarted && !puzzleSolved;
}
*/