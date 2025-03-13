using System;
using System.Collections;
using System.Collections.Generic;
using Game.Config;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    FSM npc;
    DialogueEntity config;
    ChestInfo chestInfo;
    private void Awake()
    {
        GameSystem.Instance.dialogueTrigger = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        if (npc != null) {
            if (DialogueViewController.Instance.IsOpen()==false)
            {
                MainViewController.Instance.Talk_Enable(true, npc._transform.position, "��̸ [F]");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    UTransform.LookTarget(UnitManager.Instance.player._transform, npc._transform);
                    UTransform.LookTarget(npc._transform, UnitManager.Instance.player._transform);
                    DialogueViewController.Instance.DODialogue(config.text);
                }
            }
            else
            {
                MainViewController.Instance.Talk_Enable(false, npc._transform.position, "��̸ [F]");
            }
        }
        else if (chestInfo!=null)
        {
            if (chestInfo.IsOpen()==false)
            {
                MainViewController.Instance.Talk_Enable(true, chestInfo.transform.position,"�� [F]");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    UTransform.LookTarget(UnitManager.Instance.player._transform, chestInfo.transform);
                    chestInfo.Open();
                }
            }
            else
            {
                MainViewController.Instance.Talk_Enable(false, chestInfo.transform.position, "�� [F]");
            }
        }
    }

    public void ReCheck() 
    {
        if (GameSystem.Instance.SceneController.state != 0)
        {
            return;
        }
        if (npc!=null)
        {
            var config = DialogueSystem.Instance.Get(npc.global_id);
            if (config!=null)
            {
                this.config = config;
                MainViewController.Instance.Talk_Enable(true, npc._transform.position, "��̸ [F]");
                //npc = f;
            }
            else
            {
                this.npc = null;
                this.config = null;
                MainViewController.Instance.Talk_Enable(false, Vector3.zero, "��̸ [F]");
            }
        }
    
    }


    private void OnTriggerEnter(Collider other)
    {

        var f= other.GetComponent<FSM>();
        if (f != null) {
            if (f.global_state != 1) {
                return;
            }
            var config= DialogueSystem.Instance.Get(f.global_id);
            if (config != null)
            {
                this.config = config;
                MainViewController.Instance.Talk_Enable(true, f._transform.position, "��̸ [F]");
                npc = f;
                //UTransform.LookTarget(UnitManager.Instance.player._transform, f._transform);
                //DialogueViewController.Instance.DODialogue(config.text);
            }
        }
        else
        {
            if (other.gameObject.layer==LayerMask.NameToLayer("Chest"))
            {
                //����
                chestInfo = other.GetComponent<ChestInfo>();//.Open();
            }
        }
    }
   

    private void OnTriggerExit(Collider other)
    {
        if (npc!=null)
        {
            var f = other.GetComponent<FSM>();
            if (f != null && f.global_id == npc.global_id)
            {
                MainViewController.Instance.Talk_Enable(false, npc._transform.position, "��̸ [F] ");
                npc = null;
                config = null;
            }
        }
        else if (chestInfo!=null)
        {
            MainViewController.Instance.Talk_Enable(false, chestInfo.transform.position, "��̸ [F] ");
            chestInfo = null;
        }
    }

    internal void DoFight()
    {
        if (npc!=null)
        {
            npc.ToFightState();
        }
    }
}
