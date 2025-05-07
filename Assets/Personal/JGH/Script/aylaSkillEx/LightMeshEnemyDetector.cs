using HardLight2DUtil;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class LightMeshEnemyDetector : MonoBehaviour
{
    private Mesh lightMesh;
    private Vector3[] meshWorldPoints;
    
    [Header("Enemy Detection Settings")]
    [SerializeField] private float speedMultiplierInLight = 0.5f; // 빛 안에서의 속도 배율 (0.5 = 50% 감소)
    [SerializeField] private bool affectBoss = false; // 보스에게도 영향을 줄지 여부
    [SerializeField] private bool debugMode = true; // 디버그 모드
    
    // 감지된 적 객체와 원래 속도를 저장하는 사전
    private Dictionary<GameObject, EnemySpeedData> detectedEnemies = new Dictionary<GameObject, EnemySpeedData>();
    
    // 감지된 적 객체를 저장하는 리스트 (매 프레임 검색용)
    private List<GameObject> enemiesInRange = new List<GameObject>();

    // 적 속도 데이터 저장 구조체
    private struct EnemySpeedData
    {
        public float moveSpeed;
        public float runSpeed;
        public bool isInLight;

        public EnemySpeedData(float moveSpeed, float runSpeed = 0f)
        {
            this.moveSpeed = moveSpeed;
            this.runSpeed = runSpeed;
            this.isInLight = false;
        }
    }

    void Start()
    {
        lightMesh = GetComponent<MeshFilter>().mesh;
        
        // 시작할 때 씬에 있는 모든 Enemy 찾기
        FindAllEnemies();
    }

    void Update()
    {
        if (lightMesh == null) return;
        
        // 씬에 적이 없으면 다시 찾기 시도
        if (detectedEnemies.Count == 0)
        {
            if (debugMode) Debug.Log("감지된 적이 없어 다시 찾는 중...");
            FindAllEnemies();
            if (detectedEnemies.Count == 0) return; // 여전히 없으면 종료
        }

        // 메시 외곽선 계산
        List<Vector3> outline = GetOutline(lightMesh);
        meshWorldPoints = new Vector3[outline.Count];

        for (int i = 0; i < outline.Count; i++)
        {
            meshWorldPoints[i] = transform.TransformPoint(outline[i]);
        }

        // 2D 폴리곤 생성
        Vector2[] polygon2D = new Vector2[meshWorldPoints.Length];
        for (int i = 0; i < meshWorldPoints.Length; i++)
        {
            polygon2D[i] = new Vector2(meshWorldPoints[i].x, meshWorldPoints[i].y);
        }

        // 이전 프레임에서 감지된 적 리스트 초기화
        enemiesInRange.Clear();

        // 모든 적에 대해 위치 확인 (복사본 사용하여 반복 중 수정 방지)
        var enemyKeys = new List<GameObject>(detectedEnemies.Keys);
        foreach (var enemy in enemyKeys)
        {
            if (enemy == null) 
            {
                // 파괴된 객체는 사전에서 제거
                detectedEnemies.Remove(enemy);
                continue;
            }

            Vector2 enemyPos2D = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            bool isEnemyInside = PointInPolygon(enemyPos2D, polygon2D);

            // 적이 빛 영역 안에 있는지 확인
            if (isEnemyInside)
            {
                // 적이 빛 영역 안에 있을 때
                enemiesInRange.Add(enemy);
                
                var speedData = detectedEnemies[enemy];
                
                // 이미 빛 안에 있는 상태면 건너뛰기
                if (speedData.isInLight) continue;
                
                // 빛 안에 들어왔다고 표시
                speedData.isInLight = true;
                detectedEnemies[enemy] = speedData;
                
                // 보스 타입인지 확인
                Boss boss = enemy.GetComponent<Boss>();
                if (boss != null)
                {
                    if (affectBoss)
                    {
                        // 보스의 원래 속도 값 저장 (아직 저장되지 않았다면)
                        if (speedData.moveSpeed <= 0)
                        {
                            speedData.moveSpeed = boss.moveSpeed;
                            speedData.runSpeed = boss.runSpeed;
                            detectedEnemies[enemy] = speedData;
                        }
                        
                        // 보스 속도 조절
                        boss.moveSpeed *= speedMultiplierInLight;
                        boss.runSpeed *= speedMultiplierInLight;
                        
                        if (debugMode)
                        {
                            Debug.Log($"보스 '{enemy.name}' 감지됨: 이동속도 {speedData.moveSpeed} → {boss.moveSpeed}, 달리기속도 {speedData.runSpeed} → {boss.runSpeed}");
                        }
                    }
                }
                else
                {
                    // 일반 적 컴포넌트 확인 (Entity 자식 클래스 또는 기타 컴포넌트)
                    var entity = enemy.GetComponent<Entity>();
                    if (entity != null)
                    {
                        try
                        {
                            // 리플렉션으로 moveSpeed 필드 찾기 (private, protected 필드도 포함)
                            var fields = entity.GetType().GetFields(System.Reflection.BindingFlags.Public | 
                                                                   System.Reflection.BindingFlags.NonPublic | 
                                                                   System.Reflection.BindingFlags.Instance);
                            
                            bool found = false;
                            foreach (var field in fields)
                            {
                                if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower().Contains("move_speed"))
                                {
                                    // 원래 속도 저장 (아직 저장되지 않았다면)
                                    if (speedData.moveSpeed <= 0)
                                    {
                                        speedData.moveSpeed = (float)field.GetValue(entity);
                                        detectedEnemies[enemy] = speedData;
                                    }
                                    
                                    // Entity 속도 조절
                                    field.SetValue(entity, speedData.moveSpeed * speedMultiplierInLight);
                                    
                                    if (debugMode)
                                    {
                                        Debug.Log($"적 '{enemy.name}' 감지됨: 속도({field.Name}) {speedData.moveSpeed} → {(float)field.GetValue(entity)}");
                                    }
                                    
                                    found = true;
                                    break;
                                }
                            }
                            
                            // 상속받은 클래스에서도 찾지 못했으면 보스 스크립트에서 직접 확인
                            if (!found)
                            {
                                var components = enemy.GetComponents<MonoBehaviour>();
                                foreach (var comp in components)
                                {
                                    fields = comp.GetType().GetFields(System.Reflection.BindingFlags.Public | 
                                                                    System.Reflection.BindingFlags.NonPublic | 
                                                                    System.Reflection.BindingFlags.Instance);
                                    
                                    foreach (var field in fields)
                                    {
                                        if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower() == "speed")
                                        {
                                            if (field.FieldType == typeof(float))
                                            {
                                                // 원래 속도 저장
                                                if (speedData.moveSpeed <= 0)
                                                {
                                                    speedData.moveSpeed = (float)field.GetValue(comp);
                                                    detectedEnemies[enemy] = speedData;
                                                }
                                                
                                                // 속도 조절
                                                field.SetValue(comp, speedData.moveSpeed * speedMultiplierInLight);
                                                
                                                if (debugMode)
                                                {
                                                    Debug.Log($"적 '{enemy.name}' 컴포넌트({comp.GetType().Name}) 속도({field.Name}) 감지됨: {speedData.moveSpeed} → {(float)field.GetValue(comp)}");
                                                }
                                                
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    
                                    if (found) break;
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            if (debugMode)
                            {
                                Debug.LogWarning($"적 '{enemy.name}' 속도 조절 중 오류: {e.Message}");
                            }
                        }
                    }
                }
            }
            else if (detectedEnemies[enemy].isInLight)
            {
                // 적이 빛 영역 밖으로 나갔을 때
                var speedData = detectedEnemies[enemy];
                speedData.isInLight = false;
                detectedEnemies[enemy] = speedData;
                
                // 보스 타입인지 확인
                Boss boss = enemy.GetComponent<Boss>();
                if (boss != null)
                {
                    if (affectBoss && speedData.moveSpeed > 0)
                    {
                        // 보스 속도 복구
                        boss.moveSpeed = speedData.moveSpeed;
                        boss.runSpeed = speedData.runSpeed;
                        
                        if (debugMode)
                        {
                            Debug.Log($"보스 '{enemy.name}' 빛 밖으로 나감: 이동속도 복구 → {boss.moveSpeed}, 달리기속도 복구 → {boss.runSpeed}");
                        }
                    }
                }
                else
                {
                    // 일반 적 컴포넌트 확인
                    var entity = enemy.GetComponent<Entity>();
                    if (entity != null)
                    {
                        try
                        {
                            // 리플렉션으로 moveSpeed 필드 찾기
                            var fields = entity.GetType().GetFields(System.Reflection.BindingFlags.Public | 
                                                                System.Reflection.BindingFlags.NonPublic | 
                                                                System.Reflection.BindingFlags.Instance);
                            
                            bool found = false;
                            foreach (var field in fields)
                            {
                                if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower().Contains("move_speed"))
                                {
                                    if (speedData.moveSpeed > 0)
                                    {
                                        // Entity 속도 복구
                                        field.SetValue(entity, speedData.moveSpeed);
                                        
                                        if (debugMode)
                                        {
                                            Debug.Log($"적 '{enemy.name}' 빛 밖으로 나감: 속도({field.Name}) 복구 → {speedData.moveSpeed}");
                                        }
                                        
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            
                            // 상속받은 클래스에서도 찾지 못했으면 모든 컴포넌트에서 확인
                            if (!found)
                            {
                                var components = enemy.GetComponents<MonoBehaviour>();
                                foreach (var comp in components)
                                {
                                    fields = comp.GetType().GetFields(System.Reflection.BindingFlags.Public | 
                                                                    System.Reflection.BindingFlags.NonPublic | 
                                                                    System.Reflection.BindingFlags.Instance);
                                    
                                    foreach (var field in fields)
                                    {
                                        if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower() == "speed")
                                        {
                                            if (field.FieldType == typeof(float) && speedData.moveSpeed > 0)
                                            {
                                                // 속도 복구
                                                field.SetValue(comp, speedData.moveSpeed);
                                                
                                                if (debugMode)
                                                {
                                                    Debug.Log($"적 '{enemy.name}' 컴포넌트({comp.GetType().Name}) 속도({field.Name}) 복구: → {speedData.moveSpeed}");
                                                }
                                                
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    
                                    if (found) break;
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            if (debugMode)
                            {
                                Debug.LogWarning($"적 '{enemy.name}' 속도 복원 중 오류: {e.Message}");
                            }
                        }
                    }
                }
            }
        }
    }

    // 모든 적 객체 찾기 (여러 방법으로 시도)
    private void FindAllEnemies()
    {
        // 방법 1: Enemy 태그로 찾기
        GameObject[] enemiesByTag = GameObject.FindGameObjectsWithTag("Enemy");
        
        // 방법 2: Entity 컴포넌트로 찾기
        Entity[] entitiesInScene = FindObjectsOfType<Entity>();
        
        // 디버그용 로그
        if (debugMode)
        {
            Debug.Log($"'Enemy' 태그로 찾은 객체 수: {enemiesByTag.Length}");
            Debug.Log($"Entity 컴포넌트로 찾은 객체 수: {entitiesInScene.Length}");
        }
        
        // 방법 1: Enemy 태그 객체 처리
        foreach (var enemy in enemiesByTag)
        {
            ProcessEnemyObject(enemy);
        }
        
        // 방법 2: Entity 객체 처리 (Enemy 태그가 없는 경우를 위해)
        foreach (var entity in entitiesInScene)
        {
            // 이미 처리된 객체는 건너뛰기
            if (detectedEnemies.ContainsKey(entity.gameObject)) continue;
            
            // Player가 아닌 Entity를 적으로 처리
            if (!entity.CompareTag("Player"))
            {
                ProcessEnemyObject(entity.gameObject);
            }
        }
        
        if (debugMode)
        {
            Debug.Log($"총 {detectedEnemies.Count}개의 적 객체 발견됨");
            
            // 감지된 객체들의 이름 출력
            if (detectedEnemies.Count > 0)
            {
                string enemyNames = "감지된 적 객체 목록: ";
                foreach (var enemy in detectedEnemies.Keys)
                {
                    enemyNames += enemy.name + ", ";
                }
                Debug.Log(enemyNames);
            }
        }
    }
    
    // 적 객체 처리 메서드
    private void ProcessEnemyObject(GameObject enemy)
    {
        if (enemy == null) return;
        
        // 이미 처리된 적인지 확인
        if (detectedEnemies.ContainsKey(enemy)) return;
        
        // 보스 확인
        Boss boss = enemy.GetComponent<Boss>();
        if (boss != null)
        {
            if (affectBoss)
            {
                detectedEnemies[enemy] = new EnemySpeedData(boss.moveSpeed, boss.runSpeed);
                
                if (debugMode)
                {
                    Debug.Log($"보스 발견: {enemy.name}, 이동속도: {boss.moveSpeed}, 달리기속도: {boss.runSpeed}");
                }
            }
            return;
        }
        
        // 일반 적 처리 - 리플렉션으로 moveSpeed 찾기
        var entity = enemy.GetComponent<Entity>();
        if (entity != null)
        {
            try
            {
                // 리플렉션으로 moveSpeed 필드 찾기 (private, protected 필드도 포함)
                var fields = entity.GetType().GetFields(System.Reflection.BindingFlags.Public | 
                                                       System.Reflection.BindingFlags.NonPublic | 
                                                       System.Reflection.BindingFlags.Instance);
                
                foreach (var field in fields)
                {
                    if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower().Contains("move_speed"))
                    {
                        float moveSpeed = (float)field.GetValue(entity);
                        detectedEnemies[enemy] = new EnemySpeedData(moveSpeed);
                        
                        if (debugMode)
                        {
                            Debug.Log($"적 발견: {enemy.name}, 이동속도({field.Name}): {moveSpeed}");
                        }
                        return;
                    }
                }
                
                // 상속받은 클래스에서 moveSpeed 찾기
                var baseType = entity.GetType().BaseType;
                while (baseType != null)
                {
                    fields = baseType.GetFields(System.Reflection.BindingFlags.Public | 
                                               System.Reflection.BindingFlags.NonPublic | 
                                               System.Reflection.BindingFlags.Instance);
                    
                    foreach (var field in fields)
                    {
                        if (field.Name.ToLower().Contains("movespeed") || field.Name.ToLower().Contains("move_speed"))
                        {
                            float moveSpeed = (float)field.GetValue(entity);
                            detectedEnemies[enemy] = new EnemySpeedData(moveSpeed);
                            
                            if (debugMode)
                            {
                                Debug.Log($"적 발견: {enemy.name}, 상속된 이동속도({field.Name}): {moveSpeed}");
                            }
                            return;
                        }
                    }
                    
                    baseType = baseType.BaseType;
                }
                
                // moveSpeed를 찾지 못했지만 Entity이면 기본값으로 추가
                detectedEnemies[enemy] = new EnemySpeedData(2.0f); // 기본 이동 속도 값
                
                if (debugMode)
                {
                    Debug.Log($"적 발견: {enemy.name}, 이동속도 필드를 찾지 못해 기본값 2.0 사용");
                }
            }
            catch (System.Exception e)
            {
                if (debugMode)
                {
                    Debug.LogWarning($"적 속도 처리 중 오류 발생: {enemy.name}, {e.Message}");
                }
                
                // 오류 발생해도 기본값으로 추가
                detectedEnemies[enemy] = new EnemySpeedData(2.0f);
            }
        }
    }

    // 점이 폴리곤 내부에 있는지 확인
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

    // 메시 외곽선 얻기
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

    // 빛 영역 시각화
    void OnDrawGizmos()
    {
        if (meshWorldPoints == null || meshWorldPoints.Length < 2) return;

        Gizmos.color = Color.yellow; // 빛 영역은 노란색으로
        for (int i = 0; i < meshWorldPoints.Length; i++)
        {
            Vector3 a = meshWorldPoints[i];
            Vector3 b = meshWorldPoints[(i + 1) % meshWorldPoints.Length];
            Gizmos.DrawLine(a, b);
        }

        // 감지된 적 시각화
        if (enemiesInRange != null)
        {
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(enemy.transform.position, 0.15f);
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.position, enemy.transform.position);
                }
            }
        }
    }
}