using System;
using System.Collections;
using System.Collections.Generic;

using Pathfinding;

using UnityEngine;

public class NavigationService : FSMServiceBase
{
    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
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
        OnMove();
    }

    List<Vector3> _path;
    public int state;//0���� 1����Ѱ��·�� 2����Ѱ·���
    Vector3 _point;//Ѱ·��Ŀ�ĵ�
    public int currentWaypoint;//·������(��ǰ��)
    public Vector3 _pathLast;//·�������һ��λ�� 
    public Action _success;//Ѱ·�ɹ���Ҫ�������¼�

    public void Move(Vector3 position,Action success) {
        if (state==0||(state==1&&position!=_point))
        {
            _point= position;
            this._success = success;
            state = 1;
            var p=  NavHelper.Instance.GetWalkNearestPosition(position);
            player.seeker.StartPath(player._transform.position, p, OnPathComplete);
        }
        
    }

    private void OnPathComplete(Path path)
    {
        if (state==1)
        {
            state = 2;

            if (path.error == false)
            {
                _path = path.vectorPath;
                if (_path.Count <= 1)
                {
                    currentWaypoint = 0;
                }
                else {
                    currentWaypoint = 1;
                }
                _pathLast = _path[_path.Count-1];
                this.player._transform.LookTarget(_path[currentWaypoint]);
                this._success?.Invoke();
            }
            else {
                Stop();
            }
        }
    }

    private void OnMove()
    {
        if (_path == null) { return; }
        //ÿ֡����һ��·������ƶ�
        //�ж��Ƿ�ӽ�·����,�ǵĻ� ���µ���һ��·��

        if (currentWaypoint>=_path.Count)
        {
            Stop();
            return;
        }
        else
        {
            var next_point= _path[currentWaypoint];
            Vector3 dir = next_point - player._transform.position;
            dir.y = 0;
            this.player.Move(dir * player.GetMoveSpeed(),false);

            if (Vector3.Distance(this.player._transform.position,next_point)<=0.5f)
            {
                if (currentWaypoint>=_path.Count-1)
                {
                    Stop();
                }
                else
                {
                    currentWaypoint += 1;
                    this.player._transform.LookTarget(_path[currentWaypoint]);
                }
            }

        }
    }


    public void Stop() {
        _path = null;
        state = 0;
    }


    public override void ReLoop(PlayerState state)
    {
        base.ReLoop(state);
    }

    public override void ReStart(PlayerState state)
    {
        base.ReStart(state);
    }

    internal bool IsEnd()
    {
        return _path == null;
    }
}
