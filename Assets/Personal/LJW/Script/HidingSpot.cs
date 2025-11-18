using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [Header("플레이어가 숨을 때 붙일 기준점")]
    public Transform hideAnchor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponentInParent<Player>();
        if (player == null) return;

        player.SetHidingSpotDetected(true, hideAnchor != null ? hideAnchor : transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponentInParent<Player>();
        if (player == null) return;

        player.SetHidingSpotDetected(false, null);
    }
}
