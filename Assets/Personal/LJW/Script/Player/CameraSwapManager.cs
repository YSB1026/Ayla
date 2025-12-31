using UnityEngine;

using Unity.Cinemachine;

public class CameraSwapManager : MonoBehaviour
{
    public static CameraSwapManager Instance { get; private set; }

    [Header("시네머신 카메라 설정")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera shadowCamera;

    [Header("카메라 우선순위")]
    [SerializeField] private int activeCameraPriority = 10;
    [SerializeField] private int inactiveCameraPriority = 0;

    [Header("자동 타겟 할당")]
    [SerializeField] private bool autoAssignTargets = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (autoAssignTargets)
        {
            AssignTargets();
        }

        // 시작 시 플레이어 카메라를 활성화
        SwitchToPlayer();
    }

    private void AssignTargets()
    {
        // Player 찾기
        Player player = FindObjectOfType<Player>();
        if (player != null && playerCamera != null)
        {
            playerCamera.Follow = player.transform;
            Debug.Log($"플레이어 카메라 타겟 할당: {player.gameObject.name}");
        }

        // Shadow 찾기
        Shadow shadow = FindObjectOfType<Shadow>();
        if (shadow != null && shadowCamera != null)
        {
            shadowCamera.Follow = shadow.transform;
            Debug.Log($"그림자 카메라 타겟 할당: {shadow.gameObject.name}");
        }
    }

    public void SwitchToPlayer()
    {
        if (playerCamera == null || shadowCamera == null)
        {
            Debug.LogWarning("카메라가 할당되지 않았습니다!");
            return;
        }

        playerCamera.Priority = activeCameraPriority;
        shadowCamera.Priority = inactiveCameraPriority;
        
        Debug.Log("카메라 전환: 플레이어");
    }

    public void SwitchToShadow()
    {
        if (playerCamera == null || shadowCamera == null)
        {
            Debug.LogWarning("카메라가 할당되지 않았습니다!");
            return;
        }

        playerCamera.Priority = inactiveCameraPriority;
        shadowCamera.Priority = activeCameraPriority;
        
        Debug.Log("카메라 전환: 그림자");
    }

    public void ToggleCamera(bool isPlayerActive)
    {
        if (isPlayerActive)
        {
            SwitchToPlayer();
        }
        else
        {
            SwitchToShadow();
        }
    }

    // 수동으로 카메라 설정
    public void SetCameras(CinemachineCamera player, CinemachineCamera shadow)
    {
        playerCamera = player;
        shadowCamera = shadow;
    }

    // 수동으로 타겟 설정
    public void SetTargets(Transform playerTarget, Transform shadowTarget)
    {
        if (playerCamera != null)
        {
            playerCamera.Follow = playerTarget;
        }
        if (shadowCamera != null)
        {
            shadowCamera.Follow = shadowTarget;
        }
    }
}