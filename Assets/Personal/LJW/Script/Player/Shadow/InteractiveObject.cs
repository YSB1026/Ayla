using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 플레이어가 물건을 잡았을 때 (밀림 방지 해제)
    public void FreezeObject(bool freeze)
    {
        if (rb != null)
        {
            if (freeze)
                rb.constraints = RigidbodyConstraints2D.FreezeAll; // 고정
            else
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전만 잠그고 이동 허용
        }
    }

    // 실제 물건 이동시키는 함수
    public void MoveObject(float direction)
    {
        // 여기서 물체 이동 로직 구현 (필요에 따라 수정 가능)
        // 예시로 단순히 속도를 주는 방식보다는, 플레이어가 미는 힘을 받는 식이어야 하지만
        // 일단 에러를 잡기 위해 빈 함수라도 있어야 합니다.
        // 현재 PlayerState 로직상 플레이어가 직접 transform을 밀고 있으므로
        // 여기서는 충돌 처리나 물리적인 보정만 해주면 됩니다.
    }
}