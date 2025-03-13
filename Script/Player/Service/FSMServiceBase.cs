using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMServiceBase 
{
    public FSM player;

    public virtual void Init(FSM fsm) {
        player = fsm;
    }

    //״̬��ʼ��ʱ�� ���������ӿ�
    public virtual void OnBegin(PlayerState state) { 
    
    }

    //ÿһ֡����
    public virtual void OnUpdate(float normalizedTime,PlayerState state) { }

    //�˳�״̬��ʱ��
    public virtual void OnEnd(PlayerState state) { }

    //״̬��Ӧ�Ķ���������ʱ��,����е���
    public virtual void OnAnimationEnd(PlayerState state) { }

    //�����²��Ŷ�����ʱ��
    public virtual void ReStart(PlayerState state) { }

    //��������ѭ�� (���ܵȼ�-���Ŷ�� ������� ���ܻ��ظ����--)

    public virtual void ReLoop(PlayerState state) { }


    public virtual void OnDisable(PlayerState state) { }

    public Dictionary<int,bool> executed=new Dictionary<int, bool> ();

    public void SetExcuted(int index) {
        executed[index] = true;
    }

    public void ReSetExcuted(int index)
    {
        executed[index] = false;
    }

    public void ReSetAllExcuted()
    {
        if (executed.Count > 0)
        {
            executed.Clear();
        }
    }

    public bool GetExcuted(int index) {
        if (executed.TryGetValue(index, out var v)) {
            return v;
        }
        return false;
    }
}
