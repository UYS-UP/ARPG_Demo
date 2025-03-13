using System;
using System.Collections;
using System.Collections.Generic;

using Game.Config;

using UnityEngine;
using UnityEngine.UI;

public class ForgeView : View
{
    Transform config_content;
    Text forge_info;
    Transform target_prop;
    Text Mat1_Text, Mat2_Text, Mat3_Text,Mat4_Text;
    Transform prop1, prop2, prop3;

    public override void Awake()
    {
        base.Awake();
        config_content = transform.Find("Drawing/Viewport/Content");
        forge_info = GetComponent<Text>("Forge_Tips");
        target_prop = transform.Find("Prop");
        Mat1_Text = GetComponent<Text>("Mat1_Text");
        Mat2_Text = GetComponent<Text>("Mat2_Text");
        Mat3_Text = GetComponent<Text>("Mat3_Text");
        Mat4_Text = GetComponent<Text>("Mat4_Text");

        prop1 = transform.Find("Material/Prop_1");
        prop2 = transform.Find("Material/Prop_2");
        prop3 = transform.Find("Material/Prop_3");

        ShowConfigs();

        GetComponent<Button>("ForgeBtn").onClick.AddListener(ForgeOnClick);
    }

    private void ForgeOnClick()
    {
        if (target_config_id != 0)
        {
            //少了哪些物品
            Dictionary<int, int> dct = new Dictionary<int, int>();
            var config = ForgeData.Get(target_config_id);
            if (config != null)
            {
                if (config.mat1!=null)
                {
                    var count = BagData.Instance.Query(config.mat1[0]);
                    if (count < config.mat1[1])
                    {
                        //打开
                        dct[config.mat1[0]] = config.mat1[1] - count;
                    }
                }

                if (config.mat2 != null)
                {
                    var count = BagData.Instance.Query(config.mat2[0]);
                    if (count < config.mat2[1])
                    {
                        dct[config.mat2[0]] = config.mat2[1] - count;
                    }
                }

                if (config.mat3 != null)
                {
                    var count = BagData.Instance.Query(config.mat3[0]);
                    if (count < config.mat3[1])
                    {
                        dct[config.mat3[0]] = config.mat3[1] - count;
                    }
                }

                if (dct.Count==0)
                {
                    //扣掉背包的材料 
                    if (config.mat1 != null) {
                        BagData.Instance.Remove_Prop_Id(config.mat1[0], config.mat1[1],null);
                    }

                    if (config.mat2 != null)
                    {
                        BagData.Instance.Remove_Prop_Id(config.mat2[0], config.mat2[1], null);
                    }

                    if (config.mat3 != null)
                    {
                        BagData.Instance.Remove_Prop_Id(config.mat3[0], config.mat3[1], null);
                    }

                    //获得物品(添加物品到背包)
                    BagData.Instance.Add(config.prop_id, 1, null);
                    BagViewController.Instance.view.Show(-1);
                    //打开提示窗 提示一下 合成成功
                    //TipsViewController.Instance.ShowForgeResult(true,null);

                    string tips = $"合成成功,获得: <color=red>{PropData.Get(config.prop_id).name}</color> *1";
                    TipsViewController.Instance.Show(tips, null, null);
                }
                else
                {
                    //TipsViewController.Instance.ShowForgeResult(false, dct);
                    string tips = $"合成物品: <color=red>{PropData.Get(config.prop_id).name}</color> 失败,材料不足!";
                    TipsViewController.Instance.Show(tips,null,null);
                }

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
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
   
    void ShowConfigs() {
        var data= ForgeData.all;
        if (data!=null)
        {
            foreach (var item in data)
            {
                //"Drawing/Viewport/Content"

                var obj=ResourcesManager.Instance.CreateForgeItem();
                obj.gameObject.name = item.Key.ToString();
                var id = item.Key;

                obj.transform.SetParent(config_content, false);
                //名称 图标

                var p= PropData.Get(item.Value.prop_id);
                obj.transform.Find("Name").GetComponent<Text>().text = p.name.Split("-")[0];
                obj.transform.Find("Icon").GetComponent<Image>().sprite = ResourcesManager.Instance.LoadIcon(p);

                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UpdateMat(id);
                });
            }
        }
        
    }
    Transform select;
    public int target_config_id;
    void UpdateMat(int id) {

        if (select!=null)
        {
            select.GetComponent<Image>().color = Color.black;
            if (ColorUtility.TryParseHtmlString("#FA8D1C",out Color org_color))
            {
                select.transform.Find("Name").GetComponent<Text>().color = org_color;
            }
        }

        select= config_content.transform.Find(id.ToString());
        select.GetComponent<Image>().color = Color.yellow;
        select.transform.Find("Name").GetComponent<Text>().color = Color.white;

        target_config_id = id;
        var config= ForgeData.Get(id);   
        if (config!=null)
        {
            var p = PropData.Get(config.prop_id);
            SetText(forge_info, p.info);


            //最终合成的物品
            UpdateMatInfo(target_prop, p, 1, Mat4_Text,false);


            //更新材料1的显示
            if (config.mat1==null)
            {
                if (prop1.transform.childCount>0)
                {
                    ResourcesManager.Instance.Destroy(prop1.GetChild(0).gameObject);
                }
                Mat1_Text.text = "";
            }
            else
            {
                var p1 = PropData.Get(config.mat1[0]);
                UpdateMatInfo(prop1, p1, config.mat1[1], Mat1_Text);
            }

            //更新材料2的显示
            if (config.mat2 == null)
            {
                if (prop2.transform.childCount > 0)
                {
                    ResourcesManager.Instance.Destroy(prop2.GetChild(0).gameObject);
                }
                Mat2_Text.text = "";
            }
            else
            {
                
                var p2 = PropData.Get(config.mat2[0]);
                UpdateMatInfo(prop2, p2, config.mat2[1], Mat2_Text);
            }
           

            //更新材料3的显示
            if (config.mat3 == null)
            {
                if (prop3.transform.childCount > 0)
                {
                    ResourcesManager.Instance.Destroy(prop3.GetChild(0).gameObject);
                }
                Mat3_Text.text = "";
            }
            else
            {
                var p3 = PropData.Get(config.mat3[0]);
                UpdateMatInfo(prop3, p3, config.mat3[1], Mat3_Text);
            }
          
        }


    }

    public void UpdateMatInfo(Transform parent, PropEntity p,int count,Text name,bool check_count=true) {
        if (parent.childCount>0)
        {
           ResourcesManager.Instance.Destroy(parent.GetChild(0).gameObject);    
        }
        var item = ResourcesManager.Instance.CreatePropItem(p.id, count);
        item.transform.SetParent(parent, false);
        SetText(name, p.name);

        if (check_count)
        {
            var prop_count = BagData.Instance.Query(p.id);
            var count_text = item.transform.Find("count/key_text").GetComponent<Text>();
            if (prop_count < count) {
                count_text.text = count.ToString() + "/" + "<color=red>"+prop_count+"</color>";
            }
            else
            {
                count_text.text = count.ToString() + "/" + prop_count;
            }
        }
     

    }
}
