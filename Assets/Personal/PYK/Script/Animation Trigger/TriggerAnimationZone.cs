using UnityEngine;

public class TriggerAnimationZone : MonoBehaviour
{
    public Animator targetAnimator;
    public string triggerName = "ActivateTrigger";
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ ObjectTrigger ���̾ ���� ���� �����ϰ�, �ٴ��� �����ϵ��� ����
        if (!hasTriggered && other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("ObjectTrigger"))
        {
            targetAnimator.SetTrigger(triggerName);
            hasTriggered = true;  // �ѹ��� ����ǵ��� ����
        }
    }
}
