
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class DialogueData
    {

        static DialogueData()
        {
            entityDic = new Dictionary<int, DialogueEntity>(2);
             DialogueEntity e0 = new DialogueEntity(1001,@"Txt/5005/1001",500501,new int[]{0,30});
            entityDic.Add(e0.id, e0);
             DialogueEntity e1 = new DialogueEntity(1002,@"Txt/5005/1002",500501,new int[]{30,60});
            entityDic.Add(e1.id, e1);

        }

       
        
        public static Dictionary<int, DialogueEntity> all {
            get {
                return entityDic;
            }
        }
		static Dictionary<int, DialogueEntity> entityDic;
		public static DialogueEntity Get(int id)
		{
            if (entityDic!=null&&entityDic.TryGetValue(id,out var entity))
			{
				return entity;
			}
            return null;
		}
    }

    
    public class DialogueEntity
    {
        //TemplateMember
		public int id;//对话ID
		public string text;//文本ID
		public int npc_id;//全局_npc_id
		public int[] level;//主角等级(条件)

        public DialogueEntity(){}
        public DialogueEntity(int id,string text,int npc_id,int[] level){
           
           this.id = id;
           this.text = text;
           this.npc_id = npc_id;
           this.level = level;

        }
    }
}
