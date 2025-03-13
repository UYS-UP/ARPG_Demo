using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecovery : MonoBehaviour
{
    [Header("����ʱ��")]
    public float destroy_time;
    [Header("��Դ��·��")]
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
