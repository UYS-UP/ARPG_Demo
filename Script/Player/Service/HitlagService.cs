using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitlagService : FSMServiceBase
{
    public override void Init(FSM fsm)
    {
        base.Init(fsm);
    }

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
        if (state.stateEntity.hitlagConfig!=null&& state.stateEntity.hitlagConfig.Count>0)
        {
            for (int i = 0; i < state.stateEntity.hitlagConfig.Count; i++)
            {
                var x = state.stateEntity.hitlagConfig[i];
                if (x.triggerType==0&&normalizedTime>=x.trigger&&GetExcuted(i)==false)
                {
                    SetExcuted(i);
                    GameEvent.DOHitlag?.Invoke(x.frame, x.lerp);
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

    public void DOHitlag_OnAttack(float normalizedTime, PlayerState state) {
        if (state.stateEntity.hitlagConfig != null && state.stateEntity.hitlagConfig.Count > 0)
        {
            for (int i = 0; i < state.stateEntity.hitlagConfig.Count; i++)
            {
                var x = state.stateEntity.hitlagConfig[i];
                if (x.triggerType == 1 && normalizedTime >= x.trigger && normalizedTime <= x.trigger2)
                {
                    if (GetExcuted(i) == false) {
                        SetExcuted(i);
                        GameEvent.DOHitlag?.Invoke(x.frame, x.lerp);
                    }
                }
            }
        }
    }
}
