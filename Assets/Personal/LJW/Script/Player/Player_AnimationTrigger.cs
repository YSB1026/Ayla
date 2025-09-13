using UnityEngine;

public class Player_AnimationTrigger : MonoBehaviour
{
    private Player player => GetComponent<Player>();

    private void AnimationEndTrigger()
    {
        player.AnimationTrigger();
    }

    private void OnPlayFootstepSound()
    {
        player.PlayFootstepSound();
    }

    private void OnPlayCrawlingSound()
    {
        player.PlayCrawlingSound();
    }

    private void DeadAnimtionEndEvent()
    {
        /*GameManager.Instance.RespawnPlayer();
        Destroy(this.gameObject);*/
        player.transform.position = GameManager.Instance.currentSave.savePoint;
        player.stateMachine.ChangeState(player.inputState);
    }
}
