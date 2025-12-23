using UnityEngine;

public class ShadowAbility : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private GameObject shadow; // 조종할 그림자
    [SerializeField] private float moveSpeed = 5f;    // 그림자 이동 속도

    private Rigidbody2D shadowRb;
    private void Start()
    {
        shadowRb = shadow.GetComponent<Rigidbody2D>();
        // 시작할 때 그림자는 숨기기
        if (shadow != null) shadow.SetActive(false);
    }

    private void Update()
    {
        // Q키로 껐다 켰다 하기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleShadow();
        }
    }

    private void FixedUpdate()
    {
        // 그림자가 켜져 있고, Rigidbody가 있을 때만 이동
        if (shadow != null && shadow.activeSelf && shadowRb != null)
        {
            MoveShadowPhysics();
        }
    }

    // 그림자 끄고 켜는 함수
    private void ToggleShadow()
    {
        if (shadow == null) return;

        bool isActive = !shadow.activeSelf; // 현재 상태의 반대로 설정
        shadow.SetActive(isActive);

        if (isActive)
        {
            // 그림자를 플레이어 위치로 소환
            shadow.transform.position = transform.position;
            Debug.Log("그림자 모드 ON");
        }
        else
        {
            if (shadowRb != null) shadowRb.linearVelocity = Vector2.zero;
            Debug.Log("그림자 모드 OFF");
        }
    }

    private void MoveShadowPhysics()
    {
        // 좌우 입력만 받습니다 (A/D)
        float xInput = Input.GetAxisRaw("Horizontal");

        // Y축(위아래)은 건드리지 않고, 현재 떨어지는 속도(gravity)를 유지합니다.
        // X축(좌우)만 내 입력대로 속도를 줍니다.
        shadowRb.linearVelocity = new Vector2(xInput * moveSpeed, shadowRb.linearVelocity.y);
    }
}