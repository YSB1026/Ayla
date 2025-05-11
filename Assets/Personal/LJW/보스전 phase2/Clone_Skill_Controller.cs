using System.Collections;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private Transform player;
    private bool hasAttacked = false;

    [Header("공격 범위 설정")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackCheckRadius = 0.8f;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("사라짐 설정")]
    [SerializeField] private float disappearDelay = 0.5f;
    [SerializeField] private float fadeSpeed = 2f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FacePlayer();

        anim.Play("Clone_Attack");
    }

    private void FacePlayer()
    {
        if (player != null && transform.position.x > player.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // 좌우 반전
        }
    }

    // 애니메이션 이벤트에서 호출
    public void AnimationTrigger()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(disappearDelay);

        while (sr.color.a > 0)
        {
            sr.color = new Color(1f, 1f, 1f, sr.color.a - Time.deltaTime * fadeSpeed);
            yield return null;
        }

        Destroy(gameObject);
    }
    public void HitTrigger()
    {
        if (hasAttacked)
        {
            Debug.Log("이미 공격함");
            return;
        }

        Debug.Log("공격 실행");
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackCheckRadius, whatIsPlayer);

        if (hit != null)
        {
            Player playerScript = hit.GetComponent<Player>();
            if (playerScript != null)
            {
                Debug.Log("플레이어 처치!");
                playerScript.Die();
            }

            hasAttacked = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackCheckRadius);
    }
}