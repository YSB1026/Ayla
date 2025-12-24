using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("설정")]
    public float pushSpeed = 3f;

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
            {
                // 잡지 않을 땐 모든 움직임 고정 (밀림 방지)
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                // 회전과 Y축 이동을 막고, X축 이동만 허용
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }

    // 실제 물건 이동시키는 함수
    public void MoveObject(float direction) // direction은 1 또는 -1
    {
        transform.position += new Vector3(direction * pushSpeed * Time.deltaTime, 0, 0);
    }
}