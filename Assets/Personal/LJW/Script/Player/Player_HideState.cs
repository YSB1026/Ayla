using UnityEngine;

public class Player_HideState : PlayerState
{
    private Transform anchor;
    private bool prevControlEnabled;
    private bool prevColliderState;
    private Collider2D col;
    private SpriteRenderer sr;

    // ���� ���� ������ (�� �ִϸ����� �Ķ���͸� ���� ���� ����)
    private static readonly int HashIsCrouch = Animator.StringToHash("isCrouch");
    private static readonly int HashHideEnter = Animator.StringToHash("HideEnter");
    private static readonly int HashHideExit = Animator.StringToHash("HideExit");

    // ��Ŀ�� �ε巴�� �ٴ� ���� (����)
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

        // ���� ���
        prevControlEnabled = player.controlEnabled;
        player.SetControlActive(false);               // ���� ��Ȱ��ȭ

        col = player.GetComponent<Collider2D>();
        if (col != null)
        {
            prevColliderState = col.enabled;
            col.enabled = false; // �浹 ��Ȱ��ȭ�� ���� ����
        }

        sr = player.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0.2f); // ��¦�� ���̰�
        }

        // ��Ŀ ���� ������ ȸ��
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

        // ���� ���� Ʈ����(������ ���)
        if (player.anim != null) player.anim.SetTrigger(HashHideEnter);

        // �ɱ�: SitState�� �ϴ� ���� ���⼭ ���� ����
        // 1) ���� �ݶ��̴� ����
        player.SetSitCollider();
        // 2) ��ũ�� �Ķ���� �Ѽ� "���� ����" ����
        if (player.anim != null) player.anim.SetBool(HashIsCrouch, true);
    }

    public override void Update()
    {
        base.Update();

        // F�� ��� ����
        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(player.standState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // ��ũ�� ����
        if (player.anim != null)
        {
            player.anim.ResetTrigger(HashHideEnter);
            player.anim.SetBool(HashIsCrouch, false);
            player.anim.SetTrigger(HashHideExit); // ������ �ڿ�������
        }

        // �ݶ��̴�/����/��Ʈ�� ����
        player.SetIdleCollider();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1f);
        }
        if (col != null) col.enabled = prevColliderState;
        player.SetControlActive(prevControlEnabled);

        // �� ���� ����(��ġ ������ ����)
        var sc = player.transform.localScale;
        sc.x = originalScaleX;
        player.transform.localScale = sc;
    }
}
