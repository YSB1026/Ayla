using System.Collections.Generic;
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

    public Camera mainCamera; // ����Ƽ �⺻ ī�޶�

    public Transform aylaPuzzleStartPoint;     // Ayla�� �̵��� ��ġ
    public Transform Phase1_Ayla_CameraTarget; // �ν����Ϳ��� ������ ���� ��ġ

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

            // ���� ���� ���
            bool controllingAyla = target == ayla.transform;

            // ���� ����
            ayla.isCurrentlyControlled = controllingAyla;
            ayla.SetControlEnabled(controllingAyla);
            player.SetControlEnabled(!controllingAyla);

            // ī�޶� ���̾� ����ũ ������ Main Camera���� ����
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
