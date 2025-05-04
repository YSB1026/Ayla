using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightDetection : MonoBehaviour
{
    private Mesh lightMesh;             // ���� ����
    private Vector3[] meshWorldPoints;  // �Ž� �������� ���� ��ǥ�� ��ȯ

    public Enemy_Light enemy;           // �� ���� ���
    private bool enemyDetected = false; // ���� ���� ����

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
        }
    }

    // �ݶ��̴� ���� ���ڷ� ������ �� ����
    private bool IsAnyPointInLight(Collider2D collider, Vector3[] polygon)
    {
        Bounds bounds = collider.bounds;

        int steps = 5;  // ���ϼ��� ���е� ����
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
}
