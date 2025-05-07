using HardLight2DUtil;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshPlayerDetector : MonoBehaviour
{
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;
    public GameObject player;
    private PlayerStatus playerState;
    [SerializeField] private bool playerDetected = false;

    // 그림자 상호작용을 위한 변수들
    public List<ShadowPuller2D> shadowPullers = new List<ShadowPuller2D>();
    private bool shadowInLight = false;

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerState = player.GetComponent<PlayerStatus>();
        }
        
        // 모든 ShadowPuller2D 찾기
        FindAllShadowPullers();
    }

    void FindAllShadowPullers()
    {
        ShadowPuller2D[] pullers = FindObjectsOfType<ShadowPuller2D>();
        shadowPullers.AddRange(pullers);
    }

    void Update()
    {
        if (lightMesh == null) return;

        List<Vector3> outline = GetOutline(lightMesh);
        meshWorldPoints = new Vector3[outline.Count];

        for (int i = 0; i < outline.Count; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(outline[i]);
        }

        // 플레이어 감지 (기존 코드)
        if (player != null && playerState != null)
        {
            Vector2 playerPos2D = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2[] polygon2D = ConvertToVector2Array(meshWorldPoints);

            bool isPlayerInside = PointInPolygon(playerPos2D, polygon2D);

            if (isPlayerInside != playerDetected)
            {
                playerDetected = isPlayerInside;
                playerState.InLight = isPlayerInside;
                Debug.Log($"플레이어 감지 상태: {(isPlayerInside ? "InLight = true" : "InLight = false")}");
            }
        }

        // 그림자 포인트 감지 및 처리
        foreach (ShadowPuller2D shadowPuller in shadowPullers)
        {
            if (shadowPuller != null && shadowPuller.shadowPoint != null)
            {
                Vector2 shadowPointPos = new Vector2(shadowPuller.shadowPoint.position.x, shadowPuller.shadowPoint.position.y);
                Vector2[] polygon2D = ConvertToVector2Array(meshWorldPoints);

                bool isShadowInLight = PointInPolygon(shadowPointPos, polygon2D);
                
                // 그림자 포인트가 빛 안에 있음을 ShadowPuller에게 알림
                shadowPuller.SetInLightStatus(isShadowInLight);
            }
        }
    }

    private Vector2[] ConvertToVector2Array(Vector3[] vector3Array)
    {
        Vector2[] vector2Array = new Vector2[vector3Array.Length];
        for (int i = 0; i < vector3Array.Length; i++)
        {
            vector2Array[i] = new Vector2(vector3Array[i].x, vector3Array[i].y);
        }
        return vector2Array;
    }

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
        
        // 그림자 포인트 시각화
        foreach (ShadowPuller2D shadowPuller in shadowPullers)
        {
            if (shadowPuller != null && shadowPuller.shadowPoint != null)
            {
                Gizmos.color = shadowPuller.IsInLight() ? Color.yellow : Color.green;
                Gizmos.DrawSphere(shadowPuller.shadowPoint.position, 0.15f);
            }
        }
    }
}