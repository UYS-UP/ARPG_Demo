using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MovePlayableAsset : PlayableAsset, ITimelineClipAsset
{
    private readonly MoveBehaviour _Behaviour = new MoveBehaviour();
    public PhysicsConfig Config;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //return Playable.Create(graph);
        var b = ScriptPlayable<MoveBehaviour>.Create(graph, _Behaviour);
        var clone = b.GetBehaviour();
        clone.Config = Config;
        //clone.EndTransform = endTrans.Resolve(graph.GetResolver());
        return b;
    }

    public ClipCaps clipCaps => ClipCaps.None;
}
