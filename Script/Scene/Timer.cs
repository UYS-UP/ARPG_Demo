using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Ê±³¤")]
    public float time=300;

    public FSM boss;
    float begin;
    void Start()
    {
        begin = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - begin >= time)
        {
            if (boss != null)
            {
                if (boss.IsDead()==false)
                {
                    MainViewController.Instance.OnLevelComplete(false, GameSystem.Instance.SceneController.LoadLastScene);
                }
            }
            this.gameObject.SetActive(false);
        }
    }

    public float GetSaveInfo()
    {
        return time - (Time.time - begin);
    }

    public void Recover(float time)
    {
        var last = this.time - time;
        begin -= last;
    }
}
