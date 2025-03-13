using System;
using System.Collections;
using System.Collections.Generic;

using Game.Config;

using UnityEngine;
using UnityEngine.UI;

public class PurifyView : View
{
    Transform Content;
    Text Info_1, Info_2, Info_3, Info_4, Info_5;
    Text Info_1_FX, Info_2_FX, Info_3_FX, Info_4_FX, Info_5_FX;
    Transform SelectImage;
    public override void Awake()
    {
        base.Awake();
        
        Content = transform.Find("Bag/Scroll View/Viewport/Content");
        for (int i = 0; i < Content.childCount; i++) {

            var btn= Content.GetChild(i).GetComponent<Button>();
            int temp = i;
            btn.onClick.AddListener(() => {
                grild_id = temp;
                UpdateAtt_Crn();
            });
        }
        Info_1 = GetComponent<Text>("Att_Crn/Info_1");
        Info_2 = GetComponent<Text>("Att_Crn/Info_2");
        Info_3 = GetComponent<Text>("Att_Crn/Info_3");
        Info_4 = GetComponent<Text>("Att_Crn/Info_4");
        Info_5 = GetComponent<Text>("Att_Crn/Info_5");

        Info_1_FX = GetComponent<Text>("Att_FX/Info_1");
        Info_2_FX = GetComponent<Text>("Att_FX/Info_2");
        Info_3_FX = GetComponent<Text>("Att_FX/Info_3");
        Info_4_FX = GetComponent<Text>("Att_FX/Info_4");
        Info_5_FX = GetComponent<Text>("Att_FX/Info_5");

        GetComponent<Button>("Purify_Btn").onClick.AddListener(PurifyOnClick);
        SelectImage = transform.Find("SelectImage");
    }

    private void PurifyOnClick()
    {
        if (grild_id==-1)
        {
            TipsViewController.Instance.Show($"请选择需要洗炼的装备!");
            return;
        }

        if (BagData.Instance.money>=500)
        {
            BagData.Instance.money -= 500;
            NavViewController.Instance.SetMenoy();
            var data = BagData.Instance.Get(grild_id);
            data.DO_Random_Att();

            SetText(Info_1_FX, GameDefine.GetAttText(data.att1));
            SetText(Info_2_FX, GameDefine.GetAttText(data.att2));
            SetText(Info_3_FX, GameDefine.GetAttText(data.att3));
            SetText(Info_4_FX, GameDefine.GetAttText(data.att4));
            SetText(Info_5_FX, GameDefine.GetAttText(data.att5));

            Info_1_FX.gameObject.SetActive(data.att1 != null);
            Info_2_FX.gameObject.SetActive(data.att2 != null);
            Info_3_FX.gameObject.SetActive(data.att3 != null);
            Info_4_FX.gameObject.SetActive(data.att4 != null);
            Info_5_FX.gameObject.SetActive(data.att5 != null);
        }
        else
        {
            TipsViewController.Instance.Show($"金币不足!", null, null); ;
        }
       
    }

    int grild_id=-1;

    public void UpdateAtt_Crn() {
        var data= BagData.Instance.Get(grild_id);
        SetText(Info_1, GameDefine.GetAttText(data.att1));
        SetText(Info_2, GameDefine.GetAttText(data.att2));
        SetText(Info_3, GameDefine.GetAttText(data.att3));
        SetText(Info_4, GameDefine.GetAttText(data.att4));
        SetText(Info_5, GameDefine.GetAttText(data.att5));

        Info_1.gameObject.SetActive(data.att1 != null);
        Info_2.gameObject.SetActive(data.att2 != null);
        Info_3.gameObject.SetActive(data.att3 != null);
        Info_4.gameObject.SetActive(data.att4 != null);
        Info_5.gameObject.SetActive(data.att5 != null);

        SetText(Info_1_FX, "未知");
        SetText(Info_2_FX, "未知");
        SetText(Info_3_FX, "未知");
        SetText(Info_4_FX, "未知");
        SetText(Info_5_FX, "未知");

        SelectImage.gameObject.SetActive(true);
        SelectImage.transform.position = Content.transform.Find($"Prop_{grild_id}").transform.position;
        //Att_Crn
    }


    public void ShowEquip() {
        grild_id = -1;
        SelectImage.gameObject.SetActive(false);
        var dct= BagData.Instance.dct;
        foreach (var kv in dct) {
            if (kv.Value.entity.type == 1) {
                var obj= ResourcesManager.Instance.CreatePropItem(kv.Value.entity.id, kv.Value.count);
                var p=Content.Find("Prop_"+kv.Value.grild_id);
                obj.transform.SetParent(p, false);
            }
        }


        for (int i = 0; i < Content.childCount; i++)
        {
            var child = Content.GetChild(i);    
            if (child.childCount==0)
            {
                child.gameObject.SetActive(false);
            }
        }

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
        ShowEquip();

        //清理
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
