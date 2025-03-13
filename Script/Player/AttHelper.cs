using System.Collections;
using System.Collections.Generic;
using Game.Config;
using UnityEngine;

public class AttHelper 
{
    static AttHelper instance=new AttHelper();
    public static AttHelper Instance => instance;

    /// <summary>
    /// 通过ID获取属性配置表对应的实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UnitAttEntity Creat(int id)
    {
        var a = UnitAttData.Get(id);
        if (a == null) return null;
        UnitAttEntity b = new UnitAttEntity();
        b.id = a.id;
        b.hp = a.hp;
        b.phy_atk = a.phy_atk;
        b.magic_atk = a.magic_atk;
        b.phy_def = a.phy_def;
        b.magic_def = a.magic_def;
        b.critical_hit_rate = a.critical_hit_rate;
        b.critical_hit_multiple = a.critical_hit_multiple;
        b.skill_speed = a.skill_speed;
        return b;
    }

    public UnitAttEntity Creat(UnitAttEntity a)
    {
        UnitAttEntity b = new UnitAttEntity();
        b.id = a.id;
        b.hp = a.hp;
        b.phy_atk = a.phy_atk;
        b.magic_atk = a.magic_atk;
        b.phy_def = a.phy_def;
        b.magic_def = a.magic_def;
        b.critical_hit_rate = a.critical_hit_rate;
        b.critical_hit_multiple = a.critical_hit_multiple;
        b.skill_speed = a.skill_speed;
        return b;
    }

    public int Damage(FSM atk, PlayerState state, FSM hit)
    {
        int damage = 0;
        var critical = UnityEngine.Random.Range(0, 101f) <= atk.att_crn.critical_hit_rate;
      
        //没有暴击的情况
        if (critical == false)
        {
            damage = (int)((atk.att_crn.phy_atk - hit.att_crn.phy_def + state.skill.phy_damage));
        }
        else
        {  //暴击
            damage = (int)((atk.att_crn.phy_atk - hit.att_crn.phy_def + state.skill.phy_damage) 
                * atk.att_crn.critical_hit_multiple);
        }

        return damage;
    }

}
