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

    public bool _enable;//�Ƿ��
    public CanvasScaler cs;//����Ӧ����
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

    public virtual void Awake() { }//��ʼ��

    public virtual void OnEnable() {
        if (isAddUpdateListerner)
        {
            ViewManager.Instance.OnUpdate += Update;
        }
        GameSystem.Instance.CameraController.ChangeCursor(cursorEnable);
    }//��ʾ��ʱ��
    public virtual void Start() { }

    public virtual void Update() { }//ÿ֡����

    public virtual void OnDestroy() { }//�����ٵ�ʱ��

  

    public virtual void OnDisable() {
        _close_time = Time.time;
        if (isAddUpdateListerner)
        {
            ViewManager.Instance.OnUpdate -= Update;
        }
        GameSystem.Instance.CameraController.ChangeCursor(!cursorEnable);
    }//������ʾ��ʱ��

    //���ݲ�ͬ����Ļ�ֱ��� ��������
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
        //����ͼ //������ 1920*1080==>�ȱ������� 
        //�Ŵ������С ֻ�ܸ���һ����ȥ����
    }


    //����Image�����ͼƬ  ==>����������׼����
    public void SetSprite(Image image, string sprite)
    {
        var s = ResourcesManager.Instance.Load<Sprite>(sprite);
        image.sprite = s;
    }

    public void SetText(Text text,string content)  {
        text.text = content;
    }

    public void SetText(Text text, int id) { 
        //ͨ�������Ա�ȥ�������ID ��Ӧ������  ���� Ӣ��  ���� 
        //text.
    }
}
