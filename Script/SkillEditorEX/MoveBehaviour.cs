using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveBehaviour : PlayableBehaviour
{
    public PhysicsConfig Config;

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
    }

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
    }

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }

    public override void PrepareData(Playable playable, FrameData info)
    {
        base.PrepareData(playable, info);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        //位移起点 结束点 
    }
}
