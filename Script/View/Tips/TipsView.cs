using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsView : View
{
    public void Show(string tips,Action enter,Action cancel) {
        SetText(Info, tips);
        this.EnterAction = enter;
        this.CancelAction = cancel;
    }

    Text Info;
    Button Enter, Cancel;
    Action EnterAction, CancelAction;
    public override void Awake()
    {
        base.Awake();
        Enter = GetComponent<Button>("Enter");
        Cancel = GetComponent<Button>("Cancel");
        Info = GetComponent<Text>("Info");

        Enter.onClick.AddListener(OnEnter);
        Cancel.onClick.AddListener(OnCancel);
    }

    private void OnCancel()
    {
        CancelAction?.Invoke();
        Close?.Invoke(false);
        ClearButtonAction();
    }

    private void ClearButtonAction()
    {
        this.EnterAction = null;
        this.Cancel = null;
    }

    private void OnEnter()
    {
        EnterAction?.Invoke();
        Close?.Invoke(false);
        ClearButtonAction();
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

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        ClearButtonAction();
    }
}
