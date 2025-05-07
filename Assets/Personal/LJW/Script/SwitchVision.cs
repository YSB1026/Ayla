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

    private Camera mainCamera; // ����Ƽ �⺻ ī�޶�

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!canSwitch) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % targets.Count;

            Transform target = targets[currentIndex];

            CCamera.Follow = targets[currentIndex];
            CCamera.LookAt = targets[currentIndex];

            // ���� ���� ���
            bool controllingAyla = targets[currentIndex] == ayla.transform;

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
