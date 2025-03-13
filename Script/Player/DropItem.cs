using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public class DropItem : MonoBehaviour
{
    Vector3 pos;
    // Start is called before the first frame update
    public int id;
    float begin;
    void Start()
    {
        begin = Time.time;
        transform.localScale= Vector3.one*0.5f;
        pos = this.transform.position;
    }
    int state = 0;//待机 1飞行中 2已被拾取
    // Update is called once per frame
    void Update()
    {
        if (state == 0) {
            if (begin > 0 && Time.time - begin > 1f)
            {
                if (UnitManager.Instance.player != null)
                {
                    if (Vector3.Distance(pos, UnitManager.Instance.player._transform.position) <= 1.5f)
                    {
                        state = 1;
                        //transform.DOMove(UnitManager.Instance.player._transform.position + new Vector3(0, 1f, 0), 0.35f).OnComplete(() => {
                        //    BagData.Instance.Add(id, 1, null);
                        //    this.gameObject.SetActive(false);
                        //});
                       

                    }

                }
            }
        }
        else if (state==1)
        {
            transform.forward = UnitManager.Instance.player._transform.position + Vector3.up - this.transform.position;
            transform.position += transform.forward * 20 * Time.deltaTime;
            if (Vector3.Distance(this.transform.position, UnitManager.Instance.player._transform.position + Vector3.up) < 0.3f)
            {

                BagData.Instance.Add(id, 1, OnGetProp);
                //this.enabled = false;
                state = 2;
                this.gameObject.SetActive(false);
            }
        }
     
       
    }

    public void OnGetProp(BagEntity e) {
        MainViewController.Instance.OnGetProp($"获得 {e.entity.name} *1");

    }
}
