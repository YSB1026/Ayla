using UnityEngine;
using System.Collections.Generic;

public class SimplifiedPlayerSlowZone : MonoBehaviour
{
    [Header("Player Detection Settings")]
    [SerializeField] private float detectionRadius = 5f; // 플레이어 감지 반경
    [SerializeField] private float speedMultiplierInZone = 0.5f; // 플레이어의 속도 감소 배율 (0.5 = 50% 감소)
    [SerializeField] private LayerMask playerLayerMask; // 플레이어 레이어
    [SerializeField] private Color zoneColor = new Color(0.8f, 0.2f, 0.2f, 0.3f); // 영역 색상 (빨간색, 반투명)
    [SerializeField] private bool debugMode = true; // 디버그 모드

    // 플레이어 원래 속도를 저장하는 간단한 구조체
    private struct PlayerSpeedData
    {
        public Player playerComponent;
        public float originalMoveSpeed;
        public float originalRunSpeed;
        public bool isInZone;

        public PlayerSpeedData(Player player)
        {
            this.playerComponent = player;
            this.originalMoveSpeed = player.moveSpeed;
            this.originalRunSpeed = player.runSpeed;
            this.isInZone = false;
        }
    }

    // 플레이어 객체와 속도 데이터를 저장하는 딕셔너리
    private Dictionary<GameObject, PlayerSpeedData> detectedPlayers = new Dictionary<GameObject, PlayerSpeedData>();
    
    // 범위 내 플레이어 목록
    private List<GameObject> playersInRange = new List<GameObject>();

    void Start()
    {
        // 게임 시작 시 씬에 있는 모든 플레이어 찾기
        FindAllPlayers();
    }

    void Update()
    {
        // 씬에 플레이어가 없으면 다시 찾기
        if (detectedPlayers.Count == 0)
        {
            if (debugMode) Debug.Log("감지된 플레이어가 없어 다시 찾는 중...");
            FindAllPlayers();
            if (detectedPlayers.Count == 0) return;
        }

        // 이전 프레임 데이터 초기화
        playersInRange.Clear();

        // 플레이어 상태 확인 (복사본 사용)
        var playerKeys = new List<GameObject>(detectedPlayers.Keys);
        foreach (var playerObj in playerKeys)
        {
            if (playerObj == null)
            {
                // 파괴된 객체 제거
                detectedPlayers.Remove(playerObj);
                continue;
            }

            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(transform.position, playerObj.transform.position);
            bool isPlayerInRange = distance <= detectionRadius;

            // 플레이어가 감지 영역 안에 있는지 확인
            if (isPlayerInRange)
            {
                // 플레이어가 영역 안에 있을 때
                playersInRange.Add(playerObj);
                
                var speedData = detectedPlayers[playerObj];
                Player playerComponent = speedData.playerComponent;
                
                // 플레이어 컴포넌트가 없으면 건너뛰기
                if (playerComponent == null) continue;
                
                // 이미 영역 안에 있는 상태면 건너뛰기
                if (speedData.isInZone) continue;
                
                // 플레이어가 영역에 들어왔다고 표시
                speedData.isInZone = true;
                detectedPlayers[playerObj] = speedData;

                // 플레이어 속도 직접 변경
                playerComponent.moveSpeed *= speedMultiplierInZone;
                playerComponent.runSpeed *= speedMultiplierInZone;
                
                // isInZone 플래그 설정 (Player 클래스에 해당 필드가 있다면)
                playerComponent.isInZone = true;
                
                if (debugMode)
                {
                    Debug.Log($"플레이어 '{playerObj.name}' 이동속도 감소: {speedData.originalMoveSpeed} → {playerComponent.moveSpeed}");
                    Debug.Log($"플레이어 '{playerObj.name}' 달리기속도 감소: {speedData.originalRunSpeed} → {playerComponent.runSpeed}");
                }
            }
            else if (detectedPlayers[playerObj].isInZone)
            {
                // 플레이어가 영역 밖으로 나갔을 때
                var speedData = detectedPlayers[playerObj];
                Player playerComponent = speedData.playerComponent;
                
                if (playerComponent == null) continue;
                
                speedData.isInZone = false;
                detectedPlayers[playerObj] = speedData;

                // 원래 속도로 복원
                playerComponent.moveSpeed = speedData.originalMoveSpeed;
                playerComponent.runSpeed = speedData.originalRunSpeed;
                
                // isInZone 플래그 해제
                playerComponent.isInZone = false;
                
                if (debugMode)
                {
                    Debug.Log($"플레이어 '{playerObj.name}' 이동속도 복원: → {speedData.originalMoveSpeed}");
                    Debug.Log($"플레이어 '{playerObj.name}' 달리기속도 복원: → {speedData.originalRunSpeed}");
                }
            }
        }
    }

