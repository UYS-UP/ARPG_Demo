using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class View 
{
    public string resPath;
    public GameObject gameObject;
    public Transform transform;
    public Canvas canvas;
    public Action<bool> Close;
    bool isAddUpdateListerner = false;
    public bool stopAI_OnOpen = false;

    public bool _enable;//是否打开
    public CanvasScaler cs;//自适应控制
    public float _close_time;

    public bool cursorEnable = true;
    public void Init(string resPath, bool isAddUpdateListerner = false, Action<bool> close = null, bool stopAI_OnOpen = false, bool cursorEnable = true)
    {
        this.resPath = resPath;
        this.isAddUpdateListerner = isAddUpdateListerner;
        this.Close = close;
        this.stopAI_OnOpen = stopAI_OnOpen;
        this.cursorEnable = cursorEnable;
    }


    public T GetComponent<T>(string path) where T : Component  
    { 
       return this.transform.Find(path).GetComponent<T>();
    }

    public virtual void Awake() { }//初始化

    public virtual void OnEnable() {
        if (isAddUpdateListerner)
        {
            ViewManager.Instance.OnUpdate += Update;
        }
        GameSystem.Instance.CameraController.ChangeCursor(cursorEnable);
    }//显示的时候
    public virtual void Start() { }

    public virtual void Update() { }//每帧更新

    public virtual void OnDestroy() { }//当销毁的时候

  

    public virtual void OnDisable() {
        _close_time = Time.time;
        if (isAddUpdateListerner)
        {
            ViewManager.Instance.OnUpdate -= Update;
        }
        GameSystem.Instance.CameraController.ChangeCursor(!cursorEnable);
    }//禁用显示的时候

    //根据不同的屏幕分辨率 调整适配
    public void SetSize() {
        var b= Screen.width / (float)Screen.height > cs.referenceResolution.x / cs.referenceResolution.y;
        if (b)
        {
            cs.matchWidthOrHeight = 1;
        }
        else
        {
            cs.matchWidthOrHeight = 0;
        }
        //背景图 //被拉伸 1920*1080==>等比例缩放 
        //放大或者缩小 只能根据一条边去计算
    }


    //设置Image组件的图片  ==>多语言适配准备的
    public void SetSprite(Image image, string sprite)
    {
        var s = ResourcesManager.Instance.Load<Sprite>(sprite);
        image.sprite = s;
    }

    public void SetText(Text text,string content)  {
        text.text = content;
    }

    public void SetText(Text text, int id) { 
        //通过多语言表去查找这个ID 对应的内容  中文 英文  繁体 
        //text.
    }
}
