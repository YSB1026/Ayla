using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightDetection : MonoBehaviour
{
    private Mesh lightMesh;             // 광원 영역
    private Vector3[] meshWorldPoints;  // 매쉬 꼭짓점을 월드 좌표로 변환

    public Enemy_Light enemy;           // 빛 감지 대상
    private bool enemyDetected = false; // 감지 상태 저장

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
        }
    }

    // 콜라이더 영역 격자로 나눠서 빛 감지
    private bool IsAnyPointInLight(Collider2D collider, Vector3[] polygon)
    {
        Bounds bounds = collider.bounds;

        int steps = 5;  // 높일수록 정밀도 증가
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
}
