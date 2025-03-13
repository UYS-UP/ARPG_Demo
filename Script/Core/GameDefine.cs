using System.Collections;
using System.Collections.Generic;

using Game.Config;

using UnityEngine;

public class GameDefine 
{
    public static Vector3 _Gravity=new Vector3(0, -9.81f, 0);
    public static Transform _Camera;
    public static Vector3 _Ground_Dst = new Vector3(0, -0.02f, 0);

    public static int Ground_LayerMask;
    public static int Player_LayerMask;
    public static int Enemy_LayerMask;
    public static int Obs_Layer;
    public static int Jump_End_LayerMask;

    public static string WeaponTag="Weapon";

    public static string prop_root= "UI/Icon/Prop_Icon/";//物品图标的父目录
    public static string eq_root= "UI/Icon/EQ_Icon/";//装备图标的父目录
    public static string mat_root="UI/Icon/Mat_Icon/";//材料图标的父目录

    public static Dictionary<int, string> part_dct = new Dictionary<int, string>();
    public static string enemy_layer = "Enemy";
    public static string player_layer = "Player";
    //0:主角 1敌人
    static Dictionary<string, int> layer_dct = new Dictionary<string, int>();
    public static int GetEnemyLayer(string layer) {
        if (layer_dct.TryGetValue(layer, out var v))
        {
            return v;
        }
        else
        {
            v = LayerMask.NameToLayer(layer);
            layer_dct[layer] = v;
            return v;
        }
    }

    public static void Init() {
        _Camera = GameObject.Find("Main").transform.Find("Camera").transform;
        Obs_Layer = LayerMask.GetMask("Obs");
        Ground_LayerMask = LayerMask.GetMask("Default");
        Player_LayerMask = LayerMask.GetMask("Player");
        Enemy_LayerMask = LayerMask.GetMask("Enemy");
        Jump_End_LayerMask = LayerMask.GetMask("Default", "Obs");

        part_dct[1] = "Helmet/";
        part_dct[2] = "Cloth/";
        part_dct[3] = "Capes/";
        part_dct[4] = "Pants/";
        part_dct[5] = "Boots/";
        part_dct[6] = "Weapon/";
        part_dct[7] = "Earrings/";
        part_dct[8] = "Necklaces/";
        part_dct[9] = "Rings/";
        part_dct[10] = "Belt/";
    }

    public static string GetEQ_Icon(int part) {
        return eq_root + part_dct[part];

    }
    public static string GetAttText(int[] data) {

        if (data!=null)
        {
            return GetAttText(data[0], data[1], data[2]);
        }
        return null;    
    }

    public static string GetAttText(int id, int v, int type)
    {
        if (v > 0)
        {
            return AttNameData.Get(id).name + ":+" + (type == 0 ? v : v + "%");
        }
        else
        {
            return AttNameData.Get(id).name + ":" + (type == 0 ? v : v + "%");
        }
    }
}
