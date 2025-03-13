using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class MainViewController : ViewController<MainViewController, MainView>
{
    public override void AddListener()
    {
        base.AddListener();
    }

    public void UpdateBossHP(float v)
    {
        view.UpdateBossHP(v);
    }

    public void UpdatePlayerHP(float v)
    {

        view.UpdatePlayerHP(v);
    }

    public void UpdatePlayerMP(float v)
    {
        view.UpdatePlayerMP(v);
    }


    public void EnableBossHP(bool enable,string name) {
        if (!IsOpen())
        {
            Open();
        }
        view.EnableBossHP(enable, name);
    }

    public void SetSkillCD(int type, float cd) { 
        view.SetSkillCD(type, cd);  
    }

    internal void OpenCD_Tips()
    {
        view.OpenCD_Tips();
    }

    public async void OnLevelComplete(bool win,Action callback)
    {
        if (IsOpen()==false)
        {
            Open();
        }
        view.OnLevelComplete(win);
        await Task.Delay(5000);
        view.CloseResult();
        callback?.Invoke();
    }

    public void Talk_Enable(bool enable,Vector3 world_pos,string text)
    {
        if (IsOpen()) {
            if (enable)
            {
                var v = GameSystem.Instance.CameraController._camera.WorldToScreenPoint(world_pos);
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(), v, GameSystem.Instance.CameraController._camera, out var localPoint);
                view.Talk_Enable(enable, v, text);
            }
            else
            {
                view.Talk_Enable(enable, world_pos, text);
            }
        }
      
    }

    public void OnGetProp(string text)
    {
         view?.OnGetProp(text);
    }
}
