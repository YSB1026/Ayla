using UnityEngine;

public class Boss_AnimationTrigger : MonoBehaviour
{
    private Boss boss => GetComponentInParent<Boss>();
    private void AnimationEndTrigger()
    {
        boss.AnimationTrigger();
    }

    private void HitTrigger()
    {
        boss.HitTrigger();
    }
}
