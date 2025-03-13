using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    //记录当前的格子ID 
    public int grild_id;
    Transform org;

    public int type;//0表示背包里的格子 1身上的装备的格子
    public void Awake()
    {
        if (this.CompareTag("Equip")) {
            type = 1;
        }
        else if (this.CompareTag("Quick"))
        {
            type = 2;
        }
        else
        {
            type = 0;
        }
        grild_id =int.Parse(transform.gameObject.name.Split("_")[1]);
    }

    GameObject temp;
    //当开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        //创建一个 物品
        //Debug.LogError("开始拖动:"+eventData.pointerEnter.gameObject.name);

        if (transform.childCount>0)
        {
            var p = transform.GetChild(0);
            org = p;
            p.gameObject.SetActive(false);
            int id = int.Parse(p.name);
            int count = int.Parse(p.transform.Find("count/key_text").GetComponent<Text>().text);
            temp= ResourcesManager.Instance.CreatePropItem(id, count);
            temp.transform.SetParent(BagViewController.Instance.view.transform, false);
            var rt= temp.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(120f, 120f);

        }
    }

    //正在拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        //更新这个物品的位置
        //Debug.LogError("拖动中:" + eventData.pointerEnter.gameObject.name);
        if (temp!=null)
        {
            temp.transform.position=eventData.position;
        }
    }

    //当结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        //交换位置: 有可能交换的位置 物品是空 或者不是空
        //Debug.LogError("结束拖拽:" + eventData.pointerEnter.gameObject.name);
        if (temp!=null)
        {
            temp.gameObject.SetActive(false);
            ResourcesManager.Instance.Destroy(temp);
            temp = null;

            //背包的格子操作
            if (type==0)
            {
               
                if (eventData.pointerEnter.gameObject.CompareTag("Equip"))
                { 
                    //背包拖向装备栏的逻辑
                    var n = eventData.pointerEnter.gameObject.name;
                    if (n.StartsWith("Item_"))
                    {
                        var part_type = int.Parse(n.Split("_")[1]);

                        var data = BagData.Instance.Get(grild_id);
                        if (data != null)
                        {
                            if (data.entity.part == part_type)
                            {
                                BagData.Instance.Switch_Equip(grild_id);

                                if (eventData.pointerEnter.gameObject.transform.childCount == 0)
                                {
                                    org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                                    org.gameObject.SetActive(true);
                                    BagViewController.Instance.RemoveBagItem(grild_id);
                                    org.transform.Find("count").gameObject.SetActive(false);
                                }
                                else
                                {
                                    org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                                    org.gameObject.SetActive(true);
                                    org.transform.Find("count").gameObject.SetActive(false);

                                    //交换
                                    var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                                    c.SetParent(this.transform, false);
                                    c.transform.Find("count").gameObject.SetActive(true);

                                    BagViewController.Instance.RemoveBagItem(grild_id);
                                    BagViewController.Instance.AddEquipItem(part_type, org.gameObject);
                                    BagViewController.Instance.AddBagItem(grild_id, c.gameObject);

                                }


                            }
                            else
                            {
                                org.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                else if (eventData.pointerEnter.gameObject.CompareTag("Quick"))
                {
                    //背包到快捷栏
                    //背包拖向装备栏的逻辑
                    var n = eventData.pointerEnter.gameObject.name;
                    if (n.StartsWith("Prop_"))
                    {
                        //1234
                        var target_quick = int.Parse(n.Split("_")[1]);

                        //获取背包数据
                        var data = BagData.Instance.Get(grild_id);
                        if (data != null)
                        {
                            if (data.entity.type==0)
                            {
                                BagData.Instance.Switch_Quick(grild_id, target_quick);

                                if (eventData.pointerEnter.gameObject.transform.childCount == 0)
                                {
                                    org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                                    org.gameObject.SetActive(true);
                                    BagViewController.Instance.RemoveBagItem(grild_id);
                                    org.transform.Find("count").gameObject.SetActive(true);
                                }
                                else
                                {
                                    //先设置到快捷栏
                                    org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                                    org.gameObject.SetActive(true);
                                    org.transform.Find("count").gameObject.SetActive(true);

                                    //交换
                                    var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                                    c.SetParent(this.transform, false);
                                    c.transform.Find("count").gameObject.SetActive(true);

                                    BagViewController.Instance.RemoveBagItem(grild_id);
                                    BagViewController.Instance.AddQuickItem(target_quick, org.gameObject);
                                    BagViewController.Instance.AddBagItem(grild_id, c.gameObject);

                                }


                            }
                            else
                            {
                                org.gameObject.SetActive(true);
                            }
                        }
                    }


                }
                else
                {
                    var n = eventData.pointerEnter.gameObject.name;
                    if (n.StartsWith("Prop_"))
                    {
                        var grild_id2 = int.Parse(n.Split("_")[1]);

                        //数据的处理
                        BagData.Instance.Modify_Grild(grild_id, grild_id2, null);

                        if (eventData.pointerEnter.gameObject.transform.childCount == 0)
                        {
                            //换到鼠标拖动的物体上 
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                            org.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            //换到鼠标拖动的物体上 
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                      
                            //org.transform.localPosition = Vector3.zero;

                            //交换
                            var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.SetParent(this.transform, false);
                        }
                    }
                    else
                    {
                        //飞回原位
                        org.gameObject.SetActive(true);
                    }
                }
            }
           //身上装备的格子的操作
            else if (type==1)
            {
                var n = eventData.pointerEnter.gameObject.name;
                if (n.StartsWith("Prop_"))
                {
                    var equip_entity = BagData.Instance.Get_Equip(grild_id);

                    var grild_id2 = int.Parse(n.Split("_")[1]);

                    var bag_entity= BagData.Instance.Get(grild_id2);
                    if (bag_entity == null)
                    {
                        //背包里的这个位置没有物品 脱下这个装备
                        //移除装备
                        BagData.Instance.Remove_Equip(grild_id);
                        BagViewController.Instance.RemoveEquipItem(grild_id);

                        org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                        org.gameObject.SetActive(true);
                        
                        BagData.Instance.AddEquipToBag(equip_entity, grild_id2);//背包数据层的更新
                        BagViewController.Instance.AddBagItem(grild_id2, org.gameObject);//GameObject的缓存维护
                    }
                    else
                    {
                        if (bag_entity.entity.type==1 && bag_entity.entity.part==grild_id)
                        {
                            //移除背包 要交换位置上的物品
                            BagData.Instance.Remove_Grild_ID(grild_id2, bag_entity.count, null); ;
                            BagViewController.Instance.RemoveBagItem(grild_id2);

                            //装备移除
                            BagData.Instance.Remove_Equip(grild_id);
                            BagViewController.Instance.RemoveEquipItem(grild_id);

                            
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.transform.Find("count").gameObject.SetActive(true);
                            org.gameObject.SetActive(true);

                            BagData.Instance.AddEquipToBag(equip_entity, grild_id2);
                            BagViewController.Instance.AddBagItem(grild_id2, org.gameObject);//刷新背包的缓存

                            var c= eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.transform.SetParent(this.transform,false);
                            c.gameObject.SetActive(true);
                            c.transform.Find("count").gameObject.SetActive(false);

                            //刷新装备数据缓存
                            BagData.Instance.Add_Equip(bag_entity.entity.part, bag_entity);
                            //equip_dct
                            BagViewController.Instance.AddEquipItem(grild_id, c.gameObject);
                        }
                        else
                        {
                            //不相等的情况 
                            //脱下这个装备 然后放到背包内其他空格上
                            //背包是否满的状态
                            if (BagData.Instance.IsMax())
                            {
                                org.gameObject.SetActive(true);
                            }
                            else
                            {
                                //移除背包 要交换位置上的物品
                                //BagData.Instance.Remove_Grild_ID(grild_id2, bag_entity.count, null); 
                                BagData.Instance.Remove_Equip(grild_id);  //装备数据缓存更新
                                BagViewController.Instance.RemoveEquipItem(grild_id);//装备的GameObject的缓存

                                //grild
                                var grild_next= BagData.Instance.GetGrildId_Next();
                                BagViewController.Instance.AddBagItem(grild_next, org.gameObject);
                                BagData.Instance.AddEquipToBag(equip_entity, grild_next);

                                var parent = BagViewController.Instance.GetGrild(grild_next);
                                org.transform.SetParent(parent,false);
                                org.gameObject.SetActive(true);
                            }

                        }
                    }

                }
                else
                {
                    org.gameObject.SetActive(true);
                }
            }
            else if (type==2)//快捷栏 到背包
            {
                var n = eventData.pointerEnter.gameObject.name;
                if (n.StartsWith("Prop_"))
                {
                    var quick_entity = BagData.Instance.Get_Quick(grild_id);
                    //背包的格子ID
                    var bag_grild_id = int.Parse(n.Split("_")[1]);

                    var bag_entity = BagData.Instance.Get(bag_grild_id);
                    if (bag_entity == null)
                    {
                        BagData.Instance.Remove_Quick(grild_id);
                        BagViewController.Instance.RemoveQuickItem(grild_id);

                        org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                        org.gameObject.SetActive(true);

                        BagData.Instance.AddQuickToBag(quick_entity, bag_grild_id);//背包数据层的更新
                        BagViewController.Instance.AddBagItem(bag_grild_id, org.gameObject);//GameObject的缓存维护
                    }
                    else
                    {
                        //如果快捷栏的物品  跟 背包指定格子的物品 类型都是0 那么执行交换逻辑
                        if (bag_entity.entity.type == 0)
                        {
                            //移除背包 要交换位置上的物品
                            BagData.Instance.Remove_Grild_ID(bag_grild_id, bag_entity.count, null); ;
                            BagViewController.Instance.RemoveBagItem(bag_grild_id);

                            //移除快捷栏物品
                            BagData.Instance.Remove_Quick(grild_id);
                            BagViewController.Instance.RemoveQuickItem(grild_id);


                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                            org.transform.Find("count").gameObject.SetActive(true);

                            BagData.Instance.AddQuickToBag(quick_entity, bag_grild_id);
                            BagViewController.Instance.AddBagItem(bag_grild_id, org.gameObject);//刷新背包的缓存

                            var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.transform.SetParent(this.transform, false);
                            c.gameObject.SetActive(true);
                            c.transform.Find("count").gameObject.SetActive(true);

                            BagData.Instance.Add_Quick(bag_entity.entity.part, bag_entity);
                            BagViewController.Instance.AddQuickItem(grild_id, c.gameObject);
                        }
                        else
                        {
                            //不相等的情况 
                            //脱下这个装备 然后放到背包内其他空格上
                            //背包是否满的状态
                            if (BagData.Instance.IsMax())
                            {
                                org.gameObject.SetActive(true);
                            }
                            else
                            {
                                BagData.Instance.Remove_Quick(grild_id);  
                                BagViewController.Instance.RemoveQuickItem(grild_id);//装备的GameObject的缓存

                                //grild
                                var grild_next = BagData.Instance.GetGrildId_Next();
                                BagViewController.Instance.AddBagItem(grild_next, org.gameObject);
                                BagData.Instance.AddQuickToBag(quick_entity, grild_next);

                                var parent = BagViewController.Instance.GetGrild(grild_next);
                                org.transform.SetParent(parent, false);
                                org.gameObject.SetActive(true);
                            }

                        }
                    }

                }
                else
                {
                    org.gameObject.SetActive(true);
                }

            }
            org = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            BagViewController.Instance.OnPointerExit_Grild(grild_id, eventData);
            BagViewController.Instance.view.SelectObj = null;
            return;
        }
        else {
            if (type==0)
            {
                BagViewController.Instance.OnPointerEnter_Grild(grild_id, eventData,type);
                BagViewController.Instance.view.SelectObj = eventData.pointerEnter.gameObject;
            }else if (type==1)
            {
                BagViewController.Instance.OnPointerEnter_Grild(grild_id, eventData, type);
                BagViewController.Instance.view.SelectObj = null;
            }
            else if (type == 2)
            {
                BagViewController.Instance.OnPointerEnter_Grild(grild_id, eventData, type);
                BagViewController.Instance.view.SelectObj = null;
            }
        }
   
        //BagViewController.Instance.view.ShowPropInfo(grild_id, eventData.pointerEnter.transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BagViewController.Instance.OnPointerExit_Grild(grild_id, eventData);

        BagViewController.Instance.view.SelectObj = null;
        //BagViewController.Instance.view.ClosePropInfo();
    }
}
