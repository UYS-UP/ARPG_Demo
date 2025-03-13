using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Game.Config;

[CreateAssetMenu(menuName ="����/����״̬����")]
public class StateScriptableObject : ScriptableObject,ISerializationCallbackReceiver
{
    //��������ʵʱ�༭������
    [Header("�񵲳ɹ�ʱ,�������Ĺҵ�")]
    public string block_camera_follow;
    [Header("��նɱʱ,��Ч�Ĺҵ�")]
    public string be_execut_effect;

    [Header("�ܻ���׷�ٵĹҵ�")]
    public string hit_obj;

    [Header("����Ч�Ĺҵ�")]
    public string block_effect_point;

    [SerializeField]
    [ListDrawerSettings(ShowIndexLabels =true,ShowPaging =false,ListElementLabelName="info")]
    public List<StateEntity> states = new List<StateEntity>();




    public void OnBeforeSerialize()
    {
       
    }

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        if (states.Count == 0) {

            Dictionary<int, PlayerStateEntity> dct = PlayerStateData.all;
            foreach (var item in dct)
            {
                var info = item.Value;
                StateEntity entity = new StateEntity();
                entity.id = info.id;
                entity.info = info.id + "_" + info.info;
                states.Add(entity);
            }
        }
        else
        {
            Dictionary<int, PlayerStateEntity> dct = PlayerStateData.all;
            if (dct.Count != states.Count)
            {
                //�����������״̬
                foreach (var item in dct)
                {
                    var info = item.Value;
                    bool add = true;
                    for (int i = 0; i < states.Count; i++)
                    {
                        if (states[i].id == info.id)
                        {
                            add = false;
                            continue;
                        }
                    }
                    //�������Ҫ����
                    if (add == true)
                    {
                        StateEntity stateEntity = new StateEntity();
                        stateEntity.id = info.id;
                        stateEntity.info = info.id + "_" + info.info;
                        states.Add(stateEntity);
                    }
                }
                List<StateEntity> remove = new List<StateEntity>();
                //ɾ���������
                foreach (var item in states)
                {
                    if (dct.ContainsKey(item.id) == false)
                    {
                        remove.Add(item);
                        //UDebug.LogError(remove.Count);
                    }
                }
                
                foreach (var item in remove)
                {
                    states.Remove(item);
                }
            }

        }
#endif
    }
}

[System.Serializable]
public class StateEntity {
    public int id;
    public string info;

    [Header("�Դ���Ϊǰ��ҡ����")]
    public bool overwrite_atk;

    [Header("����ǰҡ")]
    public float atk_before;

    [Header("������ҡ")]
    public float atk_after;

    [Header("�Ƿ�����뵥λ����ײ")]
    public bool ignor_collision;

    [Header("����λ������")]
    public List<PhysicsConfig> physicsConfig;
    //public 
    [Header("������ʾor���ؿ���")]
    public List<Obj_State> obj_States;

    [Header("��֡����")]
    public List<HitlagConfig> hitlagConfig;


    [Header("����ģ������")]
    public List<RadialBlurConfig> radialBlurConfigs;

    [Header("���м��")]
    public List<HitConfig> hitConfigs;


    [Header("��Ч����")]
    public List<EffectConfig> effectConfigs;

    [Header("�ٻ�����")]
    public List<SummonConfig> summonConfigs;

}

[System.Serializable]
public class PhysicsConfig {
    [Header("������")]
    public float trigger;
    [Header("������")]
    public float time;//������
    [Header("λ�ƾ���")]
    public Vector3 force;
    [Header("��������")]
    public AnimationCurve cure = AnimationCurve.Constant(0,1,1);
    [Header("�Ƿ��������")]
    public bool ignore_gravity;

    [Header("��⵽��λ��ͣ��")]
    public float stop_dst;
}

[System.Serializable]
public class Obj_State
{
    [Header("ע��˵��")]
    public string info;

    [Header("������")]
    public float trigger;

    [Header("��Ҫ�������������")]
    public string[] obj_id;
    [Header("�򹳼���/��֮������")]
    public bool act;

    [Header("״̬��ǰ����,�Ƿ�Ҳǿ��ִ�и�����")]
    public bool force;

    [Header("ѭ��ִ��(ѭ������)")]
    public bool loop;
}


[System.Serializable]
public class HitlagConfig
{
    [Header("������")]
    public float trigger;//��ⴥ����

    [Header("��֡֡��")]
    public int frame;//��֡֡��

    [Header("������ʽ:0ֱ�Ӵ��� 1���е�λʱ����")]
    public int triggerType;//������ʽ:0ֱ�Ӵ��� 1���е�λʱ����

    //30-50֡ ���е��� ��ִ�ж�֡
    [Header("���д�����Χ������")]
    public float trigger2;//��ⴥ����
    [Header("�Ƿ��ֵ")]
    public bool lerp;
}

[System.Serializable]
public class RadialBlurConfig {

    [Header("������")]
    public float trigger;
    [Header("������߹ر�")]
    public bool active;
    [Header("ƽ��ʱ��")]
    public float lerp;
}

[System.Serializable]
public class HitConfig {

    [Header("������")]
    public float trigger;
    [Header("������")]
    public float end;
    [Header("����:0���� 1����")]
    public int type;
    [Header("����:��� �����������·��")]
    public string begin;
    [Header("���߳���")]
    public float length;
    [Header("������Ч")]
    public string hitObj;

