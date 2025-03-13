using System.Collections;
using System.Collections.Generic;
using Game.Config;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager 
{
    static ResourcesManager instance = new ResourcesManager();
    public static ResourcesManager Instance => instance;

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T Instantiate<T>(string path) where T : Object {
        var r= Load<T>(path);
        if (r != null) {
            return Object.Instantiate(r);
        }
        return null;
    }

    public void Destroy(GameObject go)
    {
        Object.Destroy(go);
    }

    Stack<EnemyHUD> hud = new Stack<EnemyHUD>(50);
    internal EnemyHUD CreateEnemyHUD()
    {
        if (hud.Count>0)
        {
           return hud.Pop();
        }
        else
        {
            var go = Instantiate<GameObject>("UI/HUD/Enemy_HUD");
            GameObject.DontDestroyOnLoad(go);
            return go.GetComponent<EnemyHUD>();
        }
    }

    public void DestroyEnemyHUD(EnemyHUD enemyHUD)
    {
        enemyHUD.gameObject.SetActive(false);
        hud.Push(enemyHUD);
    }

    internal Sprite LoadIcon(PropEntity entity)
    {
        if (entity.type == 0)
        {
          return  Load<Sprite>(GameDefine.prop_root + entity.icon);
        }
        else if (entity.type == 1)
        {
            return Load<Sprite>(GameDefine.GetEQ_Icon(entity.part) + entity.icon);
        }
        else if (entity.type == 2)
        {
            return Load<Sprite>(GameDefine.mat_root + entity.icon);
        }

        Debug.LogError(entity.id+"  "+  entity.type + "  "+ entity.icon + "   "+ entity.part);

        return null;
    }


    Stack<GameObject> bag = new Stack<GameObject>(100);
    internal void Destroy_BagItem(GameObject o)
    {
        o.gameObject.SetActive(false);
        o.transform.SetParent(null,false);
        bag.Push(o);
    }

    public GameObject Create_BagItem(string item_path) {
        if (bag.Count == 0)
        {
            var obj = Instantiate<GameObject>(item_path);
            GameObject.DontDestroyOnLoad(obj);
            return obj;
        }
        else
        {
            var obj=bag.Pop();
            obj.SetActive(true);
            return obj;
        }
    }

    string item_path = "UI/Item/Prop_Item";
    public GameObject CreatePropItem(int id, int count)
    {
        var obj = Create_BagItem(item_path);
        var icon = obj.transform.Find("icon").GetComponent<Image>();
        var entity = PropData.Get(id);
        icon.sprite = LoadIcon(entity);
        obj.gameObject.name = entity.id.ToString();
        var count_text = obj.transform.Find("count/key_text").GetComponent<Text>();
        count_text.text = count.ToString();
        return obj;
    }

    string forgeItemPath= "UI/Item/Forge_Item";
    internal GameObject CreateForgeItem()
    {
        var obj = Instantiate<GameObject>(forgeItemPath);
        GameObject.DontDestroyOnLoad(obj);
        return obj;
    }

    Dictionary<string, Stack<GameObject>> skill_effect = new Dictionary<string, Stack<GameObject>>();

    public GameObject Create_Skill(string path) {
        if (skill_effect.TryGetValue(path,out var e))
        {
            if (e.Count>0)
            {
                var o= e.Pop();
                o.gameObject.SetActive(true);
                return o;
            }
            else
            {
                var obj= Instantiate<GameObject>(path);
                GameObject.DontDestroyOnLoad(obj);
                return obj;
            }
        }
        else
        {
           

            var obj = Instantiate<GameObject>(path);
            GameObject.DontDestroyOnLoad(obj);
            return obj;
        }
    }
    public void Destroy_Skill(string path,GameObject o)
    {
        if (skill_effect.ContainsKey(path) == false)
        {
            skill_effect[path] = new Stack<GameObject>();
        }
        o.SetActive(false);
        skill_effect[path].Push(o);
    }



    Stack<GameObject> GetPropTips = new Stack<GameObject>(100);
    internal void Destroy_GetPropTips(GameObject o)
    {
        o.gameObject.SetActive(false);
        o.transform.SetParent(null, false);
        GetPropTips.Push(o);
    }
    //Assets/Resources/UI/Item/GetPropTips.prefab
    string GetPropTipsPath = "UI/Item/GetPropTips";
    public GameObject Create_GetPropTips()
    {
        if (GetPropTips.Count == 0)
        {
            var obj = Instantiate<GameObject>(GetPropTipsPath);
            GameObject.DontDestroyOnLoad(obj);
            return obj;
        }
        else
        {
            var obj = GetPropTips.Pop();
            obj.SetActive(true);
            return obj;
        }
    }


}
