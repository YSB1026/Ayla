/*using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class SwitchVision : MonoBehaviour
{
    public Ayla ayla;
    public Player player;

    public CinemachineCamera CCamera;
    public List<Transform> targets;
    private int currentIndex = 0;

    [Header("LayerMasks")]
    public LayerMask playerViewMask;
    public LayerMask aylaDefaultViewMask;
    public LayerMask aylaPhase1ViewMask;

    public bool isPhase1 = false;
    public bool controllingAyla = false;
    public bool canSwitch = true;

    public Camera mainCamera; // 유니티 기본 카메라

    public Transform aylaPuzzleStartPoint;     // Ayla가 이동할 위치
    public Transform Phase1_Ayla_CameraTarget; // 인스펙터에서 연결할 퍼즐 위치

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!canSwitch || isPhase1) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % targets.Count;

            Transform target = targets[currentIndex];

            CCamera.Follow = target;
            CCamera.LookAt = target;

            // 조작 상태 토글
            bool controllingAyla = target == ayla.transform;

            // 조작 제어
            ayla.isCurrentlyControlled = controllingAyla;
            ayla.SetControlEnabled(controllingAyla);
            player.SetControlEnabled(!controllingAyla);

            // 카메라 레이어 마스크 설정은 Main Camera에만 적용
            if (mainCamera != null)
            {
                if (controllingAyla)
                    mainCamera.cullingMask = isPhase1 ? aylaPhase1ViewMask : aylaDefaultViewMask;
                else
                    mainCamera.cullingMask = playerViewMask;
            }
        }
    }
}
*/