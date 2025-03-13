using System.Collections;
using System.Collections.Generic;

using Game.Config;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StoreView : View
{
    Transform store_item_parent;
    Text Coin_Text;
    InputField Count_InputField;
    Transform Select;
    Button Buy;
    public override void Awake()
    {
        base.Awake();
        Select = transform.Find("Select");
        store_item_parent = transform.Find("Bag/Scroll View/Viewport/Content");
        for (int i = 0; i < store_item_parent.childCount; i++)
        {
            var btn= store_item_parent.GetChild(i).GetComponent<Button>();
            btn.onClick.AddListener(() => {
                select_id = int.Parse(btn.transform.GetChild(0).name);
                Count_InputField.text = "1";
                Coin_Text.text = "�ܼ�:"+StoreData.Get(select_id).price.ToString();
                Select.transform.position = btn.transform.position;
                Select.gameObject.SetActive(true);
            });
        }
        
        Count_InputField = GetComponent<InputField>("Count_InputField");
        Coin_Text = GetComponent<Text>("Coin_Text");

        Count_InputField.onValueChanged.AddListener(Count_InputField_OnValueChanged);

        Buy = GetComponent<Button>("Buy");
        Buy.onClick.AddListener(BuyOnClick);
    }

    private void BuyOnClick()
    {
        if (select_id!=-1)
        {
           

            if (int.TryParse(Count_InputField.text, out var count)) {

                if (BagData.Instance.IsFull(select_id, count))
                {
                    TipsViewController.Instance.Show($"��������,�޷�����!");
                    return;
                }

                var price = (StoreData.Get(select_id).price * count);
                if (BagData.Instance.money >= price)
                {
                    //��ʾ����ɹ� ��ö��ٸ�XX��Ʒ
                    TipsViewController.Instance.Show($"����ɹ�,���:<color=red>{PropData.Get(select_id).name}</color>*{Count_InputField.text}");
                    //�����������Ʒ
                    BagData.Instance.Add(select_id, count, null, null);
                    BagData.Instance.money -= price;
                }
                else
                {
                    TipsViewController.Instance.Show("��Ҳ���,����ʧ��!");
                }
            }
            else
            {
                TipsViewController.Instance.Show("���������쳣,����������!");
            }
          
        }
        else
        {
            TipsViewController.Instance.Show("��ѡ����Ҫ�������Ʒ");
        }

        //�жϱ����Ƿ��㹻������
        //�������㹻 ����û�е�������
        //�Ѿ����� ��Ҫ�ж���Ʒ�Ƿ���Ե���?���ԵĻ�,���ұ������������Ʒ 
    }

   

    private void Count_InputField_OnValueChanged(string arg0)
    {
        if (int.TryParse(arg0, out var result))
        {
            if (result>0)
            {
                Coin_Text.text =$"�ܼ�:{(StoreData.Get(select_id).price * result).ToString()}";
            }
            else if (result<=0)
            {
                Count_InputField.text = "1";
            }
        } 
    }

    //�����ʱ��


    public int select_id=-1;

    public override void OnEnable()
    {
        base.OnEnable();
        select_id = -1;
        Coin_Text.text = "�ܼ�:0";
        Select.gameObject.SetActive(false);
        Show();
    }

    public void Show() {

        for (int i = 0; i < store_item_parent.childCount; i++)
        {
            var obj=store_item_parent.GetChild(i).gameObject;
            if (obj.transform.childCount>0)
            {
                ResourcesManager.Instance.Destroy_BagItem(obj.transform.GetChild(0).gameObject);
            }
        }

        //��ʾʮ�������۵ĵ���
        for (int i = 0; i < 12; i++)
        {
           var index = IntEx.Range(0, StoreData.all.Count-1);
           var data= StoreData.all.ElementAt(index).Value;
            //Debug.LogError($"��ƷID:{data.id}   {index}");
           var obj= ResourcesManager.Instance.CreatePropItem(data.id,999);
            obj.gameObject.name = data.id.ToString();//�̵����ñ��ID ʵ����Ҳ����Ʒ��ID
            //obj.transform.Find("count")
            obj.transform.SetParent(store_item_parent.GetChild(i), false);
        }
    }
}
