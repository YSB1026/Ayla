using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BubbleFollowClip : PlayableAsset
{
    public ExposedReference<Transform> target;
    public ExposedReference<RectTransform>[] balloonUIs;
    public Vector3 offset;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BubbleFollow>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.target = target.Resolve(graph.GetResolver());

        if (balloonUIs != null)
        {
            int count = balloonUIs.Length;
            behaviour.balloonUIs = new RectTransform[count];
            for (int i = 0; i < count; i++)
            {
                behaviour.balloonUIs[i] = balloonUIs[i].Resolve(graph.GetResolver());
            }
        }

        behaviour.offset = offset;
        return playable;
    }
}
