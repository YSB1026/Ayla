using UnityEngine;

public class CloneAnimationTrigger : MonoBehaviour
{
    private Clone_Skill_Controller clone => GetComponentInParent<Clone_Skill_Controller>();
    private void AnimationEndTrigger()
    {
        clone.AnimationTrigger();
    }

    private void HitTrigger()
    {
        clone.HitTrigger();
    }
}
