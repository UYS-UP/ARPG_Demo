using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnable : MonoBehaviour
{
    [Header("延迟多长时间激活")]
    public float delay;//延迟多少秒激活
    Collider _collider;
    float enable_time;

    [Header("存活时长")]
    public float duration;
    byte state = 0;//0.等待激活 1.等待关闭 2.已关闭
    private void Awake()
    {
        _collider = this.GetComponent<Collider>();
    }

    private void OnEnable()
    {
        enable_time= GameTime.time;
        _collider.enabled = false;
        state = 0;
    }

  

    // Update is called once per frame
    void Update()
    {
        if(state==0)
        {
            if (GameTime.time - enable_time >= delay)
            {
                _collider.enabled = true;
                state = 1;
            }
        }else if (state==1)
        {
            if (GameTime.time - enable_time >= delay + duration) {
                _collider.enabled = false;
                state = 2;
            }

        }
        
    }
}
