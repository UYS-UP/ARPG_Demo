using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="配置/创建战斗全局配置")]
public class GlobalCombatConfig : ScriptableObject
{

    [Header("格挡时的顿帧配置")]
    public HitlagConfig block_hitlag;

    [Header("格挡时的镜头径向模糊效果")]
    public RadialBlurConfig block_radialBlur;

    [Header("格挡成功的特效")]
    public string block_effect;

    [Header("格挡时的音效")]
    public string block_audio;


    [Header("攻击到敌人的音效")]
    public string hit_enemy_audio;
    [Header("攻击到敌人的特效")]
    public string hit_enemy_effect;


    [Header("格挡时候的相机运镜配置")]
    public CameraConfig block_camer_config;

    [Header("处决时候的相机运镜配置")]
    public CameraConfig execute_camer_config;

}
