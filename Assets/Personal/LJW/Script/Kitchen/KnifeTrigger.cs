using UnityEngine;

public class KnifeTrigger : MonoBehaviour
{
    [SerializeField] private KnifeController knife; // ����߸� Į

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            knife.Drop();                    // Į ����߸���
            Destroy(gameObject);            // Ʈ���Ŵ� 1ȸ���̸� �ı�
        }
    }
}
