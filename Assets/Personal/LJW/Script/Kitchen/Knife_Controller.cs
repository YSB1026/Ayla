using UnityEngine;
using System.Collections;

public class KnifeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    [SerializeField] private Transform gravityPivot; // 칼 끝 위치
    [SerializeField] private float rotateDuration = 1f; // 회전 완료까지 걸릴 시간
    [SerializeField] private float moveSpeed = 2f;         // 플레이어에게 다가가는 속도

    [SerializeField] private Player player;

    private bool canRotate = true;
    private bool isMovingToPlayer = false;   // 칼이 플레이어 쪽으로 끌려가는 중인지

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
                Debug.Log($"Player 자동 할당 성공: {player.name}");
            else
                Debug.LogWarning("Player 자동 할당 실패! 씬에 Player가 없음");
        }
    }

    private void Update()
    {
        if (canRotate && gravityPivot != null)
        {
            Vector2 fallDir = rb.linearVelocity.normalized;

            if (fallDir.sqrMagnitude > 0.01f)
            {
                // 회전: 칼 끝이 중력 방향을 향하도록
                Quaternion targetRot = Quaternion.FromToRotation(gravityPivot.up, fallDir) * transform.rotation;

                // 한 프레임당 회전 속도 = 전체 회전 각 / 걸릴 시간
                float maxDegreesPerFrame = 360f / rotateDuration;

                // 현재 회전에서 목표 회전까지 부드럽게 회전
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    maxDegreesPerFrame * Time.deltaTime
                );
            }
        }

        // 플레이어를 향해 칼이 끌려가는 처리
        if (isMovingToPlayer)
        {
            if (player == null)
            {
                Debug.LogWarning("isMovingToPlayer는 true인데, player는 null입니다!");
                return;
            }

            Vector2 target = player.transform.position;
            Vector2 direction = (target - (Vector2)transform.position).normalized;

            // 땅에 닿아 있을 때만 점프 가능
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
        Debug.Log("OnCollisionEnter2D 호출됨");

        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Player에게 닿음");
            // 데미지
        }

        if (collision.CompareTag("Ground"))
        {
            StuckInto(); // 충돌 지점에 박히기
        }

    }

    public void Drop()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // 떨어지도록 전환
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(RotateZ());
    }

    // 땅에 꽂히는거
    private void StuckInto()
    {
        canRotate = false;                             // 회전 멈춤
        cd.enabled = false;                            // 충돌 제거
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.angularVelocity = 0;

        // 회전만 고정, 나중에 움직일 수 있게 이동은 허용
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 1초 후에 플레이어에게 끌려가기 시작
        StartCoroutine(DelayAndMoveToPlayer(1f));
    }

    // 일정 시간 뒤 플레이어 쪽으로 이동 시작
    IEnumerator DelayAndMoveToPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.parent = null; // 땅에서 분리
        rb.bodyType = RigidbodyType2D.Dynamic; // 이 상태 유지
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 이동은 허용, 회전만 고정

        rb.gravityScale = 1f;

        isMovingToPlayer = true;
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f); // 점프 간 간격 조정
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
