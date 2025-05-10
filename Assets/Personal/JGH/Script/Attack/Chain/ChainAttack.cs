using UnityEngine;
using System.Collections;

public class ChainAttack : MonoBehaviour
{
    public float fallDuration = 0.3f;
    public float fadeDuration = 1.2f;
    private SpriteRenderer sr;
    private Collider2D col;

    private bool hasHitPlayer = false;

    public void StartAttack(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;
        StartCoroutine(AttackRoutine(targetPos));
    }

    IEnumerator AttackRoutine(Vector3 targetPos)
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        float t = 0;
        Vector3 start = transform.position;

        // 내려오는 애니메이션
        while (t < 1f)
        {
            t += Time.deltaTime / fallDuration;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        // 도착 후 → 더 이상 충돌 판정 X
        if (col != null)
            col.enabled = false;

        // 서서히 사라지기
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, timer / fadeDuration);
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitPlayer) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
                hasHitPlayer = true;
            }
        }
    }
}
