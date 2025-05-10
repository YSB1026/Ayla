using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightDetection : MonoBehaviour
{
    public Enemy_Light enemy;           // 빛 감지 대상
    private bool enemyDetected = false; // 감지 상태 저장

    private Mesh lightMesh;             // 광원 영역
    private Vector3[] meshWorldPoints;  // 매쉬 꼭짓점을 월드 좌표로 변환

    // 과부화 방지
    private float lightCheckCooldown = 0.1f;
    private float lastCheckTime = 0f;

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;

        if (enemy == null)
        {
            enemy = FindAnyObjectByType<Enemy_Light>();
        }
    }

    void Update()
    {
        if (Time.time - lastCheckTime < lightCheckCooldown) return;
        lastCheckTime = Time.time;

        if (enemy == null || lightMesh == null) return;

        // 외곽선만 추출
        List<Vector3> outline = GetOutline(lightMesh);
        meshWorldPoints = new Vector3[outline.Count];

        for (int i = 0; i < outline.Count; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(outline[i]);
        }

        // 감지
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        bool isEnemyInside = IsAnyPointInLight(enemyCollider, meshWorldPoints);

        if (isEnemyInside != enemyDetected)
        {
            enemyDetected = isEnemyInside;
            enemy.SetInLight(isEnemyInside);

            Debug.Log($"Enemy가 빛에 {(isEnemyInside ? "들어옴 (InLight = true)" : "벗어남 (InLight = false)")}");

        }
        /*if (Time.time - lastCheckTime < lightCheckCooldown) return;
        lastCheckTime = Time.time;

        if (enemy == null || lightMesh == null) return;

        // 월드 좌표로 변환
        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(verts[i]);
        }

        // 적 콜라이더의 여러 포인트로 체크
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        bool isEnemyInside = IsAnyPointInLight(enemyCollider, meshWorldPoints);

        // 감지 상태가 바뀌었을 때
        if (isEnemyInside != enemyDetected)
        {
            enemyDetected = isEnemyInside;
            enemy.SetInLight(isEnemyInside);

            if (isEnemyInside)
            {
                Debug.Log("Enemy가 빛에 들어옴 (InLight = true)");
            }
            else
            {
                Debug.Log("Enemy가 빛에서 벗어남 (InLight = false)");
            }
        }*/
    }

    // 콜라이더 영역 격자로 나눠서 빛 감지
    private bool IsAnyPointInLight(Collider2D collider, Vector3[] polygon)
    {
        Bounds bounds = collider.bounds;

        int steps = 9;  // 높일수록 정밀도 증가
        float stepX = bounds.size.x / (steps - 1);
        float stepY = bounds.size.y / (steps - 1);

        for (int i = 0; i < steps; i++)
        {
            for (int j = 0; j < steps; j++)
            {
                Vector2 point = new Vector2(bounds.min.x + stepX * i, bounds.min.y + stepY * j);
                if (PointInPolygon(point, polygon))
                    return true;
            }
        }

        return false;
    }

    // 2D 포인트가 주어진 폴리곤 내부에 있는지 확인
    private bool PointInPolygon(Vector2 point, Vector3[] polygon)
    {
        int crossings = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Length];


            // 수직선 교차 여부
            if (((a.y > point.y) != (b.y > point.y)) &&
                (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y + Mathf.Epsilon) + a.x))
            {
                crossings++;
            }
        }
        // 홀수번 교차시 내부
        return (crossings % 2 == 1);
    }

    private void OnDrawGizmos()
    {
        if (meshWorldPoints == null || meshWorldPoints.Length == 0) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < meshWorldPoints.Length; i++)
        {
            Gizmos.DrawSphere(meshWorldPoints[i], 0.05f);
            Gizmos.DrawLine(meshWorldPoints[i], meshWorldPoints[(i + 1) % meshWorldPoints.Length]);
        }
    }

    private List<Vector3> GetOutline(Mesh mesh)
    {
        var edges = new Dictionary<(int, int), int>();
        var triangles = mesh.triangles;
        var verts = mesh.vertices;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int[] tri = { triangles[i], triangles[i + 1], triangles[i + 2] };

            for (int e = 0; e < 3; e++)
            {
                int a = tri[e];
                int b = tri[(e + 1) % 3];
                var edge = (Mathf.Min(a, b), Mathf.Max(a, b));

                if (edges.ContainsKey(edge)) edges[edge]++;
                else edges[edge] = 1;
            }
        }

        List<(int, int)> outlineEdges = new();
        foreach (var kvp in edges)
        {
            if (kvp.Value == 1) outlineEdges.Add(kvp.Key);
        }

        List<int> ordered = new();
        if (outlineEdges.Count > 0)
        {
            var current = outlineEdges[0];
            ordered.Add(current.Item1);
            ordered.Add(current.Item2);
            outlineEdges.RemoveAt(0);

            while (outlineEdges.Count > 0)
            {
                bool found = false;
                for (int i = 0; i < outlineEdges.Count; i++)
                {
                    var (a, b) = outlineEdges[i];
                    if (ordered[^1] == a)
                    {
                        ordered.Add(b);
                        outlineEdges.RemoveAt(i);
                        found = true;
                        break;
                    }
                    else if (ordered[^1] == b)
                    {
                        ordered.Add(a);
                        outlineEdges.RemoveAt(i);
                        found = true;
                        break;
                    }
                }
                if (!found) break;
            }
        }

        List<Vector3> outlinePoints = new();
        foreach (var i in ordered)
        {
            outlinePoints.Add(verts[i]);
        }

        return outlinePoints;
    }

}
