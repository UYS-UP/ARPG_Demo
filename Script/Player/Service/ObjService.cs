using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjService : FSMServiceBase
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

        //
        var os = state.stateEntity.obj_States;
        if (os != null) {

            for (int i = 0; i < os.Count; i++)
            {
                var item = os[i];
                //ǿ��ִ�и������� �Ƿ�δִ�й�
                if (item.force && GetExcuted(i) == false)
                {
                    DO(item);
                }
            }
        }
        ReSetAllExcuted();
    }

    private void DO(Obj_State item)
    {
        if (item.obj_id != null)
        {
            foreach (var o_id in item.obj_id)
            {
                var obj = player.GetHangPoint(o_id);
                if (obj != null)
                {
                    obj.SetActive(item.act);
                }
            }
        }
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);

        var os = state.stateEntity.obj_States;
        if (os != null)
        {
            for (int i = 0; i < os.Count; i++)
            {
                var item = os[i];
                //ǿ��ִ�и������� �Ƿ�δִ�й�
                if (normalizedTime>=item.trigger && GetExcuted(i) == false)
                {
                    SetExcuted(i);
                    DO(item);
                }
            }
        }
    }

    //���ܵ�������ʱ�� ����20-50������Ҫ����ѭ������������
    public override void ReLoop(PlayerState state)
    {
        base.ReLoop(state);
        Item_ResetExcuted(state);
    }

    public override void ReStart(PlayerState state)
    {
        base.ReStart(state);
        Item_ResetExcuted(state);
    }

    private void Item_ResetExcuted(PlayerState state)
    {
        var os = state.stateEntity.obj_States;
        if (os != null)
        {
            for (int i = 0; i < os.Count; i++)
            {
                var item = os[i];
                //ǿ��ִ�и������� �Ƿ�δִ�й�
                if (item.loop)
                {
                    ReSetExcuted(i);
                }
            }
        }
    }

  
}
