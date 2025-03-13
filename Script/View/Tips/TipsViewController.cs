using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsViewController : ViewController<TipsViewController, TipsView>
{
    public void Show(string tips, Action enter=null, Action cancel = null) {
        Open();
        SetTipsToTop();
        view.Show(tips, enter, cancel);
    }
}
