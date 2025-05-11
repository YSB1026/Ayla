using UnityEngine;

public class PlayerStayZone : MonoBehaviour
{
    public float stayThreshold = 3f; // 머무를 시간 기준
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
                Debug.Log("[StayZone] 플레이어 너무 오래 머무름 → 분신 생성!");
                //Phase2_Manager.Instance.SpawnClone(transform.position);
                stayTimer = 0f; // 중복 생성 방지
            }
        }
    }
}
