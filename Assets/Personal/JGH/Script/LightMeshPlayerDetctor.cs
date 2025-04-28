using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshPlayerDetector : MonoBehaviour
{
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;
    public GameObject player;
    private PlayerStatus playerState; // 플레이어 상태 스크립트 참조
    private bool playerDetected = false;

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerState = player.GetComponent<PlayerStatus>(); // PlayerState 컴포넌트를 찾는다
        }
    }

    void Update()
    {
        if (player == null || lightMesh == null || playerState == null) return;

        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(verts[i]);
        }

        bool isPlayerInside = PointInPolygon(player.transform.position, meshWorldPoints);

        if (isPlayerInside != playerDetected)
        {
            playerDetected = isPlayerInside;
            playerState.InLight = isPlayerInside;

            if (isPlayerInside)
            {
                Debug.Log("플레이어가 빛에 비춰짐 (InLight = true)");
            }
            else
            {
                Debug.Log("플레이어가 빛에서 벗어남 (InLight = false)");
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
