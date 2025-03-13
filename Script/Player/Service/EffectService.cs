using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectService : FSMServiceBase
{
    public override void Init(FSM fsm)
    {
        base.Init(fsm);
    }

    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        ReSetAllExcuted();
    }

    public override void OnDisable(PlayerState state)
    {
        base.OnDisable(state);
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);
        if (state.stateEntity.effectConfigs != null&& state.stateEntity.effectConfigs.Count>0) {
            for (int i = 0; i < state.stateEntity.effectConfigs.Count; i++)
            {
                var e = state.stateEntity.effectConfigs[i];
                if (normalizedTime >=e.trigger&&GetExcuted(i)==false)
                {
                    SetExcuted(i);
                    //try
                    //{
                        DO(e, state);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Debug.LogError($"{player._gameObject.name}  ==> {state.id} ==> {ex.Message}");
                    //}
                   
                }
            }
        }
    }

    public override void ReLoop(PlayerState state)
    {
        base.ReLoop(state);
    }

    public override void ReStart(PlayerState state)
    {
        base.ReStart(state);
    }

    void DO(EffectConfig entity,PlayerState state) {
       //var configs= state.stateEntity.effectConfigs;
       // if (configs != null) { }
       if (entity != null)
        {
            //起点
            //朝向
            //生成多个
            //生成方式:0单发 1多发散射 2矩形队列 3范围内随机
            if (entity.create_type == 0)
            {
                var skill_obj = ResourcesManager.Instance.Create_Skill(entity.res_path);
                var hang_point = player.GetAtkTarget(entity.spawn_point_type, entity.spawn_hang_point);
                skill_obj.transform.position = hang_point.transform.position
                    +hang_point.transform.TransformDirection(entity.position_offset);

                //出生-朝向:0挂点方向 1朝向目标 2自身前方 3目标前方
                SetRotate(entity, skill_obj, hang_point);
                SetMove(entity, skill_obj, hang_point);
                SetHit(entity, skill_obj, hang_point, state);
            }
            else if (entity.create_type == 1)//1多发散射
            {
                var hang_point = player.GetAtkTarget(entity.spawn_point_type, entity.spawn_hang_point);
                var pos = hang_point.transform.position + hang_point.transform.TransformDirection(entity.position_offset);
                Vector3 begin_pos = Vector3.zero;
                Quaternion begin_rotation=Quaternion.identity;
                for (int i = 0; i < entity.fan_count; i++)
                {
                    var skill_obj = ResourcesManager.Instance.Create_Skill(entity.res_path);
                    skill_obj.transform.position = pos;

                    //出生-朝向:0挂点方向 1朝向目标 2自身前方 3目标前方
                    if (i==0)
                    {
                        SetRotate(entity, skill_obj, hang_point);
                        begin_pos = skill_obj.transform.position;
                        begin_rotation= skill_obj.transform.rotation;

                        SetMove(entity, skill_obj, hang_point);
                        SetHit(entity, skill_obj, hang_point, state);
                    }
                    else
                    {
                       
                        skill_obj.transform.forward= begin_pos.GetOffsetPoint
                            (begin_rotation, 1, i * -entity.fan_angle_difference) - skill_obj.transform.position;
                        SetMove(entity, skill_obj, hang_point);
                        SetHit(entity, skill_obj, hang_point, state);

                        var skill_obj2 = ResourcesManager.Instance.Create_Skill(entity.res_path);
                        skill_obj2.transform.position = pos;// hang_point.transform.position;
                        skill_obj2.transform.forward = begin_pos.GetOffsetPoint
                            (begin_rotation, 1, i * entity.fan_angle_difference) - skill_obj2.transform.position;
                        
                        SetMove(entity, skill_obj2, hang_point);
                        SetHit(entity, skill_obj2, hang_point, state);
                    }
                   
              
                }
            }
            else if (entity.create_type == 2)// 2矩形队列 
            {
                var hang_point = player.GetAtkTarget(entity.spawn_point_type, entity.spawn_hang_point);
                Vector3 forward = hang_point.transform.forward;
                Vector3 right= hang_point.transform.right;
                Vector3 spawn_point = hang_point.transform.position -
                    entity.rect_columns / 2.0f * right* entity.rect_columns_spacing
                    + right* (entity.rect_columns_spacing/2.0f) +
                    hang_point.transform.TransformDirection(entity.position_offset);

                for (int i = 0; i < entity.rect_rows; i++)
                {
                    for (int j = 0; j < entity.rect_columns; j++)
                    {
                        var skill_obj = ResourcesManager.Instance.Create_Skill(entity.res_path);
                        //skill_obj.transform.position = hang_point.transform.position;
                        skill_obj.transform.position= spawn_point +
                            forward * i * entity.rect_rows_spacing
                            + right * j * entity.rect_columns_spacing;

                        SetRotate(entity, skill_obj, hang_point);

                        SetMove(entity, skill_obj, hang_point);
                        SetHit(entity, skill_obj, hang_point, state);
                    }
                }

            }
            else if (entity.create_type == 3)//3范围内随机
            {
                var hang_point = player.GetAtkTarget(entity.spawn_point_type, entity.spawn_hang_point);
                Vector3 spawn_point = hang_point.transform.position 
                    + hang_point.transform.TransformDirection(entity.position_offset);

                var rotation=hang_point.transform.rotation;
                for (int i = 0; i < entity.random_count; i++)
                {
                    var skill_obj = ResourcesManager.Instance.Create_Skill(entity.res_path);
                    skill_obj.transform.position = spawn_point.GetOffsetPoint
                        (rotation,IntEx.Range( entity.random_radius, entity.random_radius_max),
                        IntEx.Range(entity.random_angle, entity.random_angle_max));
                    SetRotate(entity, skill_obj, hang_point);

                    SetMove(entity, skill_obj, hang_point);
                    SetHit(entity, skill_obj, hang_point, state);
                }

            }
        }
    }

    private void SetHit(EffectConfig entity, GameObject skill_obj, GameObject hang_point,PlayerState playerState)
    {
       var collider= skill_obj.GetComponentsInChildren<Collider>();
        var skill_effect= skill_obj.GetOrAddComponent<SkillEffect>();
        skill_effect.Init(entity);
        if (collider!=null&& collider.Length>0)
        {
            foreach (var item in collider)
            {
               var hit_ctrl= item.gameObject.GetOrAddComponent<SkillHitController>();
                hit_ctrl.Init(this, entity, playerState, skill_effect);
            }

        }
    }

    public void SetMove(EffectConfig entity, GameObject skill_obj,GameObject hang_point) {
        if (entity.move_type==0)
        {
            var direction_move= skill_obj.GetOrAddComponent<DirectionMove>();
            direction_move.Init(entity.directMoveConfg, entity.directMoveConfg.speed, skill_obj.transform.forward);
        }
        else if (entity.move_type==1)
        {
            var track_move= skill_obj.GetOrAddComponent<TrackMove>();
            track_move.Init(entity.trackMoveConfig,player.GetTrackTarget());
        }
        else if (entity.move_type==2)
        {
           var around_move= skill_obj.GetOrAddComponent<AroundMove>();
            around_move.Init(entity.aroundMoveConfig, hang_point.transform);
        }else if (entity.move_type==3)
        {
            var bezierCurveMove= skill_obj.GetOrAddComponent<BezierCurveMove>();
            bezierCurveMove.Init(entity.bezierCurveMoveConfig);
        }
        else if (entity.move_type==4)
        {
            var move= skill_obj.GetOrAddComponent<FollowMove>();
            move.Init(hang_point.transform, entity.followTween);
        }
    
    }


    private void SetRotate(EffectConfig entity, GameObject skill_obj, GameObject hang_point)
    {
        if (entity.rotate_type == 0)
        {
            skill_obj.transform.forward = hang_point.transform.forward;
        }
        else if (entity.rotate_type == 1)
        {
            skill_obj.transform.forward = player.GetAtkTarget()._transform.position - skill_obj.transform.position;
        }
        else if (entity.rotate_type == 2)
        {
            skill_obj.transform.forward = player._transform.forward;
        }
        else if (entity.rotate_type == 3)
        {
            skill_obj.transform.forward = player.GetAtkTarget()._transform.forward;
        }
    }
}
