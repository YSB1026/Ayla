using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : BaseTrigger
{
    [Header("GameObject")]
    [SerializeField] private List<GameObject> gameObjectList = new();
    [Header("Game Object 활성화 여부")]
    [SerializeField] private bool isActive = false;

    private PlayableDirector director;
    private bool isTriggered = false;

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

        SetActiveGameObjects();

        Destroy(gameObject);
    }

    private void SetActiveGameObjects()
    {
        if (gameObjectList.Count == 0) return;

        foreach (var gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }
}
