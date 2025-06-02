using UnityEngine;

public class KnifeTrigger : MonoBehaviour
{
    [SerializeField] private KnifeController[] knives; // ����߸� Į

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")/*collision.GetComponent<Player>() != null*/)
        {
            foreach (var knife in knives)
            {
                knife.Drop();                    // Į ����߸���
                Destroy(gameObject);            // Ʈ���Ŵ� 1ȸ���̸� �ı�
            }
        }
    }
}
