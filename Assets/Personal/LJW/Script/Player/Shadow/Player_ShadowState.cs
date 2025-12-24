using UnityEngine;

public class Player_ShadowState : PlayerState
{
    public Player_ShadowState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    private Transform anchor;
    private SpriteRenderer sr;

    public override void Enter()
    {
        base.Enter();

        // 1. 플레이어를 멈추기
        player.SetZeroVelocity();

        // 2. 그림자 능력을 켜기
        if (player.shadowAbility != null)
        {
            // 플레이어의 현재 위치를 전달하며 키기
            player.shadowAbility.ActivateShadow(player.transform.position);
        }

        // ====================================================
        // [복사해온 부분] 숨는 효과 적용 (HideState 로직)
        // ====================================================

        // 1. 앵커 정보 가져오기 (혹시 모르니)
        anchor = player.GetHideAnchor();

        // 2. 앉기 애니메이션 & 콜라이더 적용
        if (player.anim != null) player.anim.SetBool("Sit", true);
        player.SetSitCollider();

        // 3. 반투명 처리
        sr = player.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0.2f);
        }

        // 4. 레이어 변경 (HiddenPlayer)
        int hiddenLayer = LayerMask.NameToLayer("HiddenPlayer");
        if (hiddenLayer != -1) SetLayerRecursively(player.gameObject, hiddenLayer);

        // 5. 숨김 상태 플래그
        player.SetHidden(true);

        // 6. 위치 고정 (스냅)
        if (anchor != null)
        {
            player.transform.position = new Vector3(
                anchor.position.x, anchor.position.y, player.transform.position.z);
        }
    }

    public override void Update()
    {
        base.Update();

        // 3. 이동 코드 무시
        player.SetZeroVelocity();

        // 앵커에 계속 고정
        if (anchor != null)
        {
            player.transform.position = new Vector3(
                anchor.position.x, anchor.position.y, player.transform.position.z);
        }

        // 4. Q키를 다시 누르면 원래 상태(Hide)로 복귀
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.hideState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 5. 상태를 나갈 때 그림자 능력을 끄기
        if (player.shadowAbility != null)
        {
            player.shadowAbility.DeactivateShadow();
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}