using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : BaseTrigger
{
    //[SerializeField] private Player player; // �÷��̾�
    private PlayableDirector director;
    private bool isTriggered = false;

    //private void OnValidate()
    //{
    //    if (player == null)
    //    {
    //        Debug.LogError($"{gameObject.name} Ÿ�Ӷ��� Ʈ���ſ� player �־��ּ���!!", this);
    //    }
    //}

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    protected override void OnPlayerEnter()
    {
        if (isTriggered) return;

        isTriggered = true;
        GameManager.Instance.SetPlayerControlEnabled(false);

        if (director != null)
        {
            director.stopped += OnTimelineStopped;
            director.Play();
        }
    }

    private void OnTimelineStopped(PlayableDirector d)
    {
        GameManager.Instance.SetPlayerControlEnabled(true);

        director.stopped -= OnTimelineStopped;

        Destroy(gameObject);
    }
}
