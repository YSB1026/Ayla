using UnityEngine;

public class Player_AnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationEndTrigger()
    {
        player.AnimationTrigger();
    }
}
