using System;
using System.Collections;
using System.Collections.Generic;

using Game.Config;
using Google.Protobuf.GS;
using UnityEngine;
public class BagData
{
    static BagData instance = new BagData();
    public static BagData Instance => instance;

    public int money = 5000;


    //ÿһ�����ӵ���Ʒ��Ϣ
    public Dictionary<int, BagEntity> dct = new Dictionary<int, BagEntity>();
    public int max = 54;
    public void Reset() {
        dct.Clear();
    }

    public void SortByType()
    {
        List<BagEntity> prop = new List<BagEntity>();
        List<BagEntity> equip = new List<BagEntity>();
        List<BagEntity> mat = new List<BagEntity>();

        foreach (var item in dct.Values)
        {
            if (item.entity.type == 0)
            {
                prop.Add(item);
            }
            else if (item.entity.type == 1)
            {
                equip.Add(item);
            }
            else if (item.entity.type == 2) {
                mat.Add(item);
            }
        }

        dct.Clear();

        for (int i = 0; i < prop.Count; i++)
        {
            var item= prop[i];
            item.grild_id = i;
            dct[item.grild_id] = item;
        }

        for (int i = 0; i < equip.Count; i++)
        {
            var item = equip[i];
            item.grild_id = i+ prop.Count;
            dct[item.grild_id] = item;
        }
        int offset = prop.Count + equip.Count;
        for (int i = 0; i < mat.Count; i++)
        {
            var item = mat[i];
            item.grild_id = i+ offset;
            dct[item.grild_id] = item;
        }

    }

    public void AddEquipToBag(BagEntity e,int grild)
    {
        e.grild_id = grild;
        dct[e.grild_id] = e;
    }



    public void AddQuickToBag(BagEntity e, int grild)
    {
        e.grild_id = grild;
        dct[e.grild_id] = e;
    }

    public void Add(int id,int count,Action<BagEntity> callback,Action<int> error=null) {
        if (dct.Count>=max) { error?.Invoke(0); return; }

        int grild = -1;
        foreach (var item in dct)
        {
            if (item.Value.entity.id==id)
            {
                grild=item.Value.grild_id;
                break;
            }
        }

        //������Ƿ������ͬ����Ʒ  �������  ��ֱ�ӵ�����������
        foreach (var item in quick_dct)
        {
            if (item.Value.entity.id==id)
            {
                item.Value.count += count;
                callback?.Invoke(item.Value);
                return;
            }
        }

        if (dct.ContainsKey(grild) && dct[grild].entity.superposition > 0)
        {
            dct[grild].count += count;
            callback?.Invoke(dct[grild]);
        }
        else
        {
            var e= PropData.Get(id);
            if (e.superposition==0)//���������
            {
                for (int j = 0; j < count; j++)
                {
                    if (dct.Count >= max) { error?.Invoke(0); return; }

                    BagEntity bagEntity = new BagEntity();
                    bagEntity.id = id;
                    bagEntity.count = 1;
                    bagEntity.entity = e;
                    bagEntity.SetAtt();

                    //�Ҹ��ӵ��߼�
                    for (int i = 0; i < max; i++)
                    {
                        if (dct.ContainsKey(i))
                        {
                            continue;
                        }
                        else
                        {
                            bagEntity.grild_id = i;
                            dct[i] = bagEntity;
                            callback?.Invoke(bagEntity);
                            break;
                        }
                    }
                }
            }
            else if (e.superposition==1)
            {
                BagEntity bagEntity = new BagEntity();
                bagEntity.id = id;
                //bagEntity.grild_id = dct.Count;
                bagEntity.count = count;
                bagEntity.entity = PropData.Get(id);
                bagEntity.SetAtt();

                for (int i = 0; i < max; i++)
                {
                    if (dct.ContainsKey(i))
                    {
                        continue;
                    }
                    else
                    {
                        bagEntity.grild_id = i;
                        dct[i] = bagEntity;
                        callback?.Invoke(bagEntity);
                        return;
                    }
                }
            }
          
        }
    }


    //���ݸ���ID�����Ƴ�
    public void Remove_Grild_ID(int grild_id, int count, Action<BagEntity> callback) {
        if (dct.ContainsKey(grild_id) == false)
        {
            return;
        }

        dct.Remove(grild_id);
        return;
        //var e = dct[grild_id];
        //if (e.count >= count)
        //{
        //    e.count -= count;
        //    if (e.count == 0)
        //    {
        //        dct.Remove(grild_id);
        //    }
        //    callback?.Invoke(e);
        //}
        //else
        //{
        //    Debug.LogError($"�Ƴ���������:{grild_id}  {e.id}   {e.count}");
        //}
    }

