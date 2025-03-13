using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void Awake()
    {
        if (GameSystem.Instance.init) {
            Destroy(this.gameObject);
            return;
        }

        GameSystem.Instance.init = true;
        GameEvent.OnSceneLoadComplete += OnSceneLoadComplete;
        GameSystem.Instance.SceneController=this.GetComponent<SceneController>();
        GameSystem.Instance.CameraController = this.transform.Find("Camera").GetComponent<UCameraController>();
        GameSystem.Instance.EventSystem = EventSystem.current;//transform.Find("EventSystem").GetComponent<EventSystem>().current;


        GameObject.DontDestroyOnLoad(this.gameObject);
        SystemInit();
        if (SceneManager.GetActiveScene().name== "Init")
        {
            LoginViewController.Instance.Open();
        }
        else
        {
            MainViewController.Instance.Open();
            GameSystem.Instance.CameraController.SetTarget(GameObject.Find("1001").transform);
        }
       
    }

    private void SystemInit()
    {
        DialogueSystem.Instance.Init();
        CombatConfig.Instance.Init();

        GameEvent.DOHitlag = DOHitlag;
        GameEvent.DORadialBlur=DORadialBlur;
        GameDefine.Init();

        ViewManager.Instance.Init();

    }

    void Start()
    {
        
    }

    bool yet = false;
    // Update is called once per frame
    void Update()
    {
        GameTime.Update();
        ViewManager.Instance.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BagViewController.Instance.IsOpen() == false &&
                ForgeViewController.Instance.IsOpen() == false &&
                PurifyViewController.Instance.IsOpen() == false &&
                StoreViewController.Instance.IsOpen() == false &&
                DialogueViewController.Instance.IsOpen() == false
                )
            {
                BagViewController.Instance.Open();
                BagViewController.Instance.view.Show(-1);
            }
            else
            {
                BagViewController.Instance.Close();
                ForgeViewController.Instance.Close();
                PurifyViewController.Instance.Close();
                StoreViewController.Instance.Close();
                DialogueViewController.Instance.Close();
            }

        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            //BagData.Instance.Add(IntEx.Range(1001,1010),IntEx.Range(1,30),null);
            //BagData.Instance.Add(IntEx.Range(2001, 2057), IntEx.Range(1, 10), null);
            //BagData.Instance.Add(IntEx.Range(3001, 3013), IntEx.Range(1, 60), null);
            if (yet == false)
            {
                yet = true;
                BagData.Instance.Add(2001, 1, null);
                BagData.Instance.Add(3001, 1, null);
                BagData.Instance.Add(3002, 1, null);
            }


            BagViewController.Instance.OpenOrClose();
            if (BagViewController.Instance.IsOpen())
            {
                BagViewController.Instance.view.Show(-1);
            }
            //BagViewController.Instance.view.Show(-1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ForgeViewController.Instance.OpenOrClose();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            PurifyViewController.Instance.Open();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StoreViewController.Instance.OpenOrClose();
        }


        else if (Input.GetKeyDown(KeyCode.Z))
        {
            DialogueViewController.Instance.DODialogue("Txt/1002/D001");
        }else if (Input.GetKeyDown(KeyCode.V))
        {
            GameSave.Instance.Save(0, GameSystem.Instance.SceneController.GetActiveScene());
        }else if (Input.GetKeyDown(KeyCode.C))
        {
            GameSave.Instance.Load();
        }
    }
    Coroutine coroutine_hitlag;
    public void DOHitlag(int frame, bool lerp) {
        if (frame>0&&Time.timeScale==1)
        {
            if (coroutine_hitlag != null)
            {
                StopCoroutine(coroutine_hitlag);
            }
            coroutine_hitlag = StartCoroutine(Hitlag(frame, lerp));
        }
    }

    IEnumerator Hitlag(int frame,bool lerp)
    {
        for (int i = 0; i < frame; i++) {
            Time.timeScale = lerp ? Mathf.Lerp(1, 0, (float)i / frame) : 0;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
        coroutine_hitlag = null;
    }

    RadialBlur radialBlur;
    Volume volume;
    Coroutine RadiaBlurCoroutine;

    public void OnSceneLoadComplete() {
        volume = null;
    }

    private void DORadialBlur(RadialBlurConfig config) {
        if (volume==null)
        {
            volume = GameObject.Find("Sky and Fog Global Volume").GetComponent<Volume>();
            volume.profile.TryGet<RadialBlur>(out radialBlur);
        }
        if (RadiaBlurCoroutine != null) {
            StopCoroutine(RadiaBlurCoroutine);
        }
        RadiaBlurCoroutine= StartCoroutine(RadialBlur(config));
    }


    //¾¶ÏòÄ£ºý
    public IEnumerator RadialBlur(RadialBlurConfig config) {
        if (config != null)
        {
            radialBlur.active = config.active;

            float begin = GameTime.time;
            float end = GameTime.time + config.lerp;
            while (begin <= end)
            {
                begin += GameTime.deltaTime;
                var v = Mathf.Lerp(0, 0.106f, begin / end);
                radialBlur.intensity.value = config.active ? v : 0.106f - v;
                yield return new WaitForEndOfFrame();
            }

            if (config.active)
            {
                yield return new WaitForSeconds(0.5f);
                RadialBlurConfig quit = new RadialBlurConfig();
                quit.active = false;
                quit.lerp = 0.2f;
                DORadialBlur(quit);
            }
        }

        RadiaBlurCoroutine = null;
    }

}
