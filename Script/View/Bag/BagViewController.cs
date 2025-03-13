using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagViewController : ViewController<BagViewController, BagView>
{
    internal void OnPointerEnter_Grild(int grild_id, PointerEventData eventData,int type)
    {
        BagEntity e = null;
        if (type==0)
        {
            e = BagData.Instance.Get(grild_id);
        }
        else if (type==1) { e = BagData.Instance.Get_Equip(grild_id); }
        else if (type==2) { e = BagData.Instance.Get_Quick(grild_id); }   

        //var e= type==0? BagData.Instance.Get(grild_id):BagData.Instance.Get_Equip(grild_id);
        if (e != null) {
            if (e.entity.type == 0)
            {
                view.ShowPropInfo(grild_id, eventData.position, e);
            }
            else if (e.entity.type==1)
            {
                view.ShowEquipInfo(grild_id, eventData.position, e);
            }
            else if (e.entity.type == 2)
            {
                view.ShowMaterialInfo(grild_id,eventData.position, e); ;
            }
        }
    }

    internal void OnPointerExit_Grild(int grild_id, PointerEventData eventData)
    {
        view.CloseDescribe();
    }

    internal void RemoveBagItem(int grild_id)
    {
       view.RemoveBagItem(grild_id);
    }

  
    internal void AddBagItem(int grild_id, GameObject obj)
    {
        view.AddBagItem(grild_id, obj);
    }


    internal void AddEquipItem(int part_type, GameObject obj)
    {
        view.Add_Euqip_Item(part_type, obj);
    }


    internal void RemoveEquipItem(int grild_id)
    {
        view.Remove_Euqip_Item(grild_id);
    }

    internal Transform GetGrild(int grild_next)
    {
        //if (IsOpen())
        //{
        //    Open();
        //}
       return  view.GetGrild(grild_next);
    }


    internal void AddQuickItem(int part_type, GameObject obj)
    {
        view.Add_Quick_Item(part_type, obj);
    }


    internal void RemoveQuickItem(int grild_id)
    {
        view.Remove_Quick_Item(grild_id);
    }

}
