using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEnable : MonoBehaviour
{
    [Header("�ӳٶ೤ʱ�伤��")]
    public float delay;//�ӳٶ����뼤��
    Collider _collider;
    float enable_time;

    [Header("���ʱ��")]
    public float duration;
    byte state = 0;//0.�ȴ����� 1.�ȴ��ر� 2.�ѹر�
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
