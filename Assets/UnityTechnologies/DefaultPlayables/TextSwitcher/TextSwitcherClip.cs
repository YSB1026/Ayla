using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

[Serializable]
public class TextSwitcherClip : PlayableAsset, ITimelineClipAsset
{
    public TextSwitcherBehaviour template = new TextSwitcherBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextSwitcherBehaviour>.Create(graph, template);

        return playable;
    }
}