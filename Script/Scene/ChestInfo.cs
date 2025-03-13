using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.GS;
using UnityEngine;

public class ChestInfo : MonoBehaviour
{
    public int state;//0未打开 1打开
    public int id;//全局ID
    public int[] drop;
    Animator animator;
    private void Awake()
    {
        animator=this.GetComponent<Animator>();
    }

    public void Open() {
        if (state == 1) {
            return;
        }
        animator.Play("Open");
        animator.Update(0);
        state = 1;



    }

    internal bool IsOpen()
    {
        return state >= 1;
    }

    private void Update()
    {
        if (state == 1)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.8f)
            {

                if (drop != null)
                {
                    foreach (var item in drop)
                    {
                        int count = UnityEngine.Random.Range(1, 11);
                        for (int i = 0; i < count; i++)
                        {
                            var pos = new Vector3(transform.position.x + UnityEngine.Random.Range(-3, 3.0f), transform.position.y, transform.position.z + UnityEngine.Random.Range(-3, 3.0f));
                            var drop = ResourcesManager.Instance.Instantiate<GameObject>($"drop/{item}");
                            drop.transform.position = pos;
                        }
                    }
                }
                state = 2;
            }
        }
    }

    public void Recover(SaveChestInfo data)
    {
        state = data.State;
        if (state == 1)
        {
            animator.Play("Open", 0, 0);
            animator.Update(0);
        }
        else if(state == 2)
        {
            animator.Play("Open", 0, 1);
            
        }
    }
}
