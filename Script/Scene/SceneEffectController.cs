using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffectController : MonoBehaviour
{
    public FSM boss;
    public GameObject target;
    public int state;//0未开启 1生效 2失效

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            if (boss != null)
            {
                if (boss.att_crn.hp <= boss.att_base.hp * 0.8f)
                {
                    target.gameObject.SetActive(true);
                    state = 1;
                }
            }
        }
        else if (state == 1)
        {
            if (boss.att_crn.hp <= boss.att_base.hp * 0.2f)
            {
                target.gameObject.SetActive(false);
                state = 2;
            }
        }
    }
}
