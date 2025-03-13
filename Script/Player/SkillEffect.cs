using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    float begin;
    EffectConfig config;
    bool yet = false;

    int state = 0;//0´æ»î 1Ïú»Ù 
    internal int hit_effect_count = 0;

    public void Init(EffectConfig config)
    {
        state = 0;
        hit_effect_count = 0;
        this.config = config;
        begin = GameTime.time;
        yet = true;
    }

    void Update()
    {
        if (yet==true)
        {
            if (config != null && state == 0 && config.destroy_durtaion != 0)
            {
                if (GameTime.time - begin >= config.destroy_durtaion)
                {
                    DODestroy();
                }
            }
        }
      
    }


    public void DODestroy() {
        if (state == 0)
        {
            yet = false;
            state = 1;
            ResourcesManager.Instance.Destroy_Skill(config.res_path, this.gameObject);
        }
    }
}

