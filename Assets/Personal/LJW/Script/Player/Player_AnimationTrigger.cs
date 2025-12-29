using UnityEngine;

public class Player_AnimationTrigger : MonoBehaviour
{
    private Player player => GetComponent<Player>();

    private void AnimationEndTrigger()
    {
        if(player == null) return;
        player.AnimationTrigger();
    }

    private void OnPlayFootstepSound()
    {
        if(player == null) return;

        player.PlayFootstepSound();
    }

    private void OnPlayCrawlingSound()
    {
        if(player == null) return;

        player.PlayCrawlingSound();
    }

    private void DeadAnimtionEndEvent()
    {
        if(player == null) return;

        /*GameManager.Instance.RespawnPlayer();
        Destroy(this.gameObject);*/
        //player.transform.position = GameManager.Instance.currentSave.savePoint;
        player.stateMachine.ChangeState(player.inputState);
    }
}
