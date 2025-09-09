using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("�ڷ���Ʈ ��� ��ġ")]
    public Transform targetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && targetPosition != null)
        {
            collision.transform.position = targetPosition.position;
        }
    }
}
