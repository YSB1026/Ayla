using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[TrackColor(0.139f, 0.44f, 0.34f)]
[TrackClipType(typeof(TextSwitcherClip))]
[TrackBindingType(typeof(Text))]
public class TextSwitcherTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TextSwitcherMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        Text trackBinding = director.GetGenericBinding(this) as Text;
        if (trackBinding == null)
            return;

        var serializedObject = new SerializedObject(trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;

            driver.AddFromName<Text>(trackBinding.gameObject, iterator.propertyPath);
        }
#endif
        base.GatherProperties(director, driver);
    }
}
