using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlurService : FSMServiceBase
{
    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        ReSetAllExcuted();
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
        ReSetAllExcuted();
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);
        if (state.stateEntity.radialBlurConfigs != null && state.stateEntity.radialBlurConfigs.Count > 0)
        {
            for (int i = 0; i < state.stateEntity.radialBlurConfigs.Count; i++)
            {
                if (GetExcuted(i) == false)
                {
                    var e = state.stateEntity.radialBlurConfigs[i];
                    if (normalizedTime >= e.trigger)
                    {
                        SetExcuted(i);
                        GameEvent.DORadialBlur?.Invoke(e);
                    }
                }

            }
        }
    }

    public override void ReLoop(PlayerState state)
    {
        base.ReLoop(state);
    }

    public override void ReStart(PlayerState state)
    {
        base.ReStart(state);
    }
}
