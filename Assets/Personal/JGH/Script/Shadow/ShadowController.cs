using UnityEngine;

public class ShadowPuller2D : MonoBehaviour
{
    public GameObject shadowObject;
    public Material shadowMaterial;

    public KeyCode scaleKey = KeyCode.RightArrow;
    public float scaleSpeed = 2f;
    public float maxScaleX = 3f;

    public float directionSpeed = 1f;
    [Range(-1f, 1f)]
    public float shadowDirection = 0f;
    private Vector3 originalScale;

    [Header("Shadow Pull Settings")]
    public Transform shadowPoint;
    public float detectRadius = 0.5f;
    public KeyCode pullKey = KeyCode.Space;
    public LayerMask pullLayer;
    public float pullSpeed = 3f;
    
    // Shadow point positioning
    private Vector3 shadowPointOriginalPosition;
    public float shadowPointMaxOffset = 2f; // Max offset in either direction (-2 to 2)

    private GameObject pullingTarget;
    
    [Header("Light Interaction")]
    [SerializeField] private bool inLight = false; // 빛 안에 있는지 여부
    public float shadowShrinkSpeed = 3f; // 빛 안에서 그림자가 줄어드는 속도
    public float minScaleX = 0.5f; // 최소 그림자 크기

    void Start()
    {
        if (shadowObject != null)
            originalScale = shadowObject.transform.localScale;

        if (shadowMaterial != null)
            shadowMaterial.SetFloat("_Direction", shadowDirection);
            
        // Store the original position of shadow point
        if (shadowPoint != null)
            shadowPointOriginalPosition = shadowPoint.localPosition;
    }

    void Update()
    {
        if (shadowObject == null || shadowMaterial == null) return;

        Vector3 currentScale = shadowObject.transform.localScale;

        // 그림자 확장/축소 로직 (빛 상호작용 포함)
        if (Input.GetKey(scaleKey) && !inLight)
        {
            // 빛 안에 없을 때만 확장 가능
            currentScale.x = Mathf.Min(currentScale.x + scaleSpeed * Time.deltaTime, maxScaleX);
        }
        else if (inLight)
        {
            // 빛 안에 있으면 그림자 크기 줄이기
            currentScale.x = Mathf.Max(currentScale.x - shadowShrinkSpeed * Time.deltaTime, minScaleX);
        }
        else
        {
            // 평소에는 원래 크기로 서서히 돌아감
            currentScale = Vector3.Lerp(currentScale, originalScale, Time.deltaTime * scaleSpeed);
        }
        shadowObject.transform.localScale = currentScale;

        // 그림자 방향 제어
        if (Input.GetKey(KeyCode.Q)) shadowDirection -= directionSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) shadowDirection += directionSpeed * Time.deltaTime;
        shadowDirection = Mathf.Clamp(shadowDirection, -1f, 1f);
        shadowMaterial.SetFloat("_Direction", shadowDirection);
        
        // Update shadow point position based on shadowDirection
        if (shadowPoint != null)
        {
            Vector3 newPosition = shadowPointOriginalPosition;
            newPosition.x += shadowDirection * shadowPointMaxOffset; // Map -1 to 1 to -2 to 2
            shadowPoint.localPosition = newPosition;
        }

        // 끌어오기 감지 및 처리
        if (!Input.GetKey(scaleKey))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(shadowPoint.position, detectRadius, pullLayer);
            if (hits.Length > 0)
            {
                GameObject target = hits[0].gameObject;

                if (Input.GetKey(pullKey))
                {
                    pullingTarget = target;
                }
            }
        }

        // 끌기 동작
        if (pullingTarget != null)
        {
            Vector3 targetPos = pullingTarget.transform.position;
            Vector3 pullTargetPos = Vector3.Lerp(targetPos, shadowPoint.position, pullSpeed * Time.deltaTime);
            pullingTarget.transform.position = pullTargetPos;

            // 끌기 중 키를 떼면 멈춤
            if (!Input.GetKey(pullKey))
            {
                pullingTarget = null;
            }
        }
    }
    
    // LightMeshPlayerDetector에서 호출하는 메서드
    public void SetInLightStatus(bool isInLight)
    {
        inLight = isInLight;
        
        // 상태 변경 시 디버그 로그 추가
        if (inLight != isInLight)
        {
            Debug.Log($"그림자 포인트 상태: {(isInLight ? "빛 안에 있음" : "빛 밖에 있음")}");
        }
    }
    
    // 그림자 포인트가 빛 안에 있는지 확인하는 메서드 (Gizmo 및 외부 클래스용)
    public bool IsInLight()
    {
        return inLight;
    }

    void OnDrawGizmosSelected()
    {
        // ShadowPoint 감지 반경 시각화
        if (shadowPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(shadowPoint.position, detectRadius);
            
            // 빛 상태에 따라 색상 변경
            if (Application.isPlaying)
            {
                Gizmos.color = inLight ? Color.yellow : Color.green;
                Gizmos.DrawSphere(shadowPoint.position, 0.15f);
            }
        }
    }
}