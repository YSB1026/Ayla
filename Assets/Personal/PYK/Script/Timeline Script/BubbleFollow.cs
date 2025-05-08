using UnityEngine;
using UnityEngine.Playables;

public class BubbleFollow : PlayableBehaviour
{
    public Transform target; // ���� Ÿ��
    public RectTransform[] balloonUIs; // ���� ��ǳ�� UI��
    public Vector3 offset = new Vector3(3, 2, 0); // ��ġ ����

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (target == null || balloonUIs == null)
            return;

        foreach (var balloon in balloonUIs)
        {
            if (balloon != null)
                balloon.position = target.position + offset;
        }
    }
}