    // 모든 플레이어 객체 찾기
    private void FindAllPlayers()
    {
        // 방법 1: Player 태그로 플레이어 찾기
        GameObject[] playersByTag = GameObject.FindGameObjectsWithTag("Player");
        
        if (debugMode)
        {
            Debug.Log($"'Player' 태그로 찾은 객체 수: {playersByTag.Length}");
        }
        
        // 플레이어 태그로 찾은 객체 처리
        foreach (var playerObj in playersByTag)
        {
            ProcessPlayerObject(playerObj);
        }
        
        if (debugMode)
        {
            Debug.Log($"총 {detectedPlayers.Count}개의 플레이어 객체 발견됨");
            
            // 감지된 객체들의 이름 출력
            if (detectedPlayers.Count > 0)
            {
                string playerNames = "감지된 플레이어 객체 목록: ";
                foreach (var player in detectedPlayers.Keys)
                {
                    playerNames += player.name + ", ";
                }
                Debug.Log(playerNames);
            }
        }
    }
    
    // 플레이어 객체 처리
    private void ProcessPlayerObject(GameObject playerObj)
    {
        if (playerObj == null) return;
        
        // 이미 처리된 플레이어인지 확인
        if (detectedPlayers.ContainsKey(playerObj)) return;
        
        try
        {
            // Player 컴포넌트 가져오기
            Player playerComponent = playerObj.GetComponent<Player>();
            
            if (playerComponent != null)
            {
                // Player 컴포넌트를 찾은 경우 데이터 저장
                detectedPlayers[playerObj] = new PlayerSpeedData(playerComponent);
                
                if (debugMode)
                {
                    Debug.Log($"플레이어 '{playerObj.name}' 이동속도: {playerComponent.moveSpeed}");
                    Debug.Log($"플레이어 '{playerObj.name}' 달리기속도: {playerComponent.runSpeed}");
                }
            }
            else if (playerObj.CompareTag("Player"))
            {
                // Player 컴포넌트는 없지만 Player 태그가 있는 경우 경고
                if (debugMode)
                {
                    Debug.LogWarning($"'{playerObj.name}'에 Player 태그는 있지만 Player 컴포넌트가 없습니다.");
                }
            }
        }
        catch (System.Exception e)
        {
            if (debugMode)
            {
                Debug.LogWarning($"플레이어 '{playerObj.name}' 처리 중 오류: {e.Message}");
            }
        }
    }

    // 느린 영역 시각화
    void OnDrawGizmos()
    {
        // 감지 반경 그리기
        Gizmos.color = zoneColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // 투명한 원형 영역 그리기
        #if UNITY_EDITOR
        UnityEditor.Handles.color = zoneColor;
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, detectionRadius);
        #endif

        // 감지된 플레이어 시각화
        if (playersInRange != null)
        {
            foreach (var player in playersInRange)
            {
                if (player != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(player.transform.position, 0.2f);
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, player.transform.position);
                }
            }
        }
    }
}