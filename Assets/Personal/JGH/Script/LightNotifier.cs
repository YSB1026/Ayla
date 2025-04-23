using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshDetector : MonoBehaviour
{
    public Transform player;
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        if (player == null || lightMesh == null) return;

        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            // 로컬 → 월드 좌표 변환
            meshWorldPoints[i] = transform.TransformPoint(verts[i]);
        }

        if (PointInPolygon(player.position, meshWorldPoints))
        {
            Debug.Log("Player가 Light Mesh 영역 안에 있음");
        }
        else
        {
            Debug.Log("Player가 Light Mesh 영역 바깥에 있음");
        }
    }

    // 다각형 안에 점이 포함되는지 확인하는 로직 (Ray Casting 방식)
    private bool PointInPolygon(Vector2 point, Vector3[] polygon)
    {
        int crossings = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Length];

            // 위아래 교차 여부 체크
            if (((a.y > point.y) != (b.y > point.y)) &&
                (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y + Mathf.Epsilon) + a.x))
            {
                crossings++;
            }
        }
        return (crossings % 2 == 1);
    }
}
