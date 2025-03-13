using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavView : View
{
    Text money;
    public override void Awake()
    {
        base.Awake();
        money = GetComponent<Text>("Money/CoinText");

        //打开背包 
        //打开锻造 
        //打开洗炼
        GetComponent<Button>("Bag").onClick.AddListener(ViewManager.Instance.OpenBagView);
        GetComponent<Button>("Forge").onClick.AddListener(ViewManager.Instance.OpenForgeView);
        GetComponent<Button>("Purify").onClick.AddListener(ViewManager.Instance.OpenPurifyView);
        GetComponent<Button>("Quit").onClick.AddListener(QuitGame);
        //GetComponent<Image>("Bag").alphaHitTestMinimumThreshold = 0.3f;
        //GetComponent<Image>("Forge").alphaHitTestMinimumThreshold = 0.3f;
        //GetComponent<Image>("Purify").alphaHitTestMinimumThreshold = 0.3f;
    }

    private void QuitGame()
    {
        TipsViewController.Instance.Show("是否退出游戏，并且保存数据", () =>
        {
            GameSave.Instance.Save(0, GameSystem.Instance.SceneController.GetActiveScene());
            ViewManager.Instance.CloseAll?.Invoke(false);
            GameSystem.Instance.SceneController.Load("Init", false, () => {
                LoginViewController.Instance.Open();
            });

        }, () => { Close(false); });
    }

    Text text_last_select;
    public void SetButton_Select(int type) {
        if (type == 0) {
            if (text_last_select!=null)
            {
                text_last_select.color = Color.white;
            }
            text_last_select = GetComponent<Text>("Bag/Tips");
            text_last_select.color = Color.red;
        }
        else if (type == 1)
        {
            if (text_last_select != null)
            {
                text_last_select.color = Color.white;
            }
            text_last_select = GetComponent<Text>("Forge/Tips");
            text_last_select.color = Color.red;
        }
        else if (type==2)
        {
            if (text_last_select != null)
            {
                text_last_select.color = Color.white;
            }
            text_last_select = GetComponent<Text>("Purify/Tips");
            text_last_select.color = Color.red;
        }
        
    }

    public void SetMenoy() {
        SetText(money, BagData.Instance.money.ToString());
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetMenoy();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
