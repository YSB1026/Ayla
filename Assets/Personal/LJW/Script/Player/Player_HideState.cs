using UnityEngine;

public class Player_HideState : PlayerState
{
    private Transform anchor;

    private Rigidbody2D rb;
    private RigidbodyType2D prevBodyType;
    private float prevGravityScale;
    private RigidbodyConstraints2D prevConstraints;

    private SpriteRenderer sr;

    public Player_HideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        anchor = player.GetHideAnchor();
        player.SetZeroVelocity();

        // 물리 잠금(컨트롤 비활성화는 사용하지 않음)
        rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            prevBodyType = rb.bodyType;
            prevGravityScale = rb.gravityScale;
            prevConstraints = rb.constraints;

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // 숨는 동안 앉기 유지 (애니 파라미터 + 콜라이더)
        if (player.anim != null) player.anim.SetBool("Sit", true);
        player.SetSitCollider();

        // 반투명 처리
        sr = player.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0.2f);
        }

        // 앵커로 즉시 스냅
        if (anchor != null)
        {
            player.transform.position = new Vector3(
                anchor.position.x, anchor.position.y, player.transform.position.z);
        }
    }

    public override void Update()
    {
        base.Update();

        // 앵커에 계속 고정 (원치 않으면 이 블록 지워도 됨)
        if (anchor != null)
        {
            player.transform.position = new Vector3(
                anchor.position.x, anchor.position.y, player.transform.position.z);
        }

        // F로 해제 → 서기(Stand) 상태로 전환
        if (Input.GetKeyDown(KeyCode.F))
            stateMachine.ChangeState(player.standState);
    }

    public override void Exit()
    {
        base.Exit();

        // 앉기 해제 (애니 + 콜라이더 원복)
        if (player.anim != null) player.anim.SetBool("Sit", false);
        player.SetIdleCollider();

        // 물리 원복
        if (rb != null)
        {
            rb.bodyType = prevBodyType;
            rb.gravityScale = prevGravityScale;
            rb.constraints = prevConstraints;
        }

        // 투명도 원복
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1f);
        }
    }
}
