using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationService : FSMServiceBase
{
    public float normalizedTime;//当前动作播放的进度
    public string now_play_id;
    public override void Init(FSM fsm)
    {
        base.Init(fsm);
    }

    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    void Play(PlayerState state) {
        normalizedTime = 0;
        now_play_id = state.excel_config.anm_name;
        player.animator.Play(state.excel_config.anm_name,0,0);
        player.animator.Update(0);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        call_end = false;
        Play(state);
    }

    public override void OnDisable(PlayerState state)
    {
        base.OnDisable(state);
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
    }

    bool call_end;
    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);

        //判定播放的动作 是否跟我们配置的状态的动作 是一致的 
        if (!string.IsNullOrEmpty(now_play_id)) {
           var info= player.animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName(now_play_id))
            {
                //0-1 表示动作0%-100%的进度
                this.normalizedTime = info.normalizedTime;
                if (normalizedTime>1)
                {
                    //0.99  1.02
                    if (call_end==false)
                    {
                        this.normalizedTime = 1;
                        player.AnimationOnPlayEnd();
                        call_end = true;
                    }
                    else
                    {
                        if (call_end==true) { call_end = false; }
                        this.normalizedTime = normalizedTime % 1;
                    }
                }
            }
            else
            {
                this.normalizedTime = 0;
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
