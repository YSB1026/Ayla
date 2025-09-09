using UnityEngine;

public class Player_HideState : PlayerState
{
    private Transform anchor;
    private bool prevControlEnabled;
    private bool prevColliderState;
    private Collider2D col;
    private SpriteRenderer sr;

    // 앉은 포즈 유지용 (네 애니메이터 파라미터명에 맞춰 변경 가능)
    private static readonly int HashIsCrouch = Animator.StringToHash("isCrouch");
    private static readonly int HashHideEnter = Animator.StringToHash("HideEnter");
    private static readonly int HashHideExit = Animator.StringToHash("HideExit");

    // 앵커로 부드럽게 붙는 연출 (선택)
    private float approachTime = 0.12f;
    private float approachT;
    private Vector3 startPos;

    private float originalScaleX;

    public Player_HideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        anchor = player.GetHideAnchor();
        player.SetZeroVelocity();

        // 제어 잠금
        prevControlEnabled = player.controlEnabled;
        player.SetControlActive(false);               // 제어 비활성화

        col = player.GetComponent<Collider2D>();
        if (col != null)
        {
            prevColliderState = col.enabled;
            col.enabled = false; // 충돌 비활성화로 완전 숨김
        }

        sr = player.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0.2f); // 살짝만 보이게
        }

        // 앵커 방향 보도록 회전
        originalScaleX = player.transform.localScale.x;
        if (anchor != null)
        {
            float dir = Mathf.Sign(anchor.position.x - player.transform.position.x);
            if (dir != 0f)
            {
                var sc = player.transform.localScale;
                sc.x = Mathf.Abs(sc.x) * (dir > 0 ? 1f : -1f);
                player.transform.localScale = sc;
            }
        }

        // 숨기 진입 트리거(있으면 사용)
        if (player.anim != null) player.anim.SetTrigger(HashHideEnter);

        // 앉기: SitState가 하던 일을 여기서 직접 수행
        // 1) 앉은 콜라이더 적용
        player.SetSitCollider();
        // 2) 웅크림 파라미터 켜서 "앉은 포즈" 유지
        if (player.anim != null) player.anim.SetBool(HashIsCrouch, true);
    }

    public override void Update()
    {
        base.Update();

        // F로 토글 해제
        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(player.standState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 웅크림 해제
        if (player.anim != null)
        {
            player.anim.ResetTrigger(HashHideEnter);
            player.anim.SetBool(HashIsCrouch, false);
            player.anim.SetTrigger(HashHideExit); // 있으면 자연스러움
        }

        // 콜라이더/투명도/컨트롤 복구
        player.SetIdleCollider();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1f);
        }
        if (col != null) col.enabled = prevColliderState;
        player.SetControlActive(prevControlEnabled);

        // 얼굴 방향 복구(원치 않으면 제거)
        var sc = player.transform.localScale;
        sc.x = originalScaleX;
        player.transform.localScale = sc;
    }
}
