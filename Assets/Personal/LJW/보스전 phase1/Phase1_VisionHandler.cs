using UnityEngine;

public class Phase1_VisionHandler : MonoBehaviour
{
    [Header("����")]
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
                Debug.Log("���� ���� ��ȯ");
                switchVision.ayla.GetComponent<AylaPhase1Controller>()?.Activate();
                switchVision.ayla.SetControlEnabled(false); // ���� Ayla ������ OFF

                // 1. Ayla�� ���� ��ġ�� ���� �̵�
                if (switchVision.aylaPuzzleStartPoint != null)
                {
                    Vector3 targetPos = switchVision.aylaPuzzleStartPoint.position;

                    // transform.position ����
                    switchVision.ayla.transform.position = targetPos;
                    switchVision.ayla.SendMessage("SetFollowBasePosition", targetPos, SendMessageOptions.DontRequireReceiver);
                }

                // �ó׸ӽ� �۵� ����
                if (switchVision.CCamera != null)
                {
                    switchVision.CCamera.gameObject.SetActive(false);
                }

                // 2. ī�޶� ���� ��ġ�� �̵�
                if (switchVision.Phase1_Ayla_CameraTarget != null)
                {
                    Vector3 targetPos = switchVision.Phase1_Ayla_CameraTarget.position;
                    Vector3 cameraPos = new Vector3(targetPos.x, targetPos.y, switchVision.mainCamera.transform.position.z);
                    switchVision.mainCamera.transform.position = cameraPos;
                }

                // ī�޶� ���� �� ����ũ ��ȯ
                switchVision.mainCamera.cullingMask = switchVision.aylaPhase1ViewMask;

                // ���� ���� ��ȯ
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
                // �� �÷��̾� ���� ����

                // �ó׸ӽ� �ٽ� �ѱ�
                if (switchVision.CCamera != null)
                {
                    switchVision.CCamera.gameObject.SetActive(true);
                    switchVision.CCamera.Follow = switchVision.player.transform;
                    switchVision.CCamera.LookAt = switchVision.player.transform;
                }

                // �÷��̾� �� ����ũ
                switchVision.mainCamera.cullingMask = switchVision.playerViewMask;

                // ���� ��ȯ
                switchVision.player.SetControlEnabled(true);
                switchVision.ayla.SetControlEnabled(false);
                switchVision.ayla.isCurrentlyControlled = false;
            }
        }
    }
}
