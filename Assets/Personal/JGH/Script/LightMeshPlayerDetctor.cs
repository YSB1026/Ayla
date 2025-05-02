using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshPlayerDetector : MonoBehaviour
{
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;
    public GameObject player;
    private PlayerStatus playerState;
    private bool playerDetected = false;

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerState = player.GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (player == null || lightMesh == null || playerState == null) return;

        Vector3[] verts = lightMesh.vertices;
        meshWorldPoints = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(verts[i]); // local -> world
        }

        Vector2 playerPos2D = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2[] polygon2D = new Vector2[meshWorldPoints.Length];
        for (int i = 0; i < meshWorldPoints.Length; i++)
        {
            polygon2D[i] = new Vector2(meshWorldPoints[i].x, meshWorldPoints[i].y);
        }

        bool isPlayerInside = PointInPolygon(playerPos2D, polygon2D);

        if (isPlayerInside != playerDetected)
        {
            playerDetected = isPlayerInside;
            playerState.InLight = isPlayerInside;

            Debug.Log($"플레이어 감지 상태: {(isPlayerInside ? "InLight = true" : "InLight = false")}");
        }
    }

    // 2D 다각형 내부 판정 (레이캐스트 방식)
    private bool PointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int crossings = 0;

        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Length];

            if ((a.y > point.y) != (b.y > point.y))
            {
                float t = (point.y - a.y) / (b.y - a.y);
                float xCross = a.x + t * (b.x - a.x);
                if (point.x < xCross)
                {
                    crossings++;
                }
            }
        }

        return crossings % 2 == 1;
    }

    void OnDrawGizmos()
    {
        if (meshWorldPoints == null || meshWorldPoints.Length < 2) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < meshWorldPoints.Length; i++)
        {
            Vector3 a = meshWorldPoints[i];
            Vector3 b = meshWorldPoints[(i + 1) % meshWorldPoints.Length];
            Gizmos.DrawLine(a, b);
        }

        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.transform.position, 0.1f);
        }
    }
}
