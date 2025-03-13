using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager 
{
    static UnitManager instance = new UnitManager();
    public static UnitManager Instance => instance;

    public FSM player;

    public GameObject CreatePlayer()
    {
        if (player == null)
        {
            var go = ResourcesManager.Instance.Instantiate<GameObject>("Unit/1001");
            var targetPoint = GameObject.Find("GatePoint/0");
            go.transform.position = targetPoint.transform.position;
            go.transform.forward = targetPoint.transform.forward;

            player = go.GetComponent<FSM>();
        }
        return player.gameObject;
    }

    Dictionary<int, List<FSM>> npc = new Dictionary<int, List<FSM>>();
    public bool CanCreate(int id,int max) {
        if (npc.TryGetValue(id,out var v))
        {
            return v.Count < max;
        }
        return true;
    }

    public void RemoveNPC(FSM fsm) {
        if (npc.TryGetValue(fsm.id,out var v))
        {
            v.Remove(fsm);
        }
    }

    public FSM CreateNPC(int id) {
        var config= Game.Config.UnitData.Get(id);
        if (config!=null)
        {
            var go= ResourcesManager.Instance.Instantiate<GameObject>(config.prefab_path);
            var fsm= go.GetComponent<FSM>();
            fsm.AI = true;
            if (npc.ContainsKey(id)==false)
            {
                npc[id] = new List<FSM>();
            }
            npc[id].Add(fsm);
            return fsm;
        }
        return null;
    }

}
