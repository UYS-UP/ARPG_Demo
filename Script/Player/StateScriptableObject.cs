using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Game.Config;

[CreateAssetMenu(menuName ="配置/创建状态配置")]
public class StateScriptableObject : ScriptableObject,ISerializationCallbackReceiver
{
    //承载所有实时编辑的数据
    [Header("格挡成功时,相机跟随的挂点")]
    public string block_camera_follow;
    [Header("被斩杀时,特效的挂点")]
    public string be_execut_effect;

    [Header("受击、追踪的挂点")]
    public string hit_obj;

    [Header("格挡特效的挂点")]
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
                //遍历表格所有状态
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
                    //如果是需要增加
                    if (add == true)
                    {
                        StateEntity stateEntity = new StateEntity();
                        stateEntity.id = info.id;
                        stateEntity.info = info.id + "_" + info.info;
                        states.Add(stateEntity);
                    }
                }
                List<StateEntity> remove = new List<StateEntity>();
                //删除掉多余的
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

    [Header("以此作为前后摇参数")]
    public bool overwrite_atk;

    [Header("攻击前摇")]
    public float atk_before;

    [Header("攻击后摇")]
    public float atk_after;

    [Header("是否忽略与单位的碰撞")]
    public bool ignor_collision;

    [Header("物理位移配置")]
    public List<PhysicsConfig> physicsConfig;
    //public 
    [Header("物体显示or隐藏控制")]
    public List<Obj_State> obj_States;

    [Header("顿帧配置")]
    public List<HitlagConfig> hitlagConfig;


    [Header("径向模糊配置")]
    public List<RadialBlurConfig> radialBlurConfigs;

    [Header("命中检测")]
    public List<HitConfig> hitConfigs;


    [Header("特效配置")]
    public List<EffectConfig> effectConfigs;

    [Header("召唤配置")]
    public List<SummonConfig> summonConfigs;

}

[System.Serializable]
public class PhysicsConfig {
    [Header("触发点")]
    public float trigger;
    [Header("结束点")]
    public float time;//结束点
    [Header("位移距离")]
    public Vector3 force;
    [Header("曲线配置")]
    public AnimationCurve cure = AnimationCurve.Constant(0,1,1);
    [Header("是否忽略重力")]
    public bool ignore_gravity;

    [Header("检测到单位后停下")]
    public float stop_dst;
}

[System.Serializable]
public class Obj_State
{
    [Header("注释说明")]
    public string info;

    [Header("触发点")]
    public float trigger;

    [Header("需要操作的物体对象")]
    public string[] obj_id;
    [Header("打钩激活/反之则隐藏")]
    public bool act;

    [Header("状态提前结束,是否也强制执行该配置")]
    public bool force;

    [Header("循环执行(循环动作)")]
    public bool loop;
}


[System.Serializable]
public class HitlagConfig
{
    [Header("触发点")]
    public float trigger;//检测触发点

    [Header("顿帧帧数")]
    public int frame;//顿帧帧数

    [Header("触发方式:0直接触发 1命中单位时触发")]
    public int triggerType;//触发方式:0直接触发 1命中单位时触发

    //30-50帧 命中敌人 才执行顿帧
    [Header("命中触发范围结束点")]
    public float trigger2;//检测触发点
    [Header("是否插值")]
    public bool lerp;
}

[System.Serializable]
public class RadialBlurConfig {

    [Header("触发点")]
    public float trigger;
    [Header("激活或者关闭")]
    public bool active;
    [Header("平滑时间")]
    public float lerp;
}

[System.Serializable]
public class HitConfig {

    [Header("触发点")]
    public float trigger;
    [Header("结束点")]
    public float end;
    [Header("类型:0射线 1盒子")]
    public int type;
    [Header("射线:起点 配置子物体的路径")]
    public string begin;
    [Header("射线长度")]
    public float length;
    [Header("命中特效")]
    public string hitObj;

    [Space(20)]
    [Header("盒子中心点")]
    public Vector3 box_center;
    [Header("盒子大小")]
    public Vector3 box_size;

}
[System.Serializable]
public class CameraConfig {
    [Header("类型:0机械臂的模拟 1注视挂点")]
    public int type;
    public float dst;//距离
    public float y_mouse;//y轴模拟输入
    public float speed;//跟随速度
    public float time;//持续的时间

    [Space(30)]
    public float trigger;//触发点 
    //public float end;//结束点 
    public string target;//注视的目标
    public Vector3 offset;//偏移量
}

