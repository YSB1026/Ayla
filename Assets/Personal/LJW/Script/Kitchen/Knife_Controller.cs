using UnityEngine;
using System.Collections;

public class KnifeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    [SerializeField] private Transform gravityPivot; // Į �� ��ġ
    [SerializeField] private float rotateDuration = 1f; // ȸ�� �Ϸ���� �ɸ� �ð�
    [SerializeField] private float moveSpeed = 2f;         // �÷��̾�� �ٰ����� �ӵ�

    [SerializeField] private Player player;

    private bool canRotate = true;
    private bool isMovingToPlayer = false;   // Į�� �÷��̾� ������ �������� ������

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private bool readyToJump = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

        if (player == null)
        {
            if (player != null)
                Debug.Log($"Player �ڵ� �Ҵ� ����: {player.name}");
            else
                Debug.LogWarning("Player �ڵ� �Ҵ� ����! ���� Player�� ����");
        }
    }

    private void Update()
    {
        if (canRotate && gravityPivot != null)
        {
            Vector2 fallDir = rb.linearVelocity.normalized;

            if (fallDir.sqrMagnitude > 0.01f)
            {
                // ȸ��: Į ���� �߷� ������ ���ϵ���
                Quaternion targetRot = Quaternion.FromToRotation(gravityPivot.up, fallDir) * transform.rotation;

                // �� �����Ӵ� ȸ�� �ӵ� = ��ü ȸ�� �� / �ɸ� �ð�
                float maxDegreesPerFrame = 360f / rotateDuration;

                // ���� ȸ������ ��ǥ ȸ������ �ε巴�� ȸ��
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    maxDegreesPerFrame * Time.deltaTime
                );
            }
        }

        // �÷��̾ ���� Į�� �������� ó��
        if (isMovingToPlayer)
        {
            if (player == null)
            {
                Debug.LogWarning("isMovingToPlayer�� true�ε�, player�� null�Դϴ�!");
                return;
            }

            Vector2 target = player.transform.position;
            Vector2 direction = (target - (Vector2)transform.position).normalized;

            // ���� ��� ���� ���� ���� ����
            isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

            if (isGrounded && readyToJump)
            {
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, jumpForce);
                readyToJump = false;
                StartCoroutine(JumpCooldown());
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnCollisionEnter2D ȣ���");

        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Player���� ����");
            // ������
        }

        if (collision.CompareTag("Ground"))
        {
            StuckInto(); // �浹 ������ ������
        }

    }

    public void Drop()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // ���������� ��ȯ
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(RotateZ());
    }

    // ���� �����°�
    private void StuckInto()
    {
        canRotate = false;                             // ȸ�� ����
        cd.enabled = false;                            // �浹 ����
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.angularVelocity = 0;

        // ȸ���� ����, ���߿� ������ �� �ְ� �̵��� ���
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 1�� �Ŀ� �÷��̾�� �������� ����
        StartCoroutine(DelayAndMoveToPlayer(1f));
    }

    // ���� �ð� �� �÷��̾� ������ �̵� ����
    IEnumerator DelayAndMoveToPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.parent = null; // ������ �и�
        rb.bodyType = RigidbodyType2D.Dynamic; // �� ���� ����
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // �̵��� ���, ȸ���� ����

        rb.gravityScale = 1f;

        isMovingToPlayer = true;
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f); // ���� �� ���� ����
        readyToJump = true;
    }

    private IEnumerator RotateZ()
    {
        float elapsed = 0f;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, 180f);

        while (elapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / rotateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
