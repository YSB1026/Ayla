using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightDetection : MonoBehaviour
{
    public Enemy_Light enemy;           // �� ���� ���
    private bool enemyDetected = false; // ���� ���� ����

    private Mesh lightMesh;             // ���� ����
    private Vector3[] meshWorldPoints;  // �Ž� �������� ���� ��ǥ�� ��ȯ

    // ����ȭ ����
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

        // �ܰ����� ����
        List<Vector3> outline = GetOutline(lightMesh);
        meshWorldPoints = new Vector3[outline.Count];

        for (int i = 0; i < outline.Count; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(outline[i]);
        }

        // ����
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        bool isEnemyInside = IsAnyPointInLight(enemyCollider, meshWorldPoints);

        if (isEnemyInside != enemyDetected)
        {
            enemyDetected = isEnemyInside;
            enemy.SetInLight(isEnemyInside);

            Debug.Log($"Enemy�� ���� {(isEnemyInside ? "���� (InLight = true)" : "��� (InLight = false)")}");

        }
        /*if (Time.time - lastCheckTime < lightCheckCooldown) return;
        lastCheckTime = Time.time;

        if (enemy == null || lightMesh == null) return;

        // ���� ��ǥ�� ��ȯ
        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(verts[i]);
        }

        // �� �ݶ��̴��� ���� ����Ʈ�� üũ
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        bool isEnemyInside = IsAnyPointInLight(enemyCollider, meshWorldPoints);

        // ���� ���°� �ٲ���� ��
        if (isEnemyInside != enemyDetected)
        {
            enemyDetected = isEnemyInside;
            enemy.SetInLight(isEnemyInside);

            if (isEnemyInside)
            {
                Debug.Log("Enemy�� ���� ���� (InLight = true)");
            }
            else
            {
                Debug.Log("Enemy�� ������ ��� (InLight = false)");
            }
        }*/
    }

    // �ݶ��̴� ���� ���ڷ� ������ �� ����
    private bool IsAnyPointInLight(Collider2D collider, Vector3[] polygon)
    {
        Bounds bounds = collider.bounds;

        int steps = 9;  // ���ϼ��� ���е� ����
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

    // 2D ����Ʈ�� �־��� ������ ���ο� �ִ��� Ȯ��
    private bool PointInPolygon(Vector2 point, Vector3[] polygon)
    {
        int crossings = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Length];


            // ������ ���� ����
            if (((a.y > point.y) != (b.y > point.y)) &&
                (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y + Mathf.Epsilon) + a.x))
            {
                crossings++;
            }
        }
        // Ȧ���� ������ ����
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
