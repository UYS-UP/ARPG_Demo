using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItem : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    //��¼��ǰ�ĸ���ID 
    public int grild_id;
    Transform org;

    public int type;//0��ʾ������ĸ��� 1���ϵ�װ���ĸ���
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
    //����ʼ��ק
    public void OnBeginDrag(PointerEventData eventData)
    {
        //����һ�� ��Ʒ
        //Debug.LogError("��ʼ�϶�:"+eventData.pointerEnter.gameObject.name);

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

    //������ק��
    public void OnDrag(PointerEventData eventData)
    {
        //���������Ʒ��λ��
        //Debug.LogError("�϶���:" + eventData.pointerEnter.gameObject.name);
        if (temp!=null)
        {
            temp.transform.position=eventData.position;
        }
    }

    //��������ק
    public void OnEndDrag(PointerEventData eventData)
    {
        //����λ��: �п��ܽ�����λ�� ��Ʒ�ǿ� ���߲��ǿ�
        //Debug.LogError("������ק:" + eventData.pointerEnter.gameObject.name);
        if (temp!=null)
        {
            temp.gameObject.SetActive(false);
            ResourcesManager.Instance.Destroy(temp);
            temp = null;

            //�����ĸ��Ӳ���
            if (type==0)
            {
               
                if (eventData.pointerEnter.gameObject.CompareTag("Equip"))
                { 
                    //��������װ�������߼�
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

                                    //����
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
                    //�����������
                    //��������װ�������߼�
                    var n = eventData.pointerEnter.gameObject.name;
                    if (n.StartsWith("Prop_"))
                    {
                        //1234
                        var target_quick = int.Parse(n.Split("_")[1]);

                        //��ȡ��������
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
                                    //�����õ������
                                    org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                                    org.gameObject.SetActive(true);
                                    org.transform.Find("count").gameObject.SetActive(true);

                                    //����
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

                        //���ݵĴ���
                        BagData.Instance.Modify_Grild(grild_id, grild_id2, null);

                        if (eventData.pointerEnter.gameObject.transform.childCount == 0)
                        {
                            //��������϶��������� 
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                            org.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            //��������϶��������� 
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                      
                            //org.transform.localPosition = Vector3.zero;

                            //����
                            var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.SetParent(this.transform, false);
                        }
                    }
                    else
                    {
                        //�ɻ�ԭλ
                        org.gameObject.SetActive(true);
                    }
                }
            }
           //����װ���ĸ��ӵĲ���
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
                        //����������λ��û����Ʒ �������װ��
                        //�Ƴ�װ��
                        BagData.Instance.Remove_Equip(grild_id);
                        BagViewController.Instance.RemoveEquipItem(grild_id);

                        org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                        org.gameObject.SetActive(true);
                        
                        BagData.Instance.AddEquipToBag(equip_entity, grild_id2);//�������ݲ�ĸ���
                        BagViewController.Instance.AddBagItem(grild_id2, org.gameObject);//GameObject�Ļ���ά��
                    }
                    else
                    {
                        if (bag_entity.entity.type==1 && bag_entity.entity.part==grild_id)
                        {
                            //�Ƴ����� Ҫ����λ���ϵ���Ʒ
                            BagData.Instance.Remove_Grild_ID(grild_id2, bag_entity.count, null); ;
                            BagViewController.Instance.RemoveBagItem(grild_id2);

                            //װ���Ƴ�
                            BagData.Instance.Remove_Equip(grild_id);
                            BagViewController.Instance.RemoveEquipItem(grild_id);

                            
                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.transform.Find("count").gameObject.SetActive(true);
                            org.gameObject.SetActive(true);

                            BagData.Instance.AddEquipToBag(equip_entity, grild_id2);
                            BagViewController.Instance.AddBagItem(grild_id2, org.gameObject);//ˢ�±����Ļ���

                            var c= eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.transform.SetParent(this.transform,false);
                            c.gameObject.SetActive(true);
                            c.transform.Find("count").gameObject.SetActive(false);

                            //ˢ��װ�����ݻ���
                            BagData.Instance.Add_Equip(bag_entity.entity.part, bag_entity);
                            //equip_dct
                            BagViewController.Instance.AddEquipItem(grild_id, c.gameObject);
                        }
                        else
                        {
                            //����ȵ���� 
                            //�������װ�� Ȼ��ŵ������������ո���
                            //�����Ƿ�����״̬
                            if (BagData.Instance.IsMax())
                            {
                                org.gameObject.SetActive(true);
                            }
                            else
                            {
                                //�Ƴ����� Ҫ����λ���ϵ���Ʒ
                                //BagData.Instance.Remove_Grild_ID(grild_id2, bag_entity.count, null); 
                                BagData.Instance.Remove_Equip(grild_id);  //װ�����ݻ������
                                BagViewController.Instance.RemoveEquipItem(grild_id);//װ����GameObject�Ļ���

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
            else if (type==2)//����� ������
            {
                var n = eventData.pointerEnter.gameObject.name;
                if (n.StartsWith("Prop_"))
                {
                    var quick_entity = BagData.Instance.Get_Quick(grild_id);
                    //�����ĸ���ID
                    var bag_grild_id = int.Parse(n.Split("_")[1]);

                    var bag_entity = BagData.Instance.Get(bag_grild_id);
                    if (bag_entity == null)
                    {
                        BagData.Instance.Remove_Quick(grild_id);
                        BagViewController.Instance.RemoveQuickItem(grild_id);

                        org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                        org.gameObject.SetActive(true);

                        BagData.Instance.AddQuickToBag(quick_entity, bag_grild_id);//�������ݲ�ĸ���
                        BagViewController.Instance.AddBagItem(bag_grild_id, org.gameObject);//GameObject�Ļ���ά��
                    }
                    else
                    {
                        //������������Ʒ  �� ����ָ�����ӵ���Ʒ ���Ͷ���0 ��ôִ�н����߼�
                        if (bag_entity.entity.type == 0)
                        {
                            //�Ƴ����� Ҫ����λ���ϵ���Ʒ
                            BagData.Instance.Remove_Grild_ID(bag_grild_id, bag_entity.count, null); ;
                            BagViewController.Instance.RemoveBagItem(bag_grild_id);

                            //�Ƴ��������Ʒ
                            BagData.Instance.Remove_Quick(grild_id);
                            BagViewController.Instance.RemoveQuickItem(grild_id);


                            org.transform.SetParent(eventData.pointerEnter.gameObject.transform, false);
                            org.gameObject.SetActive(true);
                            org.transform.Find("count").gameObject.SetActive(true);

                            BagData.Instance.AddQuickToBag(quick_entity, bag_grild_id);
                            BagViewController.Instance.AddBagItem(bag_grild_id, org.gameObject);//ˢ�±����Ļ���

                            var c = eventData.pointerEnter.gameObject.transform.GetChild(0);
                            c.transform.SetParent(this.transform, false);
                            c.gameObject.SetActive(true);
                            c.transform.Find("count").gameObject.SetActive(true);

                            BagData.Instance.Add_Quick(bag_entity.entity.part, bag_entity);
                            BagViewController.Instance.AddQuickItem(grild_id, c.gameObject);
                        }
                        else
                        {
                            //����ȵ���� 
                            //�������װ�� Ȼ��ŵ������������ո���
                            //�����Ƿ�����״̬
                            if (BagData.Instance.IsMax())
                            {
                                org.gameObject.SetActive(true);
                            }
                            else
                            {
                                BagData.Instance.Remove_Quick(grild_id);  
                                BagViewController.Instance.RemoveQuickItem(grild_id);//װ����GameObject�Ļ���

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
