using UnityEngine;

public class Boss_AnimationTrigger : MonoBehaviour
{
    private Boss boss => GetComponent<Boss>();
    private void AnimationEndTrigger()
    {
        boss.AnimationTrigger();
    }
}
