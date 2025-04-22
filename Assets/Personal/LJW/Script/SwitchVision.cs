using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class SwitchVision : MonoBehaviour
{
    public CinemachineCamera Camera;
    public List<Transform> targets;
    private int currentIndex = 0;

    void Start()
    {
        if (targets.Count > 0 && Camera != null)
        {
            Camera.Follow = targets[currentIndex];
            Camera.LookAt = targets[currentIndex];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % targets.Count;

            Camera.Follow = targets[currentIndex];
            Camera.LookAt = targets[currentIndex];
        }
    }
}