[System.Serializable]
public class EffectConfig {
    [Header("资源路径")]
    public string res_path;
    [Header("触发点")]
    public float trigger;
    [Header("当前使用的道具ID")]
    public int use_prop_id;//0则没有该条件的判断
    [Header("生成方式:0单发 1多发散射 2矩形队列 3范围内随机")]
    public int create_type;

    [Space(15)]
    [Header("出生-参考位置:0自身 1目标")]
    public int spawn_point_type;
    [Header("出生-挂点名称")]
    public string spawn_hang_point;
    [Header("出生-朝向:0挂点方向 1朝向目标 2自身前方 3目标前方")]
    public int rotate_type;
    [Header("出生-位置偏差")]
    public Vector3 position_offset;


    [Space(15)]
    [Header("扇形-生成数量(左右多少)")]
    public int fan_count;
    [Header("扇形-间隔角度")]
    public int fan_angle_difference;

    [Space(15)]
    [Header("矩形-生成多少行")]
    public int rect_rows;
    [Header("矩形-生成多少列")]
    public int rect_columns;
    [Header("矩形-每行间隔")]
    public float rect_rows_spacing;
    [Header("矩形-每列间隔")]
    public float rect_columns_spacing;

    [Space(15)]
    [Header("随机范围-半径(最小)")]
    public float random_radius;
    [Header("随机范围-半径(最大)")]
    public float random_radius_max;
    [Header("随机范围-数量")]
    public int random_count;
    [Header("随机范围-数量(最大)")]
    public int random_count_max;

    [Header("随机范围-角度")]
    public float random_angle;
    [Header("随机范围-角度(最大)")]
    public float random_angle_max;

    [Space(30)]
    [Header("移动的方式:0按方向移动 1追踪移动 2围绕旋转 3贝塞尔曲线移动 4跟随移动")]
    public int move_type;
    [Header("按方向-移动")]
    public DirectMoveConfg directMoveConfg;

    [Header("追踪目标-移动")]
    public TrackMoveConfig trackMoveConfig;
    [Header("围绕旋转-移动")]
    public AroundMoveConfig aroundMoveConfig;
    [Header("曲线-移动")]
    public BezierCurveMoveConfig bezierCurveMoveConfig;

    [Header("跟随-移动")]
    public FollowTweenConfig followTween;

    [Space(30)]
    [Header("命中时的特效")]
    public string hit_effect;
    [Header("命中时特效数量:0无限制 1只创建一次")]
    public int hit_effect_count;
    [Header("命中时的音效")]
    public string hit_audio;

    [Header("同一个单位允许被命中几次")]
    public int unit_hit_count;


    [Space(30)]
    [Header("销毁-存活时间")]
    public float destroy_durtaion;

    [Header("销毁-命中多少个单位")]
    public int destroy_hit_count;
}

[System.Serializable]
public class DirectMoveConfg {

    [Header("移动速度")]
    public float speed;
    [Header("加速度/每秒")]
    public float acceleration;
    [Header("最大速度")]
    public float maxSpeed;

    [Header("是否自定义移动方向")]
    public bool custom_direction;
    [Header("自定义的方向")]
    public Vector3 direction;
    [Header("坐标类型,0世界坐标 1本地坐标")]
    public int space;

}

[System.Serializable]
public class TrackMoveConfig
{
    [Header("移动速度")]
    public float speed;

    [Header("扭矩")]
    public float torque;

    [Header("保持距离")]
    public float stopDistance;

    [Header("冻结X轴")]
    public bool x_freeze;

    [Header("冻结Z轴")]
    public bool z_freeze;
}

[System.Serializable]
public class AroundMoveConfig
{
    [Header("移动速度")]
    public float speed;

    [Header("跟随目标的速度")]
    public float follow_speed;
}

[System.Serializable]
public class BezierCurveMoveConfig {

    [Header("时长")]
    public float duration;

}

[System.Serializable]
public class FollowTweenConfig {

    [Header("跟随时长")]
    public float duration;

    [Header("朝向类型:0保持跟目标一致 1保持自身的方向")]
    public int direction_type;

    [Header("跟目标的偏移")]
    public Vector3 offset_pos;

    [Header("冻结Z轴的旋转")]
    public bool freeze_rotate_z;
}

[System.Serializable]
public class SummonConfig {
    [Header("触发点")]
    public float trigger;
    [Header("NPC_ID")]
    public int npc_id;
    [Header("数量")]
    public int count;

    [Header("保持最大的数量")]
    public int max_count;

    [Header("站位最小夹角")]
    public float angle_min;
    [Header("站位最大夹角")]
    public float angle_max;

    [Header("站位最小距离")]
    public float distance_min;
    [Header("站位最大距离")]
    public float distance_max;
    [Header("进入的状态")]
    public int state;
  
}