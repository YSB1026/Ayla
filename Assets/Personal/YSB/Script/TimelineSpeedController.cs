using UnityEngine;
using UnityEngine.Playables;

public class TimelineSpeedController : MonoBehaviour
{
    [Header("���� ���� Ű ����")]
    [SerializeField] KeyCode keyCode = KeyCode.Space;

    [Header("��� ����")]
    [Range(0.1f, 8f)]
    public float fastForwardSpeed = 8f;

    private double originalSpeed = 1.0;
    private bool isFastForwarding = false;
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("PlayableDirector�� �� GameObject�� �����ϴ�.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            ToggleFastForward();
        }
    }

    public void ToggleFastForward()
    {
        if (director == null || !director.playableGraph.IsValid())
        {
            return;
        }

        var root = director.playableGraph.GetRootPlayable(0);

        if (!isFastForwarding)
        {
            originalSpeed = root.GetSpeed();
            root.SetSpeed(fastForwardSpeed);
            isFastForwarding = true;
        }
        else
        {
            root.SetSpeed(originalSpeed);
            isFastForwarding = false;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        if (director == null || !director.playableGraph.IsValid()) return;
        director.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
    }

    public void ResetSpeed()
    {
        if (director == null || !director.playableGraph.IsValid()) return;
        director.playableGraph.GetRootPlayable(0).SetSpeed(originalSpeed);
        isFastForwarding = false;
    }
}
