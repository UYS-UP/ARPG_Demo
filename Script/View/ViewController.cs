using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController <T,V> where T:new () where V:View,new()
{
    static T instance = new T();
    public static T Instance => instance;
    bool open_nav_view;
    public V view;
    public void Init(string resPath, bool isAddUpdateListerner = false, bool stopAI_OnOpen = false,bool open_nav=false, bool cursorEnable = true) {
        view = new V();
        view.Init(resPath, isAddUpdateListerner,Close, stopAI_OnOpen, cursorEnable);
        this.open_nav_view = open_nav;
        GameEvent.ResetSortOrder += ResetSortOrder;
        ViewManager.Instance.CloseAll += Close;
        AddListener();
    }

    private void ResetSortOrder()
    {
        if (view!=null&&view._enable)
        {
            var o = view.canvas.sortingOrder - 10000;
            o = Mathf.Clamp(o, -30000, 30000);
            view.canvas.sortingOrder = o;
        }
    }

    public void SetNavToTop() {
        if (view != null && view._enable) {
            view.canvas.sortingOrder = 30001;
        }
    }

    public void SetTipsToTop() {
        if (view != null && view._enable)
        {
            view.canvas.sortingOrder = 30002;
        }

    }

    public virtual void AddListener() { 
    
        
    }

    public virtual void RemoveListener()
    {


    }
   
    public bool IsOpen() {
        if (view._enable)
        {
            return true;
        }
        else
        {
            if (view._close_time==0||(view._close_time != 0&&Time.time - view._close_time >= 0.5f))
            {
                return false;
            }
            return true;
        }
    }

    public void OpenOrClose() {
        if (IsOpen())
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void SetActive(bool act) {

        if (view._enable!=act)
        {
            ViewManager.Instance.OnViewChange_Begin(view.resPath,act, this.open_nav_view);

            view.gameObject.SetActive(act);
            if (act==true)
            {
                view.OnEnable();
                //层级排序
                ViewManager.Instance.order += 1;
                //32,767
                view.canvas.sortingOrder = ViewManager.Instance.order;
                if (ViewManager.Instance.order >= 30000) {

                    ViewManager.Instance.order -= 10000;
                    GameEvent.ResetSortOrder?.Invoke();
                }
                view.SetSize();//自适应  设置分辨率的
                view._enable = true;
            }
            else
            {
                view._enable = false;
                view.OnDisable();
            }

            ViewManager.Instance.OnViewChange_End(view.resPath, act, this.open_nav_view);
        }
    }

    public void Open() {
        if (view.gameObject == null) {
            //实例化
           var go= ResourcesManager.Instance.Instantiate<GameObject>(view.resPath);
            GameObject.DontDestroyOnLoad(go);
            view.gameObject= go;
            view.gameObject.name = view.gameObject.name.Split('(')[0];
            view.transform = go.transform;
            view.canvas=go.GetComponent<Canvas>();
            view.cs = go.GetComponent<CanvasScaler>();

            view.Awake();
            SetActive(true);

            view.Start();
        }
        else
        {
            //打开
            SetActive(true);
        }
    
    }


    public void Close(bool destroy=false) {
        if (view._enable==false)
        {
            return;
        }
        SetActive(false);
        RemoveListener();
    }
}
