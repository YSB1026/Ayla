using System.Collections;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private Transform player;
    private bool hasAttacked = false;

    [Header("���� ���� ����")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackCheckRadius = 0.8f;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("����� ����")]
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
            transform.localScale = new Vector3(-1f, 1f, 1f); // �¿� ����
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��
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
            Debug.Log("�̹� ������");
            return;
        }

        Debug.Log("���� ����");
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackCheckRadius, whatIsPlayer);

        if (hit != null)
        {
            Player playerScript = hit.GetComponent<Player>();
            if (playerScript != null)
            {
                Debug.Log("�÷��̾� óġ!");
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