using UnityEngine;

public class PlayerStayZone : MonoBehaviour
{
    public float stayThreshold = 3f; // �ӹ��� �ð� ����
    private float stayTimer = 0f;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            stayTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= stayThreshold)
            {
                Debug.Log("[StayZone] �÷��̾� �ʹ� ���� �ӹ��� �� �н� ����!");
                // Phase2_Manager.Instance.SpawnClone(transform.position);
                stayTimer = 0f; // �ߺ� ���� ����
            }
        }
    }
}
