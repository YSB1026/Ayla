using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshDetector : MonoBehaviour
{
    public Transform player;
    public float transitionDuration = 1.0f; // 전환에 걸리는 시간 (초)
    
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;
    private bool wasInside = false; // 이전 프레임에 플레이어가 안에 있었는지 확인
    private Coroutine transitionCoroutine; // 현재 실행 중인 코루틴 참조

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

        bool isInside = PointInPolygon(player.position, meshWorldPoints);
        
        // 상태가 변경되었을 때만 전환 실행
        if (isInside != wasInside)
        {
            // 이미 실행 중인 코루틴이 있다면 중지
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            
            // 새로운 전환 코루틴 시작
            if (isInside)
            {
                // 안으로 들어왔을 때: 1f → 0f로 전환
                transitionCoroutine = StartCoroutine(TransitionThreshold(0.1f, 0f, transitionDuration));
                Debug.Log("Player가 Light Mesh 영역 안으로 들어옴");
            }
            else
            {
                // 밖으로 나갔을 때: 0f → 1f로 전환
                transitionCoroutine = StartCoroutine(TransitionThreshold(0f, 0.1f, transitionDuration));
                Debug.Log("Player가 Light Mesh 영역 밖으로 나감");
            }
            
            wasInside = isInside;
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

    // 임계값을 부드럽게 전환하는 코루틴
    IEnumerator TransitionThreshold(float startValue, float endValue, float duration)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer == null) yield break;

        Material mat = playerRenderer.material;
        if (!mat.HasProperty("_Threshold")) yield break;

        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            mat.SetFloat("_Threshold", currentValue);
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        // 마지막 값 설정으로 완료
        mat.SetFloat("_Threshold", endValue);
        transitionCoroutine = null;
    }
}