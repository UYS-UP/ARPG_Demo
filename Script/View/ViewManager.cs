using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager 
{
    static ViewManager instance = new ViewManager();
    public static ViewManager Instance=>instance;
    public Action<bool> CloseAll;

    public int order;

    public void Init() {

        //注册所有界面
        LoginViewController.Instance.Init("UI/LoginCanvas",true,false,false);
        BagViewController.Instance.Init("UI/BagCanvas", true, false,true);

        ForgeViewController.Instance.Init("UI/ForgeCanvas", true, false, true);

        LoadingViewController.Instance.Init("UI/LoadingCanvas", true, false);
        MainViewController.Instance.Init("UI/MainCanvas", true, false, cursorEnable: false);

        NavViewController.Instance.Init("UI/NavCanvas", true, false);
        PurifyViewController.Instance.Init("UI/PurifyCanvas", true, false, true);
        TipsViewController.Instance.Init("UI/TipsCanvas", true, false);
        StoreViewController.Instance.Init("UI/StoreCanvas", true, false);//神秘商店...
        DialogueViewController.Instance.Init("UI/DialogueView", true, true);

        //其他界面 也都在这里初始化 注册
    }
    
    

    List<string> open_view = new List<string>();

    internal void OnViewChange_Begin(string resPath,bool act,bool open_nav_view)
    {
        //导航栏 打开或者关闭它
        if (open_view.Contains(resPath) == false) { 
            open_view.Add(resPath);
        }

        if (act&&open_nav_view)
        {
            NavViewController.Instance.SetTop();
        }

        if (resPath== "UI/BagCanvas")
        {
            NavViewController.Instance.SetButton_Select(0);
        }
        else if (resPath == "UI/ForgeCanvas")
        {
            NavViewController.Instance.SetButton_Select(1);
        }
        else if (resPath== "UI/PurifyCanvas")
        {
            NavViewController.Instance.SetButton_Select(2);
        }

    }

    internal void OnViewChange_End(string resPath,bool act, bool open_nav_view)
    {
        //导航栏 打开或者关闭它
        if (open_view.Contains(resPath))
        {
            open_view.Remove(resPath);
        }

        if (act==false && open_nav_view)
        {
            NavViewController.Instance.Close();
        }
    }

    public Action OnUpdate;
    public void Update() {
        OnUpdate?.Invoke();
    }

    internal void OpenBagView()
    {
        ForgeViewController.Instance.Close();
        PurifyViewController.Instance.Close();
        BagViewController.Instance.Open();
    }

    internal void OpenForgeView()
    {
        PurifyViewController.Instance.Close();
        BagViewController.Instance.Close();
        ForgeViewController.Instance.Open();
    }

    internal void OpenPurifyView()
    {
        ForgeViewController.Instance.Close();
        BagViewController.Instance.Close();
        PurifyViewController.Instance.Open();

    }
}
