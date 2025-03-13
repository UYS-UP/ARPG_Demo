using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHitController : MonoBehaviour
{
    EffectService effectService;
    EffectConfig effectConfig;
    PlayerState state;
    SkillEffect skillEffect;
    int hit_unit_max;
    public void Init(EffectService effectService, EffectConfig effectConfig,PlayerState state,SkillEffect skillEffect) { 
        this.effectService = effectService;
        this.effectConfig= effectConfig; 
        this.state = state;
        this.skillEffect = skillEffect;
        hit_unit_max=  effectConfig.unit_hit_count == 0 ? 1 : effectConfig.unit_hit_count;
        hit_objs.Clear();
    }
    Dictionary<int, int> hit_objs = new Dictionary<int, int>();
    //List<int> hit_objs = new List<int>();
    public void OnTriggerEnter(Collider other)
    {
        var fsm= other.GetComponent<FSM>();
        if (fsm!=null)
        {
            if (fsm.global_state==1) { return; }

            if (hit_objs.ContainsKey(fsm.instance_id) == false)
            {
                hit_objs[fsm.instance_id] = 0;
            }

            if (fsm.IsBlockState())
            {
                //�ж���ǰ����
                if (fsm._transform.ForwardOrBack(effectService.player._transform.position)>0)
                {
                    var d = Vector3.Distance(fsm._transform.position, effectService.player._transform.position);
                    if (d<= state.skill.block_distance)
                    {
                        fsm.OnBlockSucces(effectService.player);
                        effectService.player.BeBlock(fsm);


                        //1.���ɸ�ʱ��Ч
                        var blockEffect = ResourcesManager.Instance.Create_Skill(CombatConfig.Instance.Config().block_effect);
                        if (blockEffect != null)
                        {
                            blockEffect.transform.position = fsm.GetBlockEffectPoint();
                            blockEffect.transform.forward = fsm.transform.forward;
                        }

                        //��ͷģ������ 
                        GameEvent.DORadialBlur?.Invoke(CombatConfig.Instance.Config().block_radialBlur);
                        //��֡
                        GameEvent.DOHitlag?.Invoke(CombatConfig.Instance.Config().block_hitlag.frame,
                            CombatConfig.Instance.Config().block_hitlag.lerp);

                        //�Ÿ񵲳ɹ�����Ч
                        AudioController.Instance.Play(CombatConfig.Instance.Config().block_audio, blockEffect.transform.position);
                    

                        skillEffect.DODestroy();
                        return;
                    }

                }
            }

            
            if (hit_objs[fsm.instance_id] < hit_unit_max)
            {
                hit_objs[fsm.instance_id] += 1;
                if (other.gameObject.layer != effectService.player._gameObject.layer)
                {
                    if (effectConfig.hit_effect_count==0|| skillEffect.hit_effect_count<effectConfig.hit_effect_count)
                    { 
                        //�������е���Ч
                        var hit_effect = ResourcesManager.Instance.Create_Skill
                              (CombatConfig.Instance.GetHitEffectPath(effectConfig.hit_effect));

                        hit_effect.transform.position = fsm.hit_target.transform.position;
                        hit_effect.transform.forward = this.transform.position - fsm.hit_target.transform.position;

                        skillEffect.hit_effect_count += 1;
                    }

                    //�з���Ѫ
                    var damage = AttHelper.Instance.Damage(effectService.player, state, fsm);
                    fsm.UpdateHP_OnHit(damage);


                    var fb = fsm._transform.ForwardOrBack(this.transform.position) > 0 ? 0 : 1;
                    if (fsm.att_crn.hp > 0)
                    {
                        if (state.skill.add_fly != null)
                        {
                            //���ɵ�����
                            fsm.OnBash(fb, effectService.player, state.skill.add_fly,this.transform.position);
                        }
                        else
                        {
                            fsm.OnHit(fb, effectService.player);
                        }
                    }
                    else
                    {
                        fsm.OnDeath(fb);
                    }
                    //����ʱ�Ķ�֡
                    this.effectService.player.Attack_Hitlag(state);

                    //6.���е���Ч
                    AudioController.Instance.Play(effectConfig.hit_audio, this.transform.position);
                    if (hit_objs.Count>=effectConfig.destroy_hit_count)
                    {
                        skillEffect.DODestroy();
                    }
                }
            }
           
        }
    }

}
