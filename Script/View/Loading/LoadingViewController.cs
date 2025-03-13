using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingViewController : ViewController<LoadingViewController, LoadingView>
{
    internal void UpdateLoadProgress(float progress)
    {
        if (IsOpen()==false)
        {
            Open();
        }
        view.UpdateLoadProgress(progress);
    }
}
