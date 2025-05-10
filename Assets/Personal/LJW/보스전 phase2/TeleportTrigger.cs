using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("텔레포트 대상 위치")]
    public Transform targetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && targetPosition != null)
        {
            collision.transform.position = targetPosition.position;
            Debug.Log($"[TeleportTrigger] 플레이어 텔레포트됨 → {targetPosition.position}");
        }
    }
}
