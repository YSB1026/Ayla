using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Spot Light 오브젝트에 붙여서
/// - 라이트 콘 방향으로 레이를 쏨
/// - Mirror 레이어에 맞으면 반사
/// - 그 외 레이어(Stop 레이어)에 맞으면 멈춤
/// - 라인렌더러로 경로를 색상 있게 시각화
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SpotReflector : MonoBehaviour
{
    [SerializeField] private Light2D reflectedLight;   // 반사 구간을 비출 라이트(자식으로 두거나, 씬에 하나 둬서 참조)
    [SerializeField] private float reflectedOuterAngle = 30f; // 반사 라이트 콘 각도
    [SerializeField] private float maxSegmentLightRadius = 8f; // 너무 커지지 않게 상한

    [Header("레이어 마스크")]
    [Tooltip("레이가 충돌할 전체 레이어")]
    [SerializeField] private LayerMask hitMask;
    [Tooltip("반사를 적용할 레이어")]
    [SerializeField] private LayerMask reflectMask;

    [Header("레이 파라미터")]
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private int maxReflections = 5;
    [SerializeField] private float surfaceOffset = 0.001f; // 반사 직후 재히트 방지용 미세 전진
    [SerializeField] private bool useCircleCast = true;
    [SerializeField] private float circleRadius = 0.05f;
    [SerializeField] private bool ignoreTriggerColliders = true;

    [Header("방향 설정")]
    [Tooltip("스포트라이트 콘 방향과 레이를 일치시킬지 여부")]
    [SerializeField] private bool alignWithSpotDirection = true;
    [Tooltip("align을 켠 상태에서 콘과 반대로 나가면 체크")]
    [SerializeField] private bool invertSpotDirection = false;
    [Tooltip("수동 방향을 쓰고 싶으면 align을 끄고 이 값을 사용")]
    [SerializeField] private Vector2 localDirection = Vector2.right;
    [Tooltip("레이 방향 보정 각도(도). 콘 축과 90도 어긋나면 ±90으로 맞춘다")]
    [SerializeField] private float rayAngleOffsetDeg = 0f;

    [Header("디버그")]
    [SerializeField] private bool drawLine = true;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color hitColor = new Color(0.3f, 1f, 1f);
    [SerializeField] private Color noHitColor = new Color(1f, 0.3f, 0.3f);

    [Header("반사 빛 프리팹 배치")]
    [Tooltip("반사 구간에만 프리팹 배치")]
    [SerializeField] private bool spawnReflectedPrefabs = true;
    [Tooltip("반사 빛으로 사용할 프리팹")]
    [SerializeField] private GameObject reflectedPrefab;
    [SerializeField] private int initialPool = 8;

    [Header("프리팹이 Light2D(웨지)일 때")]
    [Tooltip("세그먼트 방향 대비 회전 보정(도). 콘 축이 90도 어긋나 보이면 ±90으로 교정")]
    [SerializeField] private float segmentLightRotationOffsetDeg = 0f;
    [Tooltip("접점에서 반사 방향으로 아주 조금 앞으로 밀어내는 값(꼭짓점 미세 전진)")]
    [SerializeField] private float apexInset = 0.02f;
    [Tooltip("웨지 외곽 반경의 상한")]
    [SerializeField] private float maxOuterRadius = 12f;
    [Tooltip("웨지 내부 반경 비율(외곽 반경 대비)")]
    [SerializeField, Range(0f, 1f)] private float innerRadiusRatio = 0.5f;
    [Tooltip("웨지 외곽 각도(도)")]
    [SerializeField] private float outerAngleDeg = 30f;

    [Header("프리팹이 직사각형 스프라이트일 때")]
    [Tooltip("스프라이트 Pivot 이 가운데라고 가정하면 켜기, 뒤쪽 가장자리라면 끄기")]
    [SerializeField] private bool spritePivotIsCenter = true;
    [Tooltip("스프라이트의 월드 가로 길이(스케일 1 기준). 픽셀 퍼 유닛과 임포트 값에 따라 다름")]
    [SerializeField] private float spriteWorldWidthAtScale1 = 1f;
    [Tooltip("반사 빛의 두께(월드 단위)")]
    [SerializeField] private float spriteThicknessWorld = 0.2f;
    [Tooltip("접점에서 반사 방향으로 아주 조금 전진(겹침 방지)")]
    [SerializeField] private float spriteStartEdgeInset = 0.02f;

    // 받는 빛의 비율로 X 스케일만 조절
    [SerializeField] private bool scaleXByReceived = true;          // 켜면 동작
    [SerializeField, Range(0f, 1f)] private float minXScale = 0.1f;  // 최소 가늘기
    [SerializeField] private float xScaleAtFull = 1f;                // 100% 받을 때 X 스케일(기본 1)
    [SerializeField] private int incidentRayCount = 9;               // 입사 콘 샘플 수(홀수 권장)
    [SerializeField] private float incidentHalfAngleDeg = 20f;       // 입사 콘 반쪽 각(도)

    private LineRenderer line;
    private readonly List<Vector3> points = new List<Vector3>();
    private readonly List<GameObject> prefabPool = new List<GameObject>();
    private bool prevStartInColliders;
    private bool prevHitTriggers;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.widthMultiplier = lineWidth;
        line.positionCount = 0;

        if (line.material == null)
        {
            var shader = Shader.Find("Sprites/Default");
            if (shader != null) line.material = new Material(shader);
        }

        if (spawnReflectedPrefabs && reflectedPrefab != null)
        {
            for (int i = 0; i < initialPool; i++)
            {
                var go = Instantiate(reflectedPrefab, transform);
                go.SetActive(false);
                prefabPool.Add(go);
            }
        }
    }

    private void OnEnable()
    {
        prevStartInColliders = Physics2D.queriesStartInColliders;
        prevHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesStartInColliders = false;
        if (ignoreTriggerColliders) Physics2D.queriesHitTriggers = false;
    }

    private void OnDisable()
    {
        Physics2D.queriesStartInColliders = prevStartInColliders;
        Physics2D.queriesHitTriggers = prevHitTriggers;
    }

    private void Update()
    {
        BuildPath();
        DrawDebugLine();
        if (spawnReflectedPrefabs && reflectedPrefab != null)
            PlaceReflectedPrefabs();
        else
            DisableUnusedPrefabs(0);
    }

    // 경로 계산
    private void BuildPath()
    {
        points.Clear();

        Vector2 origin = transform.position;
        Vector2 dir = GetRayDirection();

        points.Add(origin);

        Vector2 pos = origin;
        Vector2 d = dir;

        for (int i = 0; i <= maxReflections; i++)
        {
            RaycastHit2D hit;
            bool got = TryCast(pos, d, out hit);
            if (got)
            {
                Vector2 hp = hit.point;
                points.Add(hp);

                int layer = hit.collider.gameObject.layer;
                bool isMirror = (reflectMask.value & (1 << layer)) != 0;

                if (isMirror)
                {
                    // 반사
                    Vector2 normal = hit.normal;
                    d = Vector2.Reflect(d, normal).normalized;

                    // 표면에서 살짝 앞으로 밀어내고 계속
                    float inset = Mathf.Max(surfaceOffset, useCircleCast ? circleRadius * 0.5f : surfaceOffset);
                    pos = hp + d * inset;
                    continue;
                }
                else
                {
                    // 거울이 아닌 곳에 막히면 종료
                    break;
                }
            }
            else
            {
                // 더 이상 맞는 것이 없으면 직진 후 종료
                points.Add(pos + d * rayDistance);
                break;
            }
        }
    }

    private Vector2 GetRayDirection()
    {
        Vector2 dir;
        if (alignWithSpotDirection)
            dir = (invertSpotDirection ? (Vector2)transform.up : -(Vector2)transform.up);
        else
            dir = (Vector2)transform.TransformDirection(localDirection);

        if (Mathf.Abs(rayAngleOffsetDeg) > 0.01f)
            dir = (Vector2)(Quaternion.Euler(0, 0, rayAngleOffsetDeg) * dir);

        return dir.normalized;
    }

    private bool TryCast(Vector2 origin, Vector2 dir, out RaycastHit2D hit)
    {
        if (useCircleCast)
            hit = Physics2D.CircleCast(origin, circleRadius, dir, rayDistance, hitMask);
        else
            hit = Physics2D.Raycast(origin, dir, rayDistance, hitMask);

        if (ignoreTriggerColliders && hit.collider != null && hit.collider.isTrigger)
            return false;

        return hit.collider != null;
    }

    private void DrawDebugLine()
    {
        if (!drawLine || points.Count < 2) return;

        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            line.SetPosition(i, points[i]);

        line.startColor = hitColor;
        line.endColor = hitColor;
    }

    private void PlaceReflectedPrefabs()
    {
        if (points.Count < 3)
        {
            DisableUnusedPrefabs(0);
            return;
        }

        int used = 0;
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 a = points[i];         // 시작점 = 거울 접점
            Vector2 b = points[i + 1];     // 끝점
            Vector2 seg = b - a;
            float len = seg.magnitude;
            if (len < 0.0001f) continue;

            GameObject go = GetOrCreateFromPool(ref used);
            PositionPrefabOnSegment(go, a, seg, len);

            /*// 1) 받은 비율 계산 (이전 점 → 접점 방향을 입사 중심으로 사용)
            float received = 1f;
            if (i >= 1)
            {
                Vector2 prev = (Vector2)points[i - 1];   // 명시적 캐스팅
                Vector2 contact = (Vector2)points[i];       // 접점
                Vector2 approachDir = (contact - prev).normalized;

                received = ComputeReceivedRatio(prev, approachDir);
            }

            // 2) X 스케일만 받은 비율에 맞게 조정
            ScaleXByReceived(go, received);
*/
            go.SetActive(true);
        }

        DisableUnusedPrefabs(used);
    }

    private GameObject GetOrCreateFromPool(ref int used)
    {
        if (used >= prefabPool.Count)
        {
            var extra = Instantiate(reflectedPrefab, transform);
            extra.SetActive(false);
            prefabPool.Add(extra);
        }
        return prefabPool[used++];
    }

    private void PositionPrefabOnSegment(GameObject go, Vector2 start, Vector2 seg, float len)
    {
        Vector2 dirN = seg / len;
        float angle = Mathf.Atan2(seg.y, seg.x) * Mathf.Rad2Deg + segmentLightRotationOffsetDeg;

        var l2d = go.GetComponent<Light2D>();
        if (l2d != null)
        {
            // 웨지 꼭짓점을 접점에 고정, 겹침 방지를 위해 소량 전진
            Vector2 apex = start + dirN * Mathf.Max(apexInset, surfaceOffset);
            go.transform.position = new Vector3(apex.x, apex.y, go.transform.position.z);
            go.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // 반경과 각도 세팅
            float r = Mathf.Min(maxOuterRadius, len);
            l2d.pointLightOuterRadius = r;
            l2d.pointLightInnerRadius = r * innerRadiusRatio;
            l2d.pointLightOuterAngle = outerAngleDeg;

            return;
        }

        // 스프라이트 모드(직사각형)
        var sr = go.GetComponent<SpriteRenderer>();
        // 위치: 피벗이 가운데면 중앙을 접점에서 앞으로 절반 + 인셋만큼 전진
        Vector2 center = spritePivotIsCenter
            ? start + dirN * (len * 0.5f + spriteStartEdgeInset)
            : start + dirN * spriteStartEdgeInset;

        go.transform.position = new Vector3(center.x, center.y, go.transform.position.z);
        go.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 스케일: 가로를 세그먼트 길이에 맞춤, 세로는 두께 고정
        float sx = len / Mathf.Max(0.0001f, spriteWorldWidthAtScale1);
        float sy = spriteThicknessWorld / Mathf.Max(0.0001f, spriteWorldWidthAtScale1);
        var s = go.transform.localScale;
        go.transform.localScale = new Vector3(sx, sy, s.z);
    }

    private void DisableUnusedPrefabs(int used)
    {
        for (int i = used; i < prefabPool.Count; i++)
            prefabPool[i].SetActive(false);
    }

    // 입사 콘을 여러 각도로 샘플해, 최초 히트가 '반사 레이어'인 비율(0..1)을 반환
    private float ComputeReceivedRatio(Vector2 sampleOrigin, Vector2 centerDir)
    {
        int rays = Mathf.Max(1, incidentRayCount);
        int hitAny = 0, hitReflect = 0;

        for (int i = 0; i < rays; i++)
        {
            float t = (rays == 1) ? 0f : (i / (float)(rays - 1));  // 0..1
            float lerp = t * 2f - 1f;                              // -1..1
            float ang = lerp * incidentHalfAngleDeg;               // -half..+half
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, ang) * centerDir);
            dir.Normalize();

            RaycastHit2D hit;
            bool got = TryCast(sampleOrigin, dir, out hit);        // 네가 쓰는 캐스트 헬퍼 재사용
            if (!got) continue;

            hitAny++;
            int layer = hit.collider.gameObject.layer;
            bool isMirror = (reflectMask.value & (1 << layer)) != 0;
            if (isMirror) hitReflect++;
        }

        if (hitAny == 0) return 0f;
        return (float)hitReflect / hitAny; // 0.0~1.0
    }

    private void ScaleXByReceived(GameObject go, float received)
    {
        if (!scaleXByReceived) return;
        float x = Mathf.Lerp(minXScale, xScaleAtFull, Mathf.Clamp01(received));
        var s = go.transform.localScale;
        go.transform.localScale = new Vector3(x, s.y, s.z);
    }

}
