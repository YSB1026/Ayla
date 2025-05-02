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

        // 그림자 확장
        if (Input.GetKey(scaleKey))
        {
            currentScale.x = Mathf.Min(currentScale.x + scaleSpeed * Time.deltaTime, maxScaleX);
        }
        else
        {
            currentScale = Vector3.Lerp(currentScale, originalScale, Time.deltaTime * scaleSpeed);
        }
        shadowObject.transform.localScale = currentScale;

        // 그림자 방향 제어
        if (Input.GetKey(KeyCode.A)) shadowDirection -= directionSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) shadowDirection += directionSpeed * Time.deltaTime;
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

    void OnDrawGizmosSelected()
    {
        // ShadowPoint 감지 반경 시각화
        if (shadowPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(shadowPoint.position, detectRadius);
        }
    }
}