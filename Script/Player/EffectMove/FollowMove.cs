using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMove : EffectMoveBase
{
    public Transform target;//跟随的目标
    public FollowTweenConfig tweenConfig;

    public float begin;
    public void Init(Transform target,FollowTweenConfig config)
    {
        this.target = target;
        this.tweenConfig = config;

        begin = GameTime.time;

        Dofollow();

    }

    private void Dofollow( )
    {
        if (tweenConfig.direction_type == 0)
        {
            //保持相同朝向
            this.transform.position = target.position + target.TransformDirection(tweenConfig.offset_pos);
            if (tweenConfig.freeze_rotate_z)
            {
                this.transform.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y, 0);
            }
            else
            {
                this.transform.rotation = target.rotation;
            }
        }
        else
        {
            //保持自身的朝向
            var offset_1 = target.transform.InverseTransformPoint(transform.position);
            var offset_2 = Quaternion.Inverse(target.transform.rotation) * transform.rotation;

            transform.position = target.transform.TransformPoint(offset_1);
            transform.rotation = target.transform.rotation * offset_2;
        }
    }

    public void Update()
    {
        if (target!=null)
        {
            if (tweenConfig.duration!=0&& GameTime.time - begin > tweenConfig.duration)
            {
                return;
            }
            Dofollow();
        }
        
    }
}
