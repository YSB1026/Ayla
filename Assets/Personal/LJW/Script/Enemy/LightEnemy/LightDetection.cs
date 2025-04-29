using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightDetection : MonoBehaviour
{
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;

    public Enemy_Light enemy;  // ÇÑ ¸¶¸®¸¸ °¨Áö
    private bool enemyDetected = false;

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
        if (enemy == null || lightMesh == null) return;

        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(verts[i]);
        }

        Vector3 detectPoint = enemy.GetComponent<Collider2D>().bounds.center;
        bool isEnemyInside = PointInPolygon(detectPoint, meshWorldPoints);

        if (isEnemyInside != enemyDetected)
        {
            enemyDetected = isEnemyInside;
            enemy.SetInLight(isEnemyInside);

            if (isEnemyInside)
            {
                Debug.Log("Enemy°¡ ºû¿¡ µé¾î¿È (InLight = true)");
            }
            else
            {
                Debug.Log("Enemy°¡ ºû¿¡¼­ ¹þ¾î³² (InLight = false)");
            }
        }
    }

    private bool PointInPolygon(Vector2 point, Vector3[] polygon)
    {
        int crossings = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Length];

            if (((a.y > point.y) != (b.y > point.y)) &&
                (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y + Mathf.Epsilon) + a.x))
            {
                crossings++;
            }
        }
        return (crossings % 2 == 1);
    }
}
