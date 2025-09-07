using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private float stunTimer;
    private bool isStunned;

    // 물리 복구용 캐시
    private RigidbodyConstraints2D prevConstraints;
    private Vector2 prevVelocity;
    private float prevGravity;

    private void Awake()
    {
        // 자식 쪽에 Animator가 있으면 잡기
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // 보통 Rigidbody2D는 루트에 있으니 부모에서 탐색
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                Recover();
            }
        }
    }

    public void ApplyStun(float duration)
    {
        Debug.Log($"스턴: {name} {duration}s");

        if (isStunned) return;

        // 애니메이션 멈춤
        if (animator != null) animator.speed = 0f;

        // 물리 멈춤
        if (rb != null)
        {
            prevConstraints = rb.constraints;
            prevVelocity = rb.linearVelocity;
            prevGravity = rb.gravityScale;

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        stunTimer = duration;
        isStunned = true;
    }

    private void Recover()
    {
        // 애니메이션 복구
        if (animator != null) animator.speed = 1f;

        // 물리 복구
        if (rb != null)
        {
            rb.constraints = prevConstraints;
            rb.gravityScale = prevGravity;
            rb.linearVelocity = Vector2.zero; // 원래 속도를 되살리고 싶으면 prevVelocity 사용
        }

        isStunned = false;
    }
}
