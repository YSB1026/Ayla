using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum ObjectActionType
{
    None,           //None 필요없긴하네요.. 근데 없애면 난리나서 놔둘게요 ;(
    SetActiveTrue,
    SetActiveFalse,
    Destroy
}

[System.Serializable]
public class ObjectAction
{
    public GameObject target;
    public ObjectActionType actionType;
}

public class TimeLineTrigger : BaseTrigger
{
    [Header("오브젝트 행동 설정")]
    [SerializeField] private List<ObjectAction> actions = new();

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
            StartCoroutine(PlayTimelineDelayed());
        }
    }

    private IEnumerator PlayTimelineDelayed()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        director.Play();
    }

    private void OnTimelineStopped(PlayableDirector d)
    {
        GameManager.Instance.SetPlayerControlEnabled(true);
        director.stopped -= OnTimelineStopped;

        ApplyActions();

        Destroy(gameObject);
    }

    private void ApplyActions()
    {
        if (actions.Count == 0) return;

        foreach (var action in actions)
        {
            if (action.target == null) continue;

            switch (action.actionType)
            {
                case ObjectActionType.SetActiveTrue:
                    action.target.SetActive(true);
                    break;
                case ObjectActionType.SetActiveFalse:
                    action.target.SetActive(false);
                    break;
                case ObjectActionType.Destroy:
                    Destroy(action.target);
                    break;
                default:
                    break;
            }
        }
    }
}

