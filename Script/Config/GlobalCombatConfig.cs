using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="����/����ս��ȫ������")]
public class GlobalCombatConfig : ScriptableObject
{

    [Header("��ʱ�Ķ�֡����")]
    public HitlagConfig block_hitlag;

    [Header("��ʱ�ľ�ͷ����ģ��Ч��")]
    public RadialBlurConfig block_radialBlur;

    [Header("�񵲳ɹ�����Ч")]
    public string block_effect;

    [Header("��ʱ����Ч")]
    public string block_audio;


    [Header("���������˵���Ч")]
    public string hit_enemy_audio;
    [Header("���������˵���Ч")]
    public string hit_enemy_effect;


    [Header("��ʱ�������˾�����")]
    public CameraConfig block_camer_config;

    [Header("����ʱ�������˾�����")]
    public CameraConfig execute_camer_config;

}
