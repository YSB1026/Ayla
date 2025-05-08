using UnityEngine;
using UnityEngine.Playables;

public class BubbleFollow : PlayableBehaviour
{
    public Transform target; // 단일 타겟
    public RectTransform[] balloonUIs; // 여러 말풍선 UI들
    public Vector3 offset = new Vector3(3, 2, 0); // 위치 보정

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
