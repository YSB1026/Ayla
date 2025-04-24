using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class SwitchVision : MonoBehaviour
{
    public Ayla ayla;
    public Player player;

    public CinemachineCamera Camera;
    public List<Transform> targets;
    private int currentIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % targets.Count;

            Camera.Follow = targets[currentIndex];
            Camera.LookAt = targets[currentIndex];

            // 조작 상태 토글
            bool controllingAyla = targets[currentIndex] == ayla.transform;

            ayla.isCurrentlyControlled = controllingAyla;
            ayla.SetControlEnabled(controllingAyla);
            player.SetControlEnabled(!controllingAyla);
        }
    }
}
