using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : View
{
    Image Progress_Value;
    public override void Awake()
    {
        base.Awake();
        Progress_Value = GetComponent<Image>("Progress/Progress_Value");

    }
    internal void UpdateLoadProgress(float progress)
    {
        Progress_Value.fillAmount = progress;
    }
}
