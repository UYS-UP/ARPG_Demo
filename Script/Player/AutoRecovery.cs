using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecovery : MonoBehaviour
{
    [Header("回收时间")]
    public float destroy_time;
    [Header("资源的路径")]
    public string path;
    float begin;
    // Start is called before the first frame update
    private void OnEnable()
    {
        begin = GameTime.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameTime.time - begin >= destroy_time) {

            ResourcesManager.Instance.Destroy_Skill(path, this.gameObject);
        }
    }
}
