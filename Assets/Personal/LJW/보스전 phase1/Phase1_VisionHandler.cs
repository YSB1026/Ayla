using UnityEngine;

public class Phase1_VisionHandler : MonoBehaviour
{
    [Header("참조")]
    public SwitchVision switchVision;

    private bool inPuzzleView = false;

    void Update()
    {
        if (!switchVision.isPhase1)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inPuzzleView = !inPuzzleView;

            if (inPuzzleView)
            {
                Debug.Log("퍼즐 시점 전환");
                switchVision.ayla.GetComponent<AylaPhase1Controller>()?.Activate();
                switchVision.ayla.SetControlEnabled(false); // 기존 Ayla 움직임 OFF

                // 1. Ayla를 퍼즐 위치로 순간 이동
                if (switchVision.aylaPuzzleStartPoint != null)
                {
                    Vector3 targetPos = switchVision.aylaPuzzleStartPoint.position;

                    // transform.position 설정
                    switchVision.ayla.transform.position = targetPos;
                    switchVision.ayla.SendMessage("SetFollowBasePosition", targetPos, SendMessageOptions.DontRequireReceiver);
                }

                // 시네머신 작동 중지
                if (switchVision.CCamera != null)
                {
                    switchVision.CCamera.gameObject.SetActive(false);
                }

                // 2. 카메라 퍼즐 위치로 이동
                if (switchVision.Phase1_Ayla_CameraTarget != null)
                {
                    Vector3 targetPos = switchVision.Phase1_Ayla_CameraTarget.position;
                    Vector3 cameraPos = new Vector3(targetPos.x, targetPos.y, switchVision.mainCamera.transform.position.z);
                    switchVision.mainCamera.transform.position = cameraPos;
                }

                // 카메라 퍼즐 뷰 마스크 전환
                switchVision.mainCamera.cullingMask = switchVision.aylaPhase1ViewMask;

                // 조작 권한 전환
                switchVision.ayla.SetControlEnabled(true);
                switchVision.ayla.isCurrentlyControlled = true;
                switchVision.player.SetControlEnabled(false);

                int uiLayer = LayerMask.NameToLayer("UI");
                switchVision.mainCamera.cullingMask =
                    switchVision.playerViewMask
                    | (1 << uiLayer);
            }
            else
            {
                // ◀ 플레이어 시점 복귀

                // 시네머신 다시 켜기
                if (switchVision.CCamera != null)
                {
                    switchVision.CCamera.gameObject.SetActive(true);
                    switchVision.CCamera.Follow = switchVision.player.transform;
                    switchVision.CCamera.LookAt = switchVision.player.transform;
                }

                // 플레이어 뷰 마스크
                switchVision.mainCamera.cullingMask = switchVision.playerViewMask;

                // 조작 전환
                switchVision.player.SetControlEnabled(true);
                switchVision.ayla.SetControlEnabled(false);
                switchVision.ayla.isCurrentlyControlled = false;
            }
        }
    }
}
