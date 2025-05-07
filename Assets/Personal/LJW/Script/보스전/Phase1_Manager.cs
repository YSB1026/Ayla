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
            Debug.LogError("ceiling 연결 안 됨!");
        }
    }

    public void StartPhase()
    {
        Debug.Log("Phase1 시작!");
        phaseStarted = true;

        if (player != null)
            player.SetControlEnabled(false); // 플레이어 조작 차단

        if (ayla != null)
            ayla.SetControlEnabled(true);    // 에일라 조작 허용
    }

    public void SolvePuzzle()
    {
        Debug.Log("퍼즐 해결됨!");
        puzzleSolved = true;

        if (player != null)
            player.SetControlEnabled(true); // 플레이어 다시 조작 가능

        if (ayla != null)
            ayla.SetControlEnabled(false);  // 에일라 따라가기 전환

        // 필요하다면 다음 Phase로 전환하거나 씬 연출 호출
    }
    public void StopCeiling()
    {
        phaseStopped = true;
        Debug.Log("천장 멈춤");
    }
}
