using UnityEngine;

public class TriggerAnimationZone : MonoBehaviour
{
    public Animator targetAnimator;
    public string triggerName = "ActivateTrigger";
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 ObjectTrigger 레이어에 있을 때만 실행하고, 바닥은 무시하도록 설정
        if (!hasTriggered && other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("ObjectTrigger"))
        {
            targetAnimator.SetTrigger(triggerName);
            hasTriggered = true;  // 한번만 실행되도록 설정
        }
    }
}