    //������ƷID�����Ƴ�
    public void Remove_Prop_Id(int id, int count, Action<BagEntity> callback)
    {
        var loop_count = PropData.Get(id).superposition == 0 ? count : 1;
        for (int i = 0; i < loop_count; i++)
        {
            int grild_id = -1;
            foreach (var item in dct)
            {
                if (item.Value.entity.id == id)
                {
                    grild_id = item.Value.grild_id;
                    break;
                }

            }


            if (grild_id != -1)
            {
                //Remove_Grild_ID(grild_id, count, callback);
                if (dct.ContainsKey(grild_id) == false)
                {
                    break;
                }


                var e = dct[grild_id];
                if (e.count >= count)
                {
                    e.count -= count;
                    if (e.count == 0)
                    {
                        dct.Remove(grild_id);
                    }
                    callback?.Invoke(e);
                }
                else
                {
                    Debug.LogError($"�Ƴ���������:{grild_id}  {e.id}   {e.count}");
                }
            }
        }
    }

    //������ƷID ����ָ������
    public int Query(int id) {
        int count = 0;
        foreach (var item in dct)
        {
            if (item.Value.entity.id==id)
            {
                //��Ʒʵ�ʿ��Զѵ�
                count += item.Value.count;
            }
        }
        return count;
    }

    public BagEntity Get(int grild_id) {
        if (dct.TryGetValue(grild_id,out var e))
        {
            return e;
        }
        return null;
    }

    public void Modify_Grild(int grild_id_1, int grild_id_2, Action<BagEntity, BagEntity> callback)
    {
        var entity1 = Get(grild_id_1);
        var entity2 = Get(grild_id_2);
        if (entity1==null)
        {
            dct.Remove(grild_id_2);
            if (entity2 == null)
            {
                dct.Remove(grild_id_1);
            }
            else
            {
                dct[grild_id_1] = entity2;
                entity2.grild_id = grild_id_1;
            }
        }
        else
        {
            if (entity2 == null)
            {
                dct.Remove(grild_id_1);
                dct[grild_id_2] = entity1;
                entity1.grild_id = grild_id_2;
            }
            else
            {
                dct[grild_id_1] = entity2;
                dct[grild_id_2] = entity1;
                entity1.grild_id = grild_id_2;
                entity2.grild_id = grild_id_1;
            }
        }
        callback?.Invoke(entity1, entity2);
    }


    //��Ʒλ�õĵ���
    public void Modify_Grild(BagEntity entity1,BagEntity entity2,Action<BagEntity, BagEntity> callback) {
        dct[entity1.grild_id] = entity2;
        dct[entity2.grild_id] = entity1;
        entity1.grild_id = entity2.grild_id;
        entity2.grild_id = entity1.grild_id;
        callback?.Invoke(entity1,entity2);
    }


    //װ������
    public Dictionary<int, BagEntity> equip_dct = new Dictionary<int, BagEntity>();

    public void Add_Equip(int part, BagEntity bagEntity)
    {
        if (equip_dct.ContainsKey(part)==false)
        {
            bagEntity.grild_id = part;
            equip_dct[part] = bagEntity;
        }
    }

    public void Remove_Equip(int part) {
        if (equip_dct.ContainsKey(part)) {
            equip_dct.Remove(part);
        }
    }

    public BagEntity Get_Equip(int part) {
        if (equip_dct.TryGetValue(part,out var e))
        {
            return e;
        }
        return null;
    }

    //����  �ѱ�����Ҫװ���������Ƴ�,װ������ӽӿ�==��λ��(��)����,�Ƴ���,���������
    public void Switch_Equip(int grild_id) {
       var bag_entity= Get(grild_id);
        if (bag_entity!=null)
        {
            if (bag_entity.entity.type==1)
            {
                if (equip_dct.ContainsKey(bag_entity.entity.part))
                {
                    var equip_entity= equip_dct[bag_entity.entity.part];
                    Remove_Equip(bag_entity.entity.part);

                    Add_Equip(bag_entity.entity.part, bag_entity);
                    Remove_Grild_ID(grild_id,1,null);;
                    AddEquipToBag(equip_entity, grild_id);
                }
                else
                {
                    //��δ���������λ��װ��
                    Add_Equip(bag_entity.entity.part, bag_entity);
                    Remove_Grild_ID(grild_id, 1, null);
                }
            }
        }  
    }


