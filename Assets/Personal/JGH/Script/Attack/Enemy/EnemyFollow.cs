using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float normalMoveSpeed = 3f;
    public float catchUpSpeed = 15f;         // 순간 가속 속도
    public float teleportDistance = 15f;     // 이 거리 이상이면 순간 이동
    public float fastChaseDistance = 8f;     // 이 거리 이상이면 빠르게 쫓아감

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 direction = (player.position - transform.position).normalized;

        if (distance > teleportDistance)
        {
            // 순간 이동
            transform.position = player.position - direction * 2f; // 플레이어 근처로 순간 이동
        }
        else if (distance > fastChaseDistance)
        {
            // 빠르게 쫓아감
            transform.position += direction * catchUpSpeed * Time.deltaTime;
        }
        else
        {
            // 일반 추적
            transform.position += direction * normalMoveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
