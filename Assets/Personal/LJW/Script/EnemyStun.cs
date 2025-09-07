using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private float stunTimer;
    private bool isStunned;

    // ���� ������ ĳ��
    private RigidbodyConstraints2D prevConstraints;
    private Vector2 prevVelocity;
    private float prevGravity;

    private void Awake()
    {
        // �ڽ� �ʿ� Animator�� ������ ���
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // ���� Rigidbody2D�� ��Ʈ�� ������ �θ𿡼� Ž��
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
        Debug.Log($"����: {name} {duration}s");

        if (isStunned) return;

        // �ִϸ��̼� ����
        if (animator != null) animator.speed = 0f;

        // ���� ����
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
        // �ִϸ��̼� ����
        if (animator != null) animator.speed = 1f;

        // ���� ����
        if (rb != null)
        {
            rb.constraints = prevConstraints;
            rb.gravityScale = prevGravity;
            rb.linearVelocity = Vector2.zero; // ���� �ӵ��� �ǻ츮�� ������ prevVelocity ���
        }

        isStunned = false;
    }
}
