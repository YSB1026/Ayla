using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Spot Light ������Ʈ�� �ٿ���
/// - ����Ʈ �� �������� ���̸� ��
/// - Mirror ���̾ ������ �ݻ�
/// - �� �� ���̾�(Stop ���̾�)�� ������ ����
/// - ���η������� ��θ� ���� �ְ� �ð�ȭ
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SpotReflector : MonoBehaviour
{
    [SerializeField] private Light2D reflectedLight;   // �ݻ� ������ ���� ����Ʈ(�ڽ����� �ΰų�, ���� �ϳ� �ּ� ����)
    [SerializeField] private float reflectedOuterAngle = 30f; // �ݻ� ����Ʈ �� ����
    [SerializeField] private float maxSegmentLightRadius = 8f; // �ʹ� Ŀ���� �ʰ� ����

    [Header("���̾� ����ũ")]
    [Tooltip("���̰� �浹�� ��ü ���̾�")]
    [SerializeField] private LayerMask hitMask;
    [Tooltip("�ݻ縦 ������ ���̾�")]
    [SerializeField] private LayerMask reflectMask;

    [Header("���� �Ķ����")]
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private int maxReflections = 5;
    [SerializeField] private float surfaceOffset = 0.001f; // �ݻ� ���� ����Ʈ ������ �̼� ����
    [SerializeField] private bool useCircleCast = true;
    [SerializeField] private float circleRadius = 0.05f;
    [SerializeField] private bool ignoreTriggerColliders = true;

    [Header("���� ����")]
    [Tooltip("����Ʈ����Ʈ �� ����� ���̸� ��ġ��ų�� ����")]
    [SerializeField] private bool alignWithSpotDirection = true;
    [Tooltip("align�� �� ���¿��� �ܰ� �ݴ�� ������ üũ")]
    [SerializeField] private bool invertSpotDirection = false;
    [Tooltip("���� ������ ���� ������ align�� ���� �� ���� ���")]
    [SerializeField] private Vector2 localDirection = Vector2.right;
    [Tooltip("���� ���� ���� ����(��). �� ��� 90�� ��߳��� ��90���� �����")]
    [SerializeField] private float rayAngleOffsetDeg = 0f;

    [Header("�����")]
    [SerializeField] private bool drawLine = true;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color hitColor = new Color(0.3f, 1f, 1f);
    [SerializeField] private Color noHitColor = new Color(1f, 0.3f, 0.3f);

    [Header("�ݻ� �� ������ ��ġ")]
    [Tooltip("�ݻ� �������� ������ ��ġ")]
    [SerializeField] private bool spawnReflectedPrefabs = true;
    [Tooltip("�ݻ� ������ ����� ������")]
    [SerializeField] private GameObject reflectedPrefab;
    [SerializeField] private int initialPool = 8;

    [Header("�������� Light2D(����)�� ��")]
    [Tooltip("���׸�Ʈ ���� ��� ȸ�� ����(��). �� ���� 90�� ��߳� ���̸� ��90���� ����")]
    [SerializeField] private float segmentLightRotationOffsetDeg = 0f;
    [Tooltip("�������� �ݻ� �������� ���� ���� ������ �о�� ��(������ �̼� ����)")]
    [SerializeField] private float apexInset = 0.02f;
    [Tooltip("���� �ܰ� �ݰ��� ����")]
    [SerializeField] private float maxOuterRadius = 12f;
    [Tooltip("���� ���� �ݰ� ����(�ܰ� �ݰ� ���)")]
    [SerializeField, Range(0f, 1f)] private float innerRadiusRatio = 0.5f;
    [Tooltip("���� �ܰ� ����(��)")]
    [SerializeField] private float outerAngleDeg = 30f;

    [Header("�������� ���簢�� ��������Ʈ�� ��")]
    [Tooltip("��������Ʈ Pivot �� ������ �����ϸ� �ѱ�, ���� �����ڸ���� ����")]
    [SerializeField] private bool spritePivotIsCenter = true;
    [Tooltip("��������Ʈ�� ���� ���� ����(������ 1 ����). �ȼ� �� ���ְ� ����Ʈ ���� ���� �ٸ�")]
    [SerializeField] private float spriteWorldWidthAtScale1 = 1f;
    [Tooltip("�ݻ� ���� �β�(���� ����)")]
    [SerializeField] private float spriteThicknessWorld = 0.2f;
    [Tooltip("�������� �ݻ� �������� ���� ���� ����(��ħ ����)")]
    [SerializeField] private float spriteStartEdgeInset = 0.02f;

    // �޴� ���� ������ X �����ϸ� ����
    [SerializeField] private bool scaleXByReceived = true;          // �Ѹ� ����
    [SerializeField, Range(0f, 1f)] private float minXScale = 0.1f;  // �ּ� ���ñ�
    [SerializeField] private float xScaleAtFull = 1f;                // 100% ���� �� X ������(�⺻ 1)
    [SerializeField] private int incidentRayCount = 9;               // �Ի� �� ���� ��(Ȧ�� ����)
    [SerializeField] private float incidentHalfAngleDeg = 20f;       // �Ի� �� ���� ��(��)

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

    // ��� ���
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
                    // �ݻ�
                    Vector2 normal = hit.normal;
                    d = Vector2.Reflect(d, normal).normalized;

                    // ǥ�鿡�� ��¦ ������ �о�� ���
                    float inset = Mathf.Max(surfaceOffset, useCircleCast ? circleRadius * 0.5f : surfaceOffset);
                    pos = hp + d * inset;
                    continue;
                }
                else
                {
                    // �ſ��� �ƴ� ���� ������ ����
                    break;
                }
            }
            else
            {
                // �� �̻� �´� ���� ������ ���� �� ����
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
            Vector2 a = points[i];         // ������ = �ſ� ����
            Vector2 b = points[i + 1];     // ����
            Vector2 seg = b - a;
            float len = seg.magnitude;
            if (len < 0.0001f) continue;

            GameObject go = GetOrCreateFromPool(ref used);
            PositionPrefabOnSegment(go, a, seg, len);

            /*// 1) ���� ���� ��� (���� �� �� ���� ������ �Ի� �߽����� ���)
            float received = 1f;
            if (i >= 1)
            {
                Vector2 prev = (Vector2)points[i - 1];   // ����� ĳ����
                Vector2 contact = (Vector2)points[i];       // ����
                Vector2 approachDir = (contact - prev).normalized;

                received = ComputeReceivedRatio(prev, approachDir);
            }

            // 2) X �����ϸ� ���� ������ �°� ����
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
            // ���� �������� ������ ����, ��ħ ������ ���� �ҷ� ����
            Vector2 apex = start + dirN * Mathf.Max(apexInset, surfaceOffset);
            go.transform.position = new Vector3(apex.x, apex.y, go.transform.position.z);
            go.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // �ݰ�� ���� ����
            float r = Mathf.Min(maxOuterRadius, len);
            l2d.pointLightOuterRadius = r;
            l2d.pointLightInnerRadius = r * innerRadiusRatio;
            l2d.pointLightOuterAngle = outerAngleDeg;

            return;
        }

        // ��������Ʈ ���(���簢��)
        var sr = go.GetComponent<SpriteRenderer>();
        // ��ġ: �ǹ��� ����� �߾��� �������� ������ ���� + �μ¸�ŭ ����
        Vector2 center = spritePivotIsCenter
            ? start + dirN * (len * 0.5f + spriteStartEdgeInset)
            : start + dirN * spriteStartEdgeInset;

        go.transform.position = new Vector3(center.x, center.y, go.transform.position.z);
        go.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // ������: ���θ� ���׸�Ʈ ���̿� ����, ���δ� �β� ����
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

    // �Ի� ���� ���� ������ ������, ���� ��Ʈ�� '�ݻ� ���̾�'�� ����(0..1)�� ��ȯ
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
            bool got = TryCast(sampleOrigin, dir, out hit);        // �װ� ���� ĳ��Ʈ ���� ����
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
