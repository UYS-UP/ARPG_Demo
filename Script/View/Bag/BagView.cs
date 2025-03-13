using System;
using System.Collections;
using System.Collections.Generic;
using Game.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagView : View
{
 
    string content = "Bag/Scroll View/Viewport/Content/Prop_";
    string quikc_root = "Quick/Prop_";
    Transform content_parent;

    Transform Prop;
    Transform Material;
    Transform Equip;

    Text prop_name, prop_text, prop_act;
    Text prop_info1,prop_info2,prop_info3;
    Text mat_name, mat_info, mat_act;
    Text equip_name, equip_info, equip_act;

    Text att1, att2, att3, att4, att5;

    Transform select;
    Transform cmd;
    public override void Awake()
    {
        base.Awake();
        content_parent = transform.Find("Bag/Scroll View/Viewport/Content");
        GetComponent<Button>("Bag/Menu/All").onClick.AddListener(ShowAll);
        GetComponent<Button>("Bag/Menu/Prop").onClick.AddListener(ShowProp);
        GetComponent<Button>("Bag/Menu/Equip").onClick.AddListener(ShowEquip);
        GetComponent<Button>("Bag/Menu/Material").onClick.AddListener(ShowMaterial);

        Prop = transform.Find("Prop");
        Material = transform.Find("Material");
        Equip = transform.Find("Equip");
        

         prop_name =GetComponent<Text>("Prop/Name");
         prop_text =GetComponent<Text>("Prop/Info");
         prop_act = GetComponent<Text>("Prop/Act");

        prop_info1 = GetComponent<Text>("Prop/Att/Info1");
        prop_info2 = GetComponent<Text>("Prop/Att/Info2");
        prop_info3 = GetComponent<Text>("Prop/Att/Info3");

        mat_name = GetComponent<Text>("Material/Name");
        mat_info = GetComponent<Text>("Material/Info");
        mat_act = GetComponent<Text>("Material/Act");

        equip_name = GetComponent<Text>("Equip/Name");
        equip_info = GetComponent<Text>("Equip/Info");
        equip_act = GetComponent<Text>("Equip/Act");

        att1 = GetComponent<Text>("Equip/Att/1");
        att2 = GetComponent<Text>("Equip/Att/2");
        att3 = GetComponent<Text>("Equip/Att/3");
        att4 = GetComponent<Text>("Equip/Att/4");
        att5 = GetComponent<Text>("Equip/Att/5");

        sort = GetComponent<Button>("Bag/Sort");
        sort.onClick.AddListener(SortByType);

        select = GetComponent<Transform>("Bag/Select");

        for (int i = 0; i < content_parent.childCount; i++)
        {
            var btn= content_parent.GetChild(i).GetComponent<Button>();
            btn.onClick.AddListener(GrildOnClick);
        }

        cmd = transform.Find("CMD");

        GetComponent<Button>("CMD/Discard").onClick.AddListener(RemoveGrildItem);

        AttTextInit();
    }

    public void GrildOnClick() {
        select_grild_id = int.Parse(GameSystem.Instance.EventSystem.currentSelectedGameObject.name.Split('_')[1]);
        var entity = BagData.Instance.Get(select_grild_id);
        if (entity!=null)
        {
            select.gameObject.SetActive(true);
            select.transform.position = GameSystem.Instance.EventSystem.currentSelectedGameObject.transform.position;
        }
        else
        {
            select.gameObject.SetActive(false);
            cmd.gameObject.SetActive(false);
        }
    }

    int select_grild_id=-1;
    public void OnMouseButtonDown() {
        if (UInput.GetMouseButtonDown_1())
        {
            if (SelectObj != null
                && SelectObj.name.StartsWith("Prop_"))
            {
                select_grild_id=int.Parse(SelectObj.name.Split('_')[1]);
                
                var entity = BagData.Instance.Get(select_grild_id);
                if (entity!=null)
                {
                    select.gameObject.SetActive(true);
                    select.transform.position = SelectObj.transform.position;

                    cmd.gameObject.SetActive(true);
                    cmd.transform.position = SelectObj.transform.position;
                }
                else
                {
                    select.gameObject.SetActive(false);
                    cmd.gameObject.SetActive(false);
                }
            }
        }
    }

    public void RemoveGrildItem() {
        var entity= BagData.Instance.Get(select_grild_id);
        if (entity != null) {
            BagData.Instance.Remove_Grild_ID(select_grild_id, entity.count, null);
            ResourcesManager.Instance.Destroy(bag_item[select_grild_id]);
            bag_item.Remove(select_grild_id);
            select.gameObject.SetActive(false);
            cmd.gameObject.SetActive(false);
            select_grild_id = -1;
        }
    }


    Button sort;
    private void ShowMaterial()
    {
        Show(2);
    }

    private void ShowEquip()
    {
        Show(1);
    }

    private void ShowProp()
    {
        Show(0);
    }

    private void ShowAll()
    {
        Show(-1);
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
        //清理上一次的残留
        //Show(-1);
        if (UnitManager.Instance.player!=null)
        {
            UpdateAtt(UnitManager.Instance.player.att_crn);
        }
    }


    Dictionary<int, GameObject> bag_item = new Dictionary<int, GameObject>();
    public void ClearGrild()
    {
        foreach (var item in bag_item)
        {
            ResourcesManager.Instance.Destroy_BagItem(item.Value);
        }
        bag_item.Clear();
    }

    internal void AddBagItem(int grild_id, GameObject obj)
    {
        bag_item[grild_id] = obj;
    }

    public void RemoveBagItem(int grild) {
        if (bag_item.ContainsKey(grild))
        {
            bag_item.Remove(grild);
        }
    }


    Dictionary<int, GameObject> equip = new Dictionary<int, GameObject>();
    public void Add_Euqip_Item(int part,GameObject go) {
        equip[part] = go;
    }

    public void Remove_Euqip_Item(int part)
    {
        if (equip.ContainsKey(part)) {
            var go= equip[part];
            equip.Remove(part);
        }
    }


    Dictionary<int, GameObject> quick = new Dictionary<int, GameObject>();
    public void Add_Quick_Item(int part, GameObject go)
    {
        quick[part] = go;
    }

    public void Remove_Quick_Item(int part)
    {
        if (quick.ContainsKey(part))
        {
            var go = quick[part];
            quick.Remove(part);
        }
    }



    int crn_type;
    internal GameObject SelectObj;

    //-1 全部 0物品 1装备 2材料
    public void Show(int type=-1) {
        select.gameObject.SetActive(false);
        ClearGrild();
        crn_type = type;
        var data = BagData.Instance.dct;
        foreach (var item in data)
        {
            if (type == -1 || item.Value.entity.type == type)
            {
                //创建这些物品 
                var obj = CreateItem(item.Value);
                var parent = content + item.Key;
                var p = transform.Find(parent);
                obj.transform.SetParent(p, false);
                bag_item[item.Value.grild_id] = obj;
            }
        }

        if (type == -1)
        {
            for (int i = 0; i < content_parent.childCount; i++)
            {
                content_parent.GetChild(i).gameObject.SetActive(true);
            }
        }
        else {
            //判断
            for (int i = 0; i < content_parent.childCount; i++)
            {
                var obj = content_parent.GetChild(i);
                var grildInfo = BagData.Instance.Get(i);
                if (grildInfo != null)
                {
                    obj.gameObject.SetActive(grildInfo.entity.type == type);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }


        ClearGrild_Quick();
        foreach (var item in BagData.Instance.quick_dct)
        {
            //创建这些物品 
            var obj = CreateItem(item.Value);
            var parent = quikc_root + item.Key;
            var p = transform.Find(parent);
            obj.transform.SetParent(p, false);
            quick[item.Value.grild_id] = obj;
        }

    }

    private void ClearGrild_Quick()
    {
        foreach (var item in quick)
        {
            ResourcesManager.Instance.Destroy_BagItem(item.Value);
        }
        quick.Clear();
    }

    public void SortByType() {
        BagData.Instance.SortByType();
        Show(crn_type);
    }

    public GameObject CreateItem(BagEntity bagEntity) {
        var obj =ResourcesManager.Instance.CreatePropItem(bagEntity.entity.id,bagEntity.count);
        return obj;
    }

 
    public void Follow(GameObject obj,Vector3 point) {
        obj.transform.position = point;
    }



    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        OnMouseButtonDown();
    }


    public void ShowPropInfo(int grild_id,Vector3 point, BagEntity bagEntity) {

        if (bagEntity != null) {
            Prop.gameObject.SetActive(true);
            Equip.gameObject.SetActive(false);
            Material.gameObject.SetActive(false);
            //物品名称更新
            //物品描述
            //等级限制
            SetText(prop_name, bagEntity.entity.name);
            SetText(prop_text, bagEntity.entity.info);
             string level= bagEntity.entity.level==0 ? "无限制":bagEntity.entity.level.ToString()+ "级";
            SetText(prop_act, $"类型:物品\n使用等级:{level}");
            var scale = transform.localScale.x / 1;
            if (point.y<=350* scale)
            {
                //378 0 1
                Prop.transform.position = point + new Vector3(0 ,512* scale, 0);
            }
            else
            {
                Prop.transform.position = point;
            }
            //Prop.transform.position = point;

            SetItem(bagEntity.entity.recover1, prop_info1);
            SetItem(bagEntity.entity.recover2, prop_info2);
            SetItem(bagEntity.entity.recover3, prop_info3);
        }
        else
        {
            Prop.gameObject.SetActive(false);
        }


      

    }

    public void ShowMaterialInfo(int grild_id, Vector3 point, BagEntity bagEntity) {

        if (bagEntity != null)
        {
            Prop.gameObject.SetActive(false);
            Equip.gameObject.SetActive(false);
            Material.gameObject.SetActive(true);

            SetText(mat_name, bagEntity.entity.name);
            SetText(mat_info, bagEntity.entity.info);
            string level = bagEntity.entity.level == 0 ? "无限制" : bagEntity.entity.level.ToString() + "级";
            SetText(mat_act, $"类型:材料\n使用等级:{level}");
            Material.transform.position = point;
        }
        else
        {
            Material.gameObject.SetActive(false);
        }


    }

    public void ShowEquipInfo(int grild_id, Vector3 point , BagEntity bagEntity) {

        if (bagEntity != null)
        {
            Prop.gameObject.SetActive(false);
            Material.gameObject.SetActive(false);
            Equip.gameObject.SetActive(true);

            SetText(equip_name, bagEntity.entity.name);
            SetText(equip_info, bagEntity.entity.info);
            string level = bagEntity.entity.level == 0 ? "无限制" : bagEntity.entity.level.ToString() + "级";
            SetText(equip_act, $"类型:装备/{PartNameData.Get(bagEntity.entity.part).name}\n使用等级:{level}");

            Equip.transform.position = point;

            SetItem(bagEntity.entity.att1,att1);
            SetItem(bagEntity.entity.att2, att2);
            SetItem(bagEntity.entity.att3, att3);
            SetItem(bagEntity.entity.att4, att4);
            SetItem(bagEntity.entity.att5, att5);
        }
        else
        {
            Equip.gameObject.SetActive(false);
        }
    }

    private void SetItem(int[] config,Text text)
    {
        if (config != null)
        {
            text.gameObject.SetActive(true);
            SetText(text,GameDefine.GetAttText(config[0], config[1], config[2]));
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    internal void CloseDescribe()
    {
        Prop.gameObject.SetActive(false);
        Equip.gameObject.SetActive(false);
        Material.gameObject.SetActive(false);
    }

    internal Transform GetGrild(int grild_next)
    {
        return transform.Find(content+grild_next);
    }

    Text hp, phy_atk, magic_atk, phy_def, magic_def, critical_hit_rate, critical_hit_multiple, skill_speed;
    void AttTextInit() {
        hp = GetComponent<Text>("Att/hp");
        phy_atk = GetComponent<Text>("Att/phy_atk");
        magic_atk = GetComponent<Text>("Att/magic_atk");
        phy_def = GetComponent<Text>("Att/phy_def");
        magic_def = GetComponent<Text>("Att/magic_def");
        critical_hit_rate = GetComponent<Text>("Att/critical_hit_rate");
        critical_hit_multiple = GetComponent<Text>("Att/critical_hit_multiple");
        skill_speed = GetComponent<Text>("Att/skill_speed");
    }

    public void UpdateAtt(UnitAttEntity att_crn) {
        SetText(hp,AttNameData.Get(1).name+":"+ att_crn.hp);
        SetText(phy_atk, AttNameData.Get(2).name + ":" + att_crn.phy_atk);
        SetText(magic_atk, AttNameData.Get(3).name + ":" + att_crn.magic_atk);
        SetText(phy_def, AttNameData.Get(4).name + ":" + att_crn.phy_def);
        SetText(magic_def, AttNameData.Get(5).name + ":" + att_crn.magic_def);
        SetText(critical_hit_rate, AttNameData.Get(6).name + ":" + att_crn.critical_hit_rate);
        SetText(critical_hit_multiple, AttNameData.Get(7).name + ":" + att_crn.critical_hit_multiple);
        SetText(skill_speed, AttNameData.Get(8).name + ":" + att_crn.skill_speed);
    }
}
