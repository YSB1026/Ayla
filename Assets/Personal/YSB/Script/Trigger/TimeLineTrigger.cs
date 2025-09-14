using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : BaseTrigger
{
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
            GameManager.Instance.SetPlayerControl(false);
            director.Play();
            director.stopped += (PlayableDirector obj) => OnTimelineFinished();
        }
    }

    private void OnTimelineFinished()
    {
        GameManager.Instance.SetPlayerControl(true);

        SyncPlayerFacing();
        ApplyActions();
        Destroy(gameObject);
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


