using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;       // 플레이어 위치
    public float moveSpeed = 3f;   // 이동 속도

    void Update()
    {
        if (player == null) return;

        // 방향 벡터 계산
        Vector3 direction = (player.position - transform.position).normalized;

        // 이동
        transform.position += direction * moveSpeed * Time.deltaTime;
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
