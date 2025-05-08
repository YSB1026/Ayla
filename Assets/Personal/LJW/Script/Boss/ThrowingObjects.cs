using UnityEngine;

public class ThrowingObjects : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    private bool hasStuck = false;
    private bool isFlying = true;

    private Transform target;
    private Vector2 launchDirection;

    [Header("투척 정보")]
    public float launchSpeed = 15f;
    public float gravityDelay = 1f;
    private float gravityTimer;

    [Header("감지")]
    public LayerMask whatIsGround;
    private GameObject player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void SetPlayer(GameObject _player)
    {
        player = _player;
        if (player == null) return;

        Vector2 targetPos = (Vector2)player.transform.position + Vector2.up * 0.5f;
        launchDirection = (targetPos - (Vector2)transform.position).normalized;

        rb.linearVelocity = launchDirection * launchSpeed;
        rb.gravityScale = 0f;
        gravityTimer = gravityDelay;
    }

    void Update()
    {
        if (!isFlying || hasStuck) return;

        gravityTimer -= Time.deltaTime;
        if (gravityTimer <= 0f)
        {
            rb.gravityScale = 1f;
        }

        // 회전 방향 정렬 (optional)
        transform.right = rb.linearVelocity.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasStuck) return;

        if (player != null && collision.gameObject == player)
        {
            StickInto(collision.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasStuck) return;

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            StickInto(null, false);
        }
    }

    private void StickInto(Transform parentTarget, bool isPlayer)
    {
        hasStuck = true;
        isFlying = false;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        col.enabled = false;

        transform.rotation = Quaternion.identity;

        if (isPlayer && parentTarget != null)
        {
            transform.parent = parentTarget;
            transform.localPosition = Vector3.up * 0.5f;
        }
        else
        {
            transform.parent = null;
        }
    }
}
