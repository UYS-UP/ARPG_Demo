using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonService : FSMServiceBase
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

    public override void OnDisable(PlayerState state)
    {
        base.OnDisable(state);
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);
        var config = state.stateEntity.summonConfigs;
        if (config != null&&config.Count>0) {
            for (int i = 0; i < config.Count; i++)
            {
                if (GetExcuted(i)==true)
                {
                    continue;
                }
                var item = config[i];
                if (normalizedTime>=item.trigger)
                {
                    for (int j = 0; j < item.count; j++)
                    {
                        if (UnitManager.Instance.CanCreate(item.npc_id, item.max_count) == false) {
                            SetExcuted(i);
                            break;
                        }

                        var fsm = UnitManager.Instance.CreateNPC(item.npc_id);
                        if (fsm != null)
                        {
                            //ÉèÖÃÎ»ÖÃ ½Ç¶È
                            fsm._transform.position = this.player._transform.GetOffsetPoint(UnityEngine.Random.Range(item.distance_min,
                                item.distance_max), UnityEngine.Random.Range(item.angle_min, item.angle_max));
                            fsm._transform.forward = this.player._transform.forward;

                            fsm.SetAtkTarget(this.player.GetAtkTarget());
                            if (item.state != 0)
                            {
                                fsm.ToNext(item.state);
                            }
                        }
                    }
                    SetExcuted(i);
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
