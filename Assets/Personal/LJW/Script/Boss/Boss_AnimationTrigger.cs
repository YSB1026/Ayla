using UnityEngine;

public class Boss_AnimationTrigger : MonoBehaviour
{
    private Boss boss => GetComponentInParent<Boss>();
    public void AnimationEndTrigger()
    {
        boss.AnimationTrigger();
    }

    private void HitTrigger()
    {
        boss.HitTrigger();
    }
}
