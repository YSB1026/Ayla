using UnityEngine.Playables;
using UnityEngine;

public class DestroyAfterTimeline : MonoBehaviour
{
    public PlayableDirector director;

    private void Start()
    {
        director.stopped += OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector d)
    {
        Destroy(gameObject);
    }
}
