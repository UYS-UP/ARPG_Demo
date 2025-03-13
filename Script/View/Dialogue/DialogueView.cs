using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DialogueView : View
{
    Text Name, Content;
    Button Option1, Option2, Option3;
    Transform Option_Root;
    public override void Awake()
    {
        base.Awake();
        Name = GetComponent<Text>("DialogueBG/Name/Text");
        Content = GetComponent<Text>("DialogueBG/Content");

        Option1 = GetComponent<Button>("DialogueBG/Option/Option1");
        Option2 = GetComponent<Button>("DialogueBG/Option/Option2");
        Option3 = GetComponent<Button>("DialogueBG/Option/Option3");
        Option_Root = transform.Find("DialogueBG/Option");
    }


    List<DialogItem> data;//当前的对话内容
    internal void DODialogue(List<DialogItem> data)
    {
        this.data = data;
        var txt = data[0];
        DOUpdate(txt.name, txt.content, txt.cmd);
    }

    float content_show_speed=0.1f;
    Tween content_tween;
    public void DOUpdate(string name,string content, DialogCmd[] btnCmds) {
        Name.text = name;
        if (content_tween!=null)
        {
            content_tween.Kill();
        }

        Option1.gameObject.SetActive(false);
        Option2.gameObject.SetActive(false);
        Option3.gameObject.SetActive(false);
        Option1.onClick.RemoveAllListeners();
        Option2.onClick.RemoveAllListeners();
        Option3.onClick.RemoveAllListeners();

        Content.text = null;
        content_tween = Content.DOText(content, content.Length * content_show_speed);
        content_tween.OnComplete(() =>
        {
            if (btnCmds == null || btnCmds.Length == 0)
            {
                Option1.gameObject.SetActive(false);
                Option2.gameObject.SetActive(false);
                Option3.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (btnCmds.Length > i)
                    {
                        var c = Option_Root.GetChild(i);
                        c.gameObject.SetActive(true);
                        c.transform.Find("Text").GetComponent<Text>().text = btnCmds[i].cmd_name;
                        if (btnCmds[i].cmd == 0)
                        {
                            c.GetComponent<Button>().onClick.AddListener(() => { Close(false); });
                        }
                        else if (btnCmds[i].cmd == 1)
                        {
                            c.GetComponent<Button>().onClick.AddListener(()=> {
                                StoreViewController.Instance.Open();
                                Close(false);
                            });
                        }
                        else if (btnCmds[i].cmd==4)
                        {
                            c.GetComponent<Button>().onClick.AddListener(() => {
                                Close(false);
                                GameSystem.Instance.dialogueTrigger.DoFight();
                            });
                        }
                        else if (btnCmds[i].cmd>10000)
                        {
                            var scene = btnCmds[i].cmd.ToString();
                            //加载对应的场景
                            c.GetComponent<Button>().onClick.AddListener(() => {
                                Close(false);
                                // LoadingViewController.Instance.Open();
                                // MainViewController.Instance.Talk_Enable(false, Vector3.zero, "交谈[F]");
                                GameSystem.Instance.SceneController.SaveCurrentPoint();
                                GameSystem.Instance.SceneController.Load(scene);
                               
                            });
                        }
                        else
                        {
                            var d = data[btnCmds[i].cmd-1];
                            c.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                DOUpdate(d.name, d.content, d.cmd);
                            });
                        }

                    }
                    else
                    {
                        Option_Root.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        });
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        // GameSystem.Instance.CameraController.ChangeCursor(false);
        GameSystem.Instance.dialogueTrigger.ReCheck();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        // GameSystem.Instance.CameraController.ChangeCursor(true);
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