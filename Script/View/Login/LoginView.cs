using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : View
{
    private Button ContinueBtn;
    
    public override void Awake()
    {
        base.Awake();
        var new_game= transform.Find("NewGame").GetComponent<Button>();
        new_game.onClick.AddListener(NewGame);

        ContinueBtn = GetComponent<Button>("Continue");
        ContinueBtn.onClick.AddListener(ContinueBtnOnClick);
        
        GetComponent<Button>("Quit").onClick.AddListener(Application.Quit);

    }

    private void ContinueBtnOnClick()
    {
        if (GameSave.Instance.HasSaveVersion())
        {
            GameSave.Instance.Load();
        }
        else
        {
            NewGame();   
        }
        
    }

    private void NewGame()
    {
        //打开Loading 
        //切换场景
        //更新场景的加载进度
        GameSystem.Instance.SceneController.Load("Game");
        GameSave.Instance.NewGame();
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