    public void Switch_Quick(int grild_id,int quick_grild)
    {
        var bag_entity = Get(grild_id);
        if (bag_entity != null)
        {
            if (bag_entity.entity.type == 0)
            {
                if (quick_dct.ContainsKey(quick_grild))
                {
                    var entity = quick_dct[quick_grild];
                    Remove_Quick(quick_grild);

                    Add_Quick(quick_grild, bag_entity);
                    Remove_Grild_ID(grild_id, 1, null); ;
                    AddQuickToBag(entity, grild_id);
                }
                else
                {
                    Add_Quick(quick_grild, bag_entity);
                    Remove_Grild_ID(grild_id, 1, null);
                }
            }
        }
    }


    internal bool IsMax()
    {
        return dct.Count >= max;
    }

    internal int GetGrildId_Next()
    {
        for (int i = 0; i < max; i++)
        {
            if (dct.ContainsKey(i)==false)
            {
                return i;
            }
        }
        return -1;
    }



    //�����������:key Լ��:1234
    public Dictionary<int, BagEntity> quick_dct = new Dictionary<int, BagEntity>();


    public void Add_Quick(int grild_id, BagEntity bagEntity)
    {
        if (quick_dct.ContainsKey(grild_id) == false)
        {
            bagEntity.grild_id = grild_id;
            quick_dct[grild_id] = bagEntity;
        }
    }

    public void Remove_Quick(int grild_id)
    {
        if (quick_dct.ContainsKey(grild_id))
        {
            quick_dct.Remove(grild_id);
        }
    }

    public BagEntity Get_Quick(int grild_id)
    {
        if (quick_dct.TryGetValue(grild_id, out var e))
        {
            return e;
        }
        return null;
    }

    /// <summary>
    /// �����Ƿ�����
    /// </summary>
    /// <param name="id">׼����ӵ���ƷID</param>
    /// <param name="count">׼����ӵ�����</param>
    /// <returns></returns>
    public bool IsFull(int id,int count) {

        var e = PropData.Get(id);//��Ʒ����
        //������Ե���
        if (e.superposition > 0) {
            if (dct.Count < max) {
                return false;
            }
            else
            {
                //С��0 ��ʾ����û�и��ӿ��Ա�ռ�� ���Է���true��ʾ��������
                return Query(id) < 0;
            }
        }
        else
        {
            //��Ʒ����+Ҫ��ӵ�����  ��������������
            if (dct.Count+ count <= max) {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void Recover(SaveInfoMain saveInfoMain)
    {
        dct.Clear();
        foreach (var item in saveInfoMain.Grilds)
        {
            BagEntity entity = new BagEntity();
            entity.id = item.Value.PropId;
            entity.grild_id = item.Key;
            entity.count = item.Value.Count;
            entity.entity = PropData.Get(item.Value.PropId);
            dct[item.Key] = entity;
        }
    }
}



public class BagEntity
{
    public int id;
    public int grild_id;//�ڱ����еĸ���ID
    public int count;//��ǰ�����Ʒ������
    public PropEntity entity;

    //����1 2 3 4 5
    public int[] att1, att2, att3,att4,att5;
    public void SetAtt() {
        if (entity.type==1)
        {
            CreateAtt(entity.att1,ref att1);
            CreateAtt(entity.att2,ref att2);
            CreateAtt(entity.att3,ref att3);
            CreateAtt(entity.att4,ref att4);
            CreateAtt(entity.att5,ref att5);
        }
    }

    private void CreateAtt(int[] org,ref int[] target)
    {
        if (org != null)
        {
            target = new int[3] { org[0], org[1], org[2] };
        }
        else
        {
            target = null;
        }
    }

    public void DO_Random_Att() {
        AttRandom(att1);
        AttRandom(att2);
        AttRandom(att3);
        AttRandom(att4);
        AttRandom(att5);
    }

    void AttRandom(int[] org) {
        if (org!=null)
        {
            org[0] = IntEx.Range(1, 8);
            var f = (int)(org[1] * 0.2);
            org[1] = IntEx.Range(org[1]- f, org[1]+f);
            //org[2] = IntEx.Range(1, 8);
        }
    }
}