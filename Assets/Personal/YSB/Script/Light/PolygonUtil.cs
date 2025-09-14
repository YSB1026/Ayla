using System.Collections.Generic;
using UnityEngine;

public class Polygon2D
{
    public Vector3[] points3D { get; private set; }
    public Vector3[] GetWorldPoints() => points3D;

    public Polygon2D(Mesh mesh, Transform transform)
    {
        if (mesh == null)
        {
            points3D = new Vector3[0];
            return;
        }

        var outline = GetOutline(mesh);

        points3D = new Vector3[outline.Count];
        for (int i = 0; i < outline.Count; i++)
        {
            // world 좌표 변환
            points3D[i] = transform.TransformPoint(outline[i]);
        }
    }

    // Mesh 외곽선 추출
    private List<Vector3> GetOutline(Mesh mesh)
    {
        var edges = new Dictionary<(int, int), int>();
        var tris = mesh.triangles;
        var verts = mesh.vertices;

        for (int i = 0; i < tris.Length; i += 3)
        {
            int a = tris[i], b = tris[i + 1], c = tris[i + 2];
            AddEdge(edges, a, b);
            AddEdge(edges, b, c);
            AddEdge(edges, c, a);
        }

        var outlineEdges = new List<(int, int)>();
        foreach (var kvp in edges)
            if (kvp.Value == 1)
                outlineEdges.Add(kvp.Key);

        var ordered = new List<int>();
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
                    var (x, y) = outlineEdges[i];
                    if (ordered[^1] == x) { ordered.Add(y); outlineEdges.RemoveAt(i); found = true; break; }
                    if (ordered[^1] == y) { ordered.Add(x); outlineEdges.RemoveAt(i); found = true; break; }
                }
                if (!found) break;
            }
        }

        var outlinePoints = new List<Vector3>();
        foreach (var i in ordered)
            outlinePoints.Add(verts[i]);

        return outlinePoints;
    }

    private void AddEdge(Dictionary<(int, int), int> edges, int a, int b)
    {
        var key = (Mathf.Min(a, b), Mathf.Max(a, b));
        if (edges.ContainsKey(key)) edges[key]++;
        else edges[key] = 1;
    }

    // 폴리곤 안에 있는지 판정 (XZ plane 기준)
    public bool IsPointInside(Vector3 point)
    {
        int crossings = 0;

        for (int i = 0; i < points3D.Length; i++)
        {
            Vector3 a = points3D[i];
            Vector3 b = points3D[(i + 1) % points3D.Length];

            // XZ plane
            if ((a.z > point.z) != (b.z > point.z))
            {
                float t = (point.z - a.z) / (b.z - a.z);
                float xCross = a.x + t * (b.x - a.x);
                if (point.x < xCross) crossings++;
            }
        }

        return crossings % 2 == 1;
    }
}
