using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMove : EffectMoveBase
{
    public Vector3 move_direction;
    public float move_speed;

    public DirectMoveConfg config;
   
    public void Init(DirectMoveConfg config,float speed,Vector3 move_direction)
    {
       this.config = config;
        this.move_speed = speed;
        this.move_direction = move_direction;
        yet = true;
    }

    private void Update()
    {
        if (yet==false) { return; }
        if (config != null) { 
        
            //处理加速度 
            if (config.acceleration!=0) {

                move_speed += config.acceleration*GameTime.deltaTime;
                if (config.maxSpeed != 0) { 
                    if(config.acceleration > 0) {
                        if (move_speed> config.maxSpeed)
                        {
                            move_speed = config.maxSpeed;
                        }
                        else if (move_speed<config.maxSpeed)
                        {
                            move_speed = config.maxSpeed;
                        }
                    }
                }
               
            }

            if (config.custom_direction)
            {
                if (config.space == 0)
                {
                    _transform.position += config.direction * move_speed * GameTime.deltaTime;
                }
                else {

                    _transform.position += _transform.TransformDirection(config.direction * move_speed * GameTime.deltaTime);
                }
            }
            else
            {
                _transform.forward = move_direction;
                _transform.Translate(Vector3.forward*move_speed*GameTime.deltaTime);
            }
        }
    }


}
