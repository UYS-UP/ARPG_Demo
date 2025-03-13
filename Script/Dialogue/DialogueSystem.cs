using System.Collections;
using System.Collections.Generic;

using Game.Config;

using UnityEngine;

public class DialogueSystem 
{
    static DialogueSystem instance = new DialogueSystem();
    public static DialogueSystem Instance => instance;
    Dictionary<int, Dictionary<int, DialogueEntity>> npc_config = new Dictionary<int, Dictionary<int, DialogueEntity>>();
    public void Init() {
        if (DialogueData.all != null) {
            foreach (var data in DialogueData.all) {

                if (npc_config.ContainsKey(data.Value.npc_id)==false)
                {
                    npc_config[data.Value.npc_id] = new Dictionary<int, DialogueEntity>();
                }
                npc_config[data.Value.npc_id][data.Key]=data.Value;
            }
        }
    }


    public DialogueEntity Get(int npc_global_id) {
        if (npc_config.TryGetValue(npc_global_id,out var v))
        {
            var player = UnitManager.Instance.player;
            foreach (var item in v)
            {
                if (item.Value.level!=null)
                {
                    if (player.level>= item.Value.level[0]&& player.level< item.Value.level[1])
                    {
                        return item.Value;
                    }
                }
            }
        }
        return null;
    }
    

}
