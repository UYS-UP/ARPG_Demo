using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavViewController : ViewController<NavViewController, NavView>
{
    public void SetTop() {
        Open();
        SetNavToTop();
    }

    public void SetMenoy() {
        if (IsOpen())
        {
            view.SetMenoy();
        }
    }

    public void SetButton_Select(int type)
    {
        view.SetButton_Select(type);
    }
}
