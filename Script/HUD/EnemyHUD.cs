using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    public Image hp_top;//最顶层
    public Image hp_middle;//中间部分的血条
    public float middle_speed = 1;//插值速度
    public float hp = -1;//血量
    public Transform target;
    public Text name_text;//昵称
    public Vector3 offset = new Vector3(0, 1.8f, 0);

    void Awake()
    {
        hp_top = transform.Find("HealthBar/HP_Top").GetComponent<Image>();
        hp_middle = transform.Find("HealthBar/HP_Middle").GetComponent<Image>();
        name_text = transform.Find("Name").GetComponent<Text>();
    }

    
    void Update()
    {
        DOUpdateHP();
        if (hp<=0)
        {
          if (hp_top.fillAmount==0&&hp_middle.fillAmount==0) {
                this.gameObject.SetActive(false);
                ResourcesManager.Instance.DestroyEnemyHUD(this);
            }
        }

        if (target!=null)
        {
            this.transform.position = target.position + offset;
            this.transform.rotation = GameDefine._Camera.transform.rotation;
        }
    }
    bool _do_update;
    public void UpdateHP(float v,Transform target,string name) {
        _do_update = true;
        hp = v;
        this.target = target;
        name_text.text = name;
        if (v>0) { this.gameObject.SetActive(true); }
    }

    private void DOUpdateHP()
    {
        if (hp == -1|| _do_update==false) {
            return;
        }

        if (hp_top.fillAmount>hp)
        {
            //减少
            hp_top.SetFillAmount(hp, middle_speed * 5);
        }
        else if (hp_top.fillAmount < hp)
        {
            //增加它
            hp_top.SetFillAmount(hp, middle_speed);
        }

        if (hp_middle.fillAmount > hp)
        {
            hp_middle.SetFillAmount(hp, middle_speed);
        }
        else if (hp_middle.fillAmount < hp)
        {
            hp_middle.SetFillAmount(hp, middle_speed * 5);
        }

        if (_do_update)
        {
            if (hp_top.fillAmount==hp&&hp_middle.fillAmount==hp)
            {
                _do_update = false;
            }
        }
    }
}
