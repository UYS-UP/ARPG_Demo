using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitService : FSMServiceBase
{
    public override void OnAnimationEnd(PlayerState state)
    {
        base.OnAnimationEnd(state);
    }

    public override void OnBegin(PlayerState state)
    {
        base.OnBegin(state);
        ReSetAllExcuted();
        hit_target.Clear();
        last_end = Vector3.zero;
    }

    public override void OnEnd(PlayerState state)
    {
        base.OnEnd(state);
        ReSetAllExcuted();
    }

    public override void OnUpdate(float normalizedTime, PlayerState state)
    {
        base.OnUpdate(normalizedTime, state);

        var configs= state.stateEntity.hitConfigs;
        if (configs != null && configs.Count>0)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                var e= configs[i];
                if (normalizedTime >=e.trigger&&normalizedTime<=e.end)
                {
                    hit_target.Clear();
                    DO(e, state);
                }
            }
        }

    }
    Vector3 last_end;
    private void DO(HitConfig config, PlayerState state)
    {
        var obj = player.GetHangPoint(config.begin);
        Vector3 begin = obj.transform.position;
        if (config.type==0)
        {
            Vector3 end = begin + obj.transform.forward * config.length;
            if (last_end==Vector3.zero)
            {
                Linecast(begin, end, config, state);
            }
            else
            {
                var _crn_id = player.currentState.id;
                for (int i = 0; i < 10; i++)
                {
                    Vector3 end2=Vector3.Lerp(last_end,end,i/10f);
                    Linecast(begin, end2, config, state);
                    if (_crn_id != player.currentState.id)
                    {
                        return;
                    }
                }
            }
            last_end = end;
        }
        else if (config.type==1) {
            BoxCast(obj.transform, config, state);
        }
    }

    List<int> hit_target=new List<int>();
    public bool Linecast(Vector3 begin,Vector3 end,HitConfig config,PlayerState state) {
        Debug.DrawLine(begin, end, Color.red, 0.2f);
        //Physics.RaycastNonAlloc
        var result = Physics.Linecast(begin, end, out var hitInfo, player.GetEnemyLayerMask(),QueryTriggerInteraction.Collide);
        if (result)
        {
            var fsm = hitInfo.transform.GetComponent<FSM>();
            if (fsm.global_state == 1) { return false; }
            var d = Vector3.Distance(fsm._transform.position, player._transform.position);
            var f = fsm._transform.ForwardOrBack(player._transform.position);

            //���ڸ�״̬
            if (hitInfo.transform.CompareTag(GameDefine.WeaponTag))
            {
                OnBlock(hitInfo);

            }
            //���߶Է������Ÿ񵲶���,�����Ǵ��ڸ񵲷�Χ�ڵ�
            else if (fsm.IsBlockState() && f > 0 && d <= state.skill.block_distance)
            {
                OnBlock(hitInfo);
            }
            else
            {
                OnHit(begin, config, state, hitInfo);
            }

            return true;
        }
        return false;
    }

    private void OnBlock(RaycastHit hitInfo)
    {
        //�񵲷�
        var fsm = hitInfo.transform.GetComponentInParent<FSM>();
        if (fsm != null && hit_target.Contains(fsm.instance_id) == false)
        {
            hit_target.Add(fsm.instance_id);
            //1.���ɸ�ʱ��Ч
            var blockEffect = ResourcesManager.Instance.Create_Skill(CombatConfig.Instance.Config().block_effect);
            if (blockEffect != null)
            {
                blockEffect.transform.position = hitInfo.point;
                blockEffect.transform.forward = hitInfo.normal;
            }

            //��ͷģ������ 
            GameEvent.DORadialBlur?.Invoke(CombatConfig.Instance.Config().block_radialBlur);
            //��֡
            GameEvent.DOHitlag?.Invoke(CombatConfig.Instance.Config().block_hitlag.frame,
                CombatConfig.Instance.Config().block_hitlag.lerp);

            //�Ÿ񵲳ɹ�����Ч
            AudioController.Instance.Play(CombatConfig.Instance.Config().block_audio, hitInfo.point);
            //2.������Ҫ���뵯��״̬
            player.BeBlock(fsm);

            //3.�񵲷�Ҫ����񵲳ɹ���״̬
            fsm.OnBlockSucces(player);

            //6.������Ѫ�� 

        }
    }

    private void OnHit(Vector3 begin, HitConfig config, PlayerState state, RaycastHit hitInfo)
    {
        //��ʾ���е�λ
        var fsm = hitInfo.transform.GetComponent<FSM>();
        if (fsm != null)
        {
            if (hit_target.Contains(fsm.instance_id) == false)
            {
                hit_target.Add(fsm.instance_id);

                //1.����������Ч
                var hitObject = ResourcesManager.Instance.Create_Skill(config.hitObj);
                hitObject.SetActive(true);
                //2.������Ч��λ�� ����

                if (hitObject != null)
                {
                    hitObject.transform.position = hitInfo.point;
                    hitObject.transform.forward = hitInfo.normal;
                }

                //3.���� �۵�Ѫ��
                var damage = AttHelper.Instance.Damage(this.player, state, fsm);
                fsm.UpdateHP_OnHit(damage);
                //4.֪ͨ�Է������ܻ� �����Ķ���
                var fb = fsm._transform.ForwardOrBack(begin) > 0 ? 0 : 1;
                if (fsm.att_crn.hp > 0)
                {
                    if (state.skill.add_fly!=null)
                    {
                        //���ɵ�����
                        fsm.OnBash(fb,this.player,state.skill.add_fly,hitInfo.point);
                    }
                    else
                    {
                        fsm.OnHit(fb, this.player);
                    }
                }
                else
                {
                    fsm.OnDeath(fb);
                }
                //����ʱ�Ķ�֡
                this.player.Attack_Hitlag(state);

                //6.���е���Ч
                AudioController.Instance.Play(CombatConfig.Instance.Config().hit_enemy_audio, hitInfo.point);

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
    RaycastHit[] raycastHits = null;
    public bool BoxCast(Transform begin, HitConfig config, PlayerState state) {
        if (raycastHits==null)
        {
            raycastHits = new RaycastHit[30];
        }
        //���е�����
       var count = Physics.BoxCastNonAlloc(begin.position + begin.transform.TransformDirection(config.box_center), config.box_size,
            begin.forward, raycastHits, begin.rotation, config.length, player.GetEnemyLayerMask(),
            QueryTriggerInteraction.Collide);

        if (count > 0)
        {
            int _crn_id = state.id;
            for (int i = 0; i < count; i++)
            {
                var hitInfo = raycastHits[i];
                var fsm = hitInfo.transform.GetComponent<FSM>();
                if (fsm.global_state == 1) { return false; }
                var d = Vector3.Distance(fsm._transform.position, player._transform.position);
                var f = fsm._transform.ForwardOrBack(player._transform.position);

                if (hitInfo.transform.CompareTag(GameDefine.WeaponTag))
                {
                    OnBlock(hitInfo);
                }
                //���߶Է������Ÿ񵲶���,�����Ǵ��ڸ񵲷�Χ�ڵ�
                else if (fsm.IsBlockState() && f > 0 && d <= state.skill.block_distance)
                {
                    OnBlock(hitInfo);
                }
                else
                {
                    OnHit(begin.position, config, state, hitInfo);
                }
                if (_crn_id!=player.currentState.id)
                {
                    break;
                }
            }
            return true;
        }
        return false;
    }
}
