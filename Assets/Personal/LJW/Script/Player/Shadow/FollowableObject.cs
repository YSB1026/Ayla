using UnityEngine;

public class FollowableObject : MonoBehaviour
{
    [Header("들기 설정")]
    public Vector2 holdOffset = new Vector2(0.5f, 0.5f); // Shadow로부터의 오프셋 (x, y)
    public float followSmoothness = 10f; // 부드러운 이동 속도
    
    private Shadow holdingShadow;
    private bool isBeingHeld = false;
    private Rigidbody2D rb;
    private Collider2D col;
    
    // 원래 물리 설정 저장
    private RigidbodyType2D originalBodyType;
    private bool originalGravity;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
        // 원래 설정 저장
        if (rb != null)
        {
            originalBodyType = rb.bodyType;
            originalGravity = rb.gravityScale > 0;
        }
    }

    private void Update()
    {
        if (isBeingHeld && holdingShadow != null)
        {
            FollowShadowHand();
        }
    }

    private void FollowShadowHand()
    {
        // Shadow의 손 위치 계산 (Shadow 앞쪽 + 오프셋)
        Vector2 targetPosition = (Vector2)holdingShadow.transform.position + 
                                 new Vector2(holdingShadow.facingDir * holdOffset.x, holdOffset.y);
        
        // 부드럽게 이동
        transform.position = Vector2.Lerp(transform.position, targetPosition, followSmoothness * Time.deltaTime);
    }

    // 마우스 클릭 감지
    private void OnMouseDown()
    {
        Shadow shadow = FindFirstObjectByType<Shadow>();
        
        if (shadow != null && shadow.gameObject.activeSelf)
        {
            if (isBeingHeld)
            {
                // 내려놓기
                Drop();
            }
            else
            {
                // 들기
                PickUp(shadow);
            }
        }
    }

    // 오브젝트 들기
    public void PickUp(Shadow shadow)
    {
        holdingShadow = shadow;
        isBeingHeld = true;
        
        // 물리 효과 비활성화
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
        
        // Shadow에 등록
        shadow.AddHeldObject(this);
        
        Debug.Log($"{gameObject.name}을(를) 들었습니다!");
    }

    // 오브젝트 내려놓기
    public void Drop()
    {
        if (holdingShadow != null)
        {
            holdingShadow.RemoveHeldObject(this);
        }
        
        isBeingHeld = false;
        
        // 물리 효과 복구
        if (rb != null)
        {
            rb.bodyType = originalBodyType;
        }
        
        holdingShadow = null;
        
        Debug.Log($"{gameObject.name}을(를) 내려놓았습니다!");
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }

    public Shadow GetHoldingShadow()
    {
        return holdingShadow;
    }

    private void OnDrawGizmos()
    {
        if (isBeingHeld && holdingShadow != null)
        {
            // 들고 있는 상태 시각화
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
            Gizmos.DrawLine(transform.position, holdingShadow.transform.position);
        }
    }
}