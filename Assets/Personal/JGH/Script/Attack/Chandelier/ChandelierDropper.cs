using UnityEngine;

public class ChandelierDropper : MonoBehaviour
{
    public Rigidbody2D chandelierRb;
    public Animator chandelierAnimator;

    public float groundCheckDistance = 0.5f;        // 땅과의 거리 감지 범위
    public LayerMask groundLayer;                   // Ground 레이어 마스크
    public Transform groundCheckpoint;              // 사용자가 설정할 ground 기준점

    private bool hasDropped = false;
    private bool isBreaking = false;

    void Start()
    {
        chandelierRb.isKinematic = true; // 처음엔 떨어지지 않음
    }

    public void DropChandelier()
    {
        if (hasDropped) return;

        hasDropped = true;
        chandelierRb.isKinematic = false; // 중력 적용
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropChandelier();
        }

        if (hasDropped && !isBreaking && groundCheckpoint != null)
        {
            // 지정한 위치에서 바닥 체크 Raycast
            RaycastHit2D hit = Physics2D.Raycast(groundCheckpoint.position, Vector2.down, groundCheckDistance, groundLayer);
            if (hit.collider != null)
            {
                if (chandelierAnimator != null)
                {
                    chandelierAnimator.SetTrigger("Break");
                    isBreaking = true;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasDropped) return;

        // Ground 충돌
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!isBreaking)
            {
                chandelierAnimator.speed = 1.5f;
                chandelierAnimator.SetTrigger("Break");
                isBreaking = true;
            }

            chandelierRb.bodyType = RigidbodyType2D.Static;
            return; // 여기서 리턴 추가
        }

        // Player 충돌
        if (collision.gameObject.CompareTag("Player"))
        {
            // 이미 멈춘 상태라면 아무 일도 하지 않음
            if (chandelierRb.bodyType == RigidbodyType2D.Static)
                return;

            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Die(); // 애니메이션 → 리스폰 흐름
            }

            if (!isBreaking)
            {
                chandelierRb.linearVelocity = Vector2.zero;
                chandelierRb.bodyType = RigidbodyType2D.Static;

                chandelierAnimator.speed = 2.0f;
                chandelierAnimator.SetTrigger("Break");

                isBreaking = true;
            }
        }
    }






}
