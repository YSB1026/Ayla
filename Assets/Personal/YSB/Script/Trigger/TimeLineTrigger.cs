using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : BaseTrigger
{
    [SerializeField] private Player player; // 플레이어
    private PlayableDirector director;
    private bool isTriggered = false;

    private void OnValidate()
    {
        if (player == null)
        {
            Debug.LogError($"{gameObject.name} 타임라인 트리거에 player 넣어주세요!!", this);
        }
    }

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    protected override void OnPlayerEnter()
    {
        if (isTriggered) return;

        isTriggered = true;
        player.SetControlEnabled(false);

        if (director != null)
        {
            director.stopped += OnTimelineStopped;
            director.Play();
        }
    }

    private void OnTimelineStopped(PlayableDirector d)
    {
        player.SetControlEnabled(true);

        director.stopped -= OnTimelineStopped;

        Destroy(gameObject);
    }
}
