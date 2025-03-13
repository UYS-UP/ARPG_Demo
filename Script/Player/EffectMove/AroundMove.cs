using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundMove : EffectMoveBase
{
    AroundMoveConfig config;
    Transform target;
    float d;
    public void Init(AroundMoveConfig config,Transform target) {
        this.config = config;
        this.target = target;
        d = Vector3.Distance(_transform.position, target.transform.position);
    }
    void Update()
    {
        if (target != null) {
            _transform.position =Vector3.Lerp(_transform.position,target.position + (_transform.position - target.transform.position).normalized * d,config.follow_speed * GameTime.deltaTime);
            _transform.RotateAround(target.position,Vector3.up,config.speed*GameTime.deltaTime);
        }
    }
}