    [Space(20)]
    [Header("�������ĵ�")]
    public Vector3 box_center;
    [Header("���Ӵ�С")]
    public Vector3 box_size;

}
[System.Serializable]
public class CameraConfig {
    [Header("����:0��е�۵�ģ�� 1ע�ӹҵ�")]
    public int type;
    public float dst;//����
    public float y_mouse;//y��ģ������
    public float speed;//�����ٶ�
    public float time;//������ʱ��

    [Space(30)]
    public float trigger;//������ 
    //public float end;//������ 
    public string target;//ע�ӵ�Ŀ��
    public Vector3 offset;//ƫ����
}

[System.Serializable]
public class EffectConfig {
    [Header("��Դ·��")]
    public string res_path;
    [Header("������")]
    public float trigger;
    [Header("��ǰʹ�õĵ���ID")]
    public int use_prop_id;//0��û�и��������ж�
    [Header("���ɷ�ʽ:0���� 1�෢ɢ�� 2���ζ��� 3��Χ�����")]
    public int create_type;

    [Space(15)]
    [Header("����-�ο�λ��:0���� 1Ŀ��")]
    public int spawn_point_type;
    [Header("����-�ҵ�����")]
    public string spawn_hang_point;
    [Header("����-����:0�ҵ㷽�� 1����Ŀ�� 2����ǰ�� 3Ŀ��ǰ��")]
    public int rotate_type;
    [Header("����-λ��ƫ��")]
    public Vector3 position_offset;


    [Space(15)]
    [Header("����-��������(���Ҷ���)")]
    public int fan_count;
    [Header("����-����Ƕ�")]
    public int fan_angle_difference;

    [Space(15)]
    [Header("����-���ɶ�����")]
    public int rect_rows;
    [Header("����-���ɶ�����")]
    public int rect_columns;
    [Header("����-ÿ�м��")]
    public float rect_rows_spacing;
    [Header("����-ÿ�м��")]
    public float rect_columns_spacing;

    [Space(15)]
    [Header("�����Χ-�뾶(��С)")]
    public float random_radius;
    [Header("�����Χ-�뾶(���)")]
    public float random_radius_max;
    [Header("�����Χ-����")]
    public int random_count;
    [Header("�����Χ-����(���)")]
    public int random_count_max;

    [Header("�����Χ-�Ƕ�")]
    public float random_angle;
    [Header("�����Χ-�Ƕ�(���)")]
    public float random_angle_max;

    [Space(30)]
    [Header("�ƶ��ķ�ʽ:0�������ƶ� 1׷���ƶ� 2Χ����ת 3�����������ƶ� 4�����ƶ�")]
    public int move_type;
    [Header("������-�ƶ�")]
    public DirectMoveConfg directMoveConfg;

    [Header("׷��Ŀ��-�ƶ�")]
    public TrackMoveConfig trackMoveConfig;
    [Header("Χ����ת-�ƶ�")]
    public AroundMoveConfig aroundMoveConfig;
    [Header("����-�ƶ�")]
    public BezierCurveMoveConfig bezierCurveMoveConfig;

    [Header("����-�ƶ�")]
    public FollowTweenConfig followTween;

    [Space(30)]
    [Header("����ʱ����Ч")]
    public string hit_effect;
    [Header("����ʱ��Ч����:0������ 1ֻ����һ��")]
    public int hit_effect_count;
    [Header("����ʱ����Ч")]
    public string hit_audio;

    [Header("ͬһ����λ�������м���")]
    public int unit_hit_count;


    [Space(30)]
    [Header("����-���ʱ��")]
    public float destroy_durtaion;

    [Header("����-���ж��ٸ���λ")]
    public int destroy_hit_count;
}

[System.Serializable]
public class DirectMoveConfg {

    [Header("�ƶ��ٶ�")]
    public float speed;
    [Header("���ٶ�/ÿ��")]
    public float acceleration;
    [Header("����ٶ�")]
    public float maxSpeed;

    [Header("�Ƿ��Զ����ƶ�����")]
    public bool custom_direction;
    [Header("�Զ���ķ���")]
    public Vector3 direction;
    [Header("��������,0�������� 1��������")]
    public int space;

}

[System.Serializable]
public class TrackMoveConfig
{
    [Header("�ƶ��ٶ�")]
    public float speed;

    [Header("Ť��")]
    public float torque;

    [Header("���־���")]
    public float stopDistance;

    [Header("����X��")]
    public bool x_freeze;

    [Header("����Z��")]
    public bool z_freeze;
}

[System.Serializable]
public class AroundMoveConfig
{
    [Header("�ƶ��ٶ�")]
    public float speed;

    [Header("����Ŀ����ٶ�")]
    public float follow_speed;
}

[System.Serializable]
public class BezierCurveMoveConfig {

    [Header("ʱ��")]
    public float duration;

}

[System.Serializable]
public class FollowTweenConfig {

    [Header("����ʱ��")]
    public float duration;

    [Header("��������:0���ָ�Ŀ��һ�� 1��������ķ���")]
    public int direction_type;

    [Header("��Ŀ���ƫ��")]
    public Vector3 offset_pos;

    [Header("����Z�����ת")]
    public bool freeze_rotate_z;
}

[System.Serializable]
public class SummonConfig {
    [Header("������")]
    public float trigger;
    [Header("NPC_ID")]
    public int npc_id;
    [Header("����")]
    public int count;

    [Header("������������")]
    public int max_count;

    [Header("վλ��С�н�")]
    public float angle_min;
    [Header("վλ���н�")]
    public float angle_max;

    [Header("վλ��С����")]
    public float distance_min;
    [Header("վλ������")]
    public float distance_max;
    [Header("�����״̬")]
    public int state;
  
}