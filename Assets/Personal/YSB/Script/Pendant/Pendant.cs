using UnityEngine;
using static LightColorController;

public class Pendant : MonoBehaviour
{
    [Header("ȹ���� ��")]
    [SerializeField] private ColorOption pendantColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PendantEventManager.TriggerPendantCollected(pendantColor);
            Destroy(gameObject);
        }
    }
}
