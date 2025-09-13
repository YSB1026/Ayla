using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum ObjectActionType
{
    None,
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
    [Header("타임라인 이후 오브젝트 액션")]
    [SerializeField] private List<ObjectAction> actions = new();
    private Player player;
    private bool isPlayerFacingRight;
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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isPlayerFacingRight = player.facingRight;

        Debug.Log($"Player Facing Right: {isPlayerFacingRight}");

        if (director != null)
        {
            GameManager.Instance.SetPlayerControlEnabled(false);
            director.Play();
            director.stopped += (PlayableDirector obj) => OnTimelineFinished();
        }
    }

    private void OnTimelineFinished()
    {
        GameManager.Instance.SetPlayerControlEnabled(true);
        SyncPlayerFacing();
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

    private void SyncPlayerFacing()
    {
        var sr = player.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && sr.flipX)
            sr.flipX = false;

        if (isPlayerFacingRight)
        {
            player.facingDir = 1;
            player.facingRight = true;
        }
        else
        {
            player.Flip();
            player.facingDir = -1;
            player.facingRight = false;
        }
    }
}


