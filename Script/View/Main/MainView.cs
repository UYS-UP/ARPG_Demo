using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainView : View
{
    Image player_hp_middle, player_hp_top;
    Image player_mp_middle, player_mp_top;
    Image boss_hp_middle, boss_hp_top;
    Transform boss_root;
    Text boss_name;
    GameObject skill_cd_tips;
    GameObject Talk;
    GameObject FailResult;
    GameObject WinResult;
    Text TalkText;
    GameObject GetPropTips;
    public override void Awake()
    {
        base.Awake();
        WinResult = transform.Find("WinResult").gameObject;
        FailResult = transform.Find("FailResult").gameObject;

        player_hp_middle = GetComponent<Image>("PlayerHP/PlayerHP_Middle");
        player_hp_top = GetComponent<Image>("PlayerHP/PlayerHP_Top");

        player_mp_middle = GetComponent<Image>("PlayerMP/PlayerMP_Middle");
        player_mp_top = GetComponent<Image>("PlayerMP/PlayerMP_Top");

        boss_hp_middle = GetComponent<Image>("BOSS/HP_Base/HP_Middle");
        boss_hp_top = GetComponent<Image>("BOSS/HP_Base/HP_Top");

        boss_root = transform.Find("BOSS");
        boss_name = GetComponent<Text>("BOSS/Name/Name_Text");

        mask_q = GetComponent<Image>("Quik/Skill_Q/Item/mask");
        mask_e = GetComponent<Image>("Quik/Skill_E/Item/mask");
        mask_r = GetComponent<Image>("Quik/Skill_R/Item/mask");
        mask_t = GetComponent<Image>("Quik/Skill_T/Item/mask");

        countdown_q = GetComponent<Text>("Quik/Skill_Q/Item/countdown");
        countdown_e = GetComponent<Text>("Quik/Skill_E/Item/countdown");
        countdown_r = GetComponent<Text>("Quik/Skill_R/Item/countdown");
        countdown_t= GetComponent<Text>("Quik/Skill_T/Item/countdown");

        skill_cd_tips = transform.Find("Skill_CD_Tips").gameObject;
        Talk = transform.Find("Talk").gameObject;
        TalkText = Talk.transform.Find("Text").GetComponent<Text>();

        GetPropTips = transform.Find("GetPropTips").gameObject;
    }

    Queue<string> _GetPropTipsQueue = new Queue<string>();
    public void OnGetProp(string text)
    {
        _GetPropTipsQueue.Enqueue(text);
    }
    float _show_get_prop_last;
    public void ShowGetPropTips()
    {
        if (Time.time- _show_get_prop_last>=0.35f)
        {
            _show_get_prop_last = Time.time;
            if (_GetPropTipsQueue.Count > 0)
            {
                if (_GetPropTipsQueue.TryDequeue(out var v))
                {
                    var obj = ResourcesManager.Instance.Create_GetPropTips();
                    obj.transform.SetParent(this.transform, false);
                    obj.transform.position = GetPropTips.transform.position;

                    obj.transform.Find("Text").GetComponent<Text>().text = v;
                    var group = obj.GetComponent<CanvasGroup>();
                    group.alpha = 1;
                    obj.transform.DOLocalMoveY(obj.transform.localPosition.y + 150f, 0.5f).OnComplete(() => {
                        group.DOFade(0, 0.8F).OnComplete(() => {
                            ResourcesManager.Instance.Destroy_GetPropTips(obj);
                            //obj.gameObject.SetActive(false);
                        });
                    });
                }
            }
        }
       

    }


    public void OnLevelComplete(bool win) {
        if (win)
        {
            WinResult.SetActive(true);
            FailResult.SetActive(false);
        }
        else
        {
            FailResult.SetActive(true);
            WinResult.SetActive(false);
        }
    }

    public void CloseResult() {
        FailResult.SetActive(false);
        WinResult.SetActive(false);
    }

    public void Talk_Enable(bool enable, Vector3 localPoint, string text) {
      
        Talk.gameObject.SetActive(enable);
        if (enable)
        {
            TalkText.text = text;
            Talk.transform.position = localPoint;
        }
        //Talk.GetComponent<RectTransform>().anchoredPosition = localPoint+Vector2.up;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        DOSetFillAmout(boss_hp, boss_hp_top, boss_hp_middle);
        DOSetFillAmout(player_mp, player_mp_top, player_mp_middle);
        DOSetFillAmout(player_hp, player_hp_top, player_hp_middle);
        DOUpdatePlayerCD();//更新cd
        CloseCD_Tips();

        ShowGetPropTips();
    }

    public void EnableBossHP(bool enable,string name) {
        if (boss_root != null)
        {
            boss_root.gameObject.SetActive(enable);
            SetText(boss_name, name);
        }

    }

    public float boss_hp = -1;
    public float middle_speed = 1f;
    public void UpdateBossHP(float v) {
        boss_hp = v;
    }

    public float player_hp = -1;
    public void UpdatePlayerHP(float v) {
        player_hp = v;
    }

    public float player_mp= -1;
    public void UpdatePlayerMP(float v)
    {
        player_mp = v;
    }


    public void DOSetFillAmout(float v,Image top,Image middle) {
        if (v == -1)
        {
            return;
        }
        if (top.fillAmount > v)
        {
            top.SetFillAmount(v, middle_speed * 2);
        }
        else if (top.fillAmount < v)
        {
            top.SetFillAmount(v, middle_speed);
        }

        if (middle.fillAmount > v)
        {
            middle.SetFillAmount(v, middle_speed);
        }
        else if (middle.fillAmount < v)
        {
            middle.SetFillAmount(v, middle_speed * 2);
        }
    }

    float q_cd, e_cd, r_cd, t_cd;
    float q_cd_begin, e_cd_begin, r_cd_begin, t_cd_begin;
    Image mask_q, mask_e, mask_r, mask_t;
    Text countdown_q, countdown_e, countdown_r, countdown_t;
    public void SetSkillCD(int type,float cd) {
        switch (type)
        {
            case 0:
                q_cd_begin = GameTime.time;
                q_cd = cd;
                mask_q.gameObject.SetActive(true);
                mask_q.fillAmount = 1;
                countdown_q.gameObject.SetActive(true);
                break;
            case 1:
                e_cd_begin = GameTime.time;
                e_cd = cd;
                mask_e.gameObject.SetActive(true);
                mask_e.fillAmount = 1;
                countdown_e.gameObject.SetActive(true);
                break;
            case 2:
                r_cd_begin = GameTime.time;
                r_cd = cd;
                mask_r.gameObject.SetActive(true);
                mask_r.fillAmount = 1;
                countdown_r.gameObject.SetActive(true);
                break;
            case 3:
                t_cd_begin = GameTime.time;
                t_cd = cd;
                mask_t.gameObject.SetActive(true);
                mask_t.fillAmount = 1;
                countdown_t.gameObject.SetActive(true);
                break;

        }
    }

    public void DOUpdatePlayerCD() {
        UpdateSkillCD(ref q_cd, q_cd_begin, mask_q, countdown_q);
        UpdateSkillCD(ref e_cd, e_cd_begin, mask_e, countdown_e);
        UpdateSkillCD(ref r_cd, r_cd_begin, mask_r, countdown_r);
        UpdateSkillCD(ref t_cd, t_cd_begin, mask_t, countdown_t);
    }

    public void UpdateSkillCD(ref float cd,float begin,Image mask,Text countdown) {
        if (cd!=0)
        {
            if (mask.fillAmount!=0)
            {
                var result=mask.SetFillAmount(0, 1/cd);
                if (result==true)
                {
                    cd = 0;
                    mask.gameObject.SetActive(false);
                    countdown.gameObject.SetActive(false);
                }
            }
            countdown.text = Math.Ceiling(cd - (GameTime.time - begin)).ToString();
        }
    }

    float cd_tips_begin;
    internal void OpenCD_Tips()
    {
        cd_tips_begin = GameTime.time;
        skill_cd_tips.gameObject.SetActive(true);
    }

    public void CloseCD_Tips() {
        if (cd_tips_begin==0)
        {
            return;
        }
        if (GameTime.time-cd_tips_begin>=2)
        {
            cd_tips_begin = 0;
            skill_cd_tips.gameObject.SetActive(false);
        }
    }
}
