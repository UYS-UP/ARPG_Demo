using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int state = 0;
 
    public void Load(string next, bool reset=false, Action action = null) {
        LoadingViewController.Instance.Open();
        LoginViewController.Instance.Close();
        state = 1;
        StartCoroutine(LoadSceneAsync(next,false, action));
    }

    IEnumerator LoadSceneAsync(string next,bool reset,Action action) {
       var op= SceneManager.LoadSceneAsync(next);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) {
            yield return new WaitForEndOfFrame();
            LoadingViewController.Instance.UpdateLoadProgress(op.progress);
        }

        float progress = op.progress;
        while (progress <= 1)
        {
            progress += GameTime.deltaTime;
            LoadingViewController.Instance.UpdateLoadProgress(progress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
        yield return op;
        var player=UnitManager.Instance.CreatePlayer();
        GameSystem.Instance.CameraController.SetTarget(player.transform);
        yield return new WaitForEndOfFrame();
        LoadingViewController.Instance.Close();
        MainViewController.Instance.Open();

        var gate_point=GameObject.Find("GatePoint");
        if (gate_point!=null)
        {
            var t= gate_point.transform.Find("0");
            player.GetComponent<FSM>().SetPosition(t);
        }
        GameEvent.OnSceneLoadComplete?.Invoke();
        
        if (reset)
        {
            ResetToLast();
        }

        state = 0;
        action?.Invoke();
    }

    //切换到上一个场景
    public void LoadLastScene() {
        Load(now_scene_id, true);
    }

    public string now_scene_id;//当前的副本ID

    Vector3 last_pos;
    Quaternion last_rot;
    public void SaveCurrentPoint() {
        now_scene_id = SceneManager.GetActiveScene().name;
        last_pos = UnitManager.Instance.player._transform.position;
        last_rot = UnitManager.Instance.player._transform.rotation;
    }

    public void ResetToLast() {
        UnitManager.Instance.player._transform.position = last_pos;
        UnitManager.Instance.player._transform.rotation = last_rot;
        now_scene_id = null;
    }

    public string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
