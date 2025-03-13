using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatConfig 
{
    static CombatConfig instance = new CombatConfig();
    public static CombatConfig Instance => instance;

    GlobalCombatConfig config;

    public void Init() {
        config = ResourcesManager.Instance.Load<GlobalCombatConfig>("GlobalConfig/Combat");
    }

    public GlobalCombatConfig Config() {
        return config;
    }

    //�ܻ���Ч������
    public string GetHitEffectPath(string res) { 
        if (string.IsNullOrEmpty( res)==false) {return res;
         }else { return Config().hit_enemy_effect; }
    }

}
