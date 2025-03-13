using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class EffectPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    private readonly EffectBehaviour _EffectBehaviour = new EffectBehaviour();
    [HideInInspector]
    public EffectConfig Config;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //return Playable.Create(graph);
       var b= ScriptPlayable<EffectBehaviour>.Create(graph, _EffectBehaviour);
        var clone = b.GetBehaviour();
        clone.Config = Config;
        //clone.EndTransform = endTrans.Resolve(graph.GetResolver());
        return b;
    }

    public ClipCaps clipCaps => ClipCaps.None;
}
