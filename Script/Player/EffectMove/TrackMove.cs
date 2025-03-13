using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMove : EffectMoveBase
{
    TrackMoveConfig config;
    Transform target;
    public void Init(TrackMoveConfig config,GameObject target) { 
        this.config = config;
        this.target = target.transform;
    }

    private void Update()
    {
       if (this.target != null)
        {
            if (config.torque == 0) {
                _transform.LookTarget(target);
            }
            else
            {
                Vector3 direction = (target.position - _transform.position).normalized;
                _transform.rotation = Quaternion.Lerp(_transform.rotation,
                    Quaternion.LookRotation(direction), config.torque * GameTime.deltaTime);

                if (config.x_freeze||config.z_freeze) {
                    var v1 = _transform.eulerAngles;
                    if (config.x_freeze)
                    {
                        v1.x = 0;
                    }

                    if (config.z_freeze)
                    {
                        v1.z = 0;
                    }
                    _transform.eulerAngles = v1;
                }
            }

            if (Vector3.Distance(_transform.position,target.position)>config.stopDistance) {
                _transform.Translate(_transform.forward * config.speed * GameTime.deltaTime, Space.World);
            }
        }
    }
}
