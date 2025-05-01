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
}
