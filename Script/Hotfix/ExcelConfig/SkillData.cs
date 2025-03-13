
using System.Collections.Generic;
using UnityEngine;

namespace Game.Config
{
    public class SkillData
    {

        static SkillData()
        {
            entityDic = new Dictionary<int, SkillEntity>(52);
             SkillEntity e0 = new SkillEntity(10011,0,0f,5,20f,0f,null,0,3f,45f,3f);
            entityDic.Add(e0.id, e0);
             SkillEntity e1 = new SkillEntity(10012,0,0f,5,28f,0f,null,0,3f,45f,3f);
            entityDic.Add(e1.id, e1);
             SkillEntity e2 = new SkillEntity(10013,0,0f,5,39f,0f,null,0,3f,45f,3f);
            entityDic.Add(e2.id, e2);
             SkillEntity e3 = new SkillEntity(10014,0,0f,5,43f,0f,null,0,3f,45f,3f);
            entityDic.Add(e3.id, e3);
             SkillEntity e4 = new SkillEntity(10015,0,3f,5,70f,0f,new float[]{0f,0f,3f},1,3f,45f,3f);
            entityDic.Add(e4.id, e4);
             SkillEntity e5 = new SkillEntity(10016,0,5f,5,85f,0f,new float[]{0f,3f,3.5f},0,3f,45f,3f);
            entityDic.Add(e5.id, e5);
             SkillEntity e6 = new SkillEntity(10017,0,6.5f,5,98f,0f,new float[]{0f,0f,3.8f},0,3f,45f,3f);
            entityDic.Add(e6.id, e6);
             SkillEntity e7 = new SkillEntity(10018,0,7.8f,5,113f,0f,null,0,3f,45f,3f);
            entityDic.Add(e7.id, e7);
             SkillEntity e8 = new SkillEntity(10019,0,0f,5,113f,0f,new float[]{0f,0f,5f},0,30f,45f,3f);
            entityDic.Add(e8.id, e8);
             SkillEntity e9 = new SkillEntity(10020,0,0f,5,113f,0f,new float[]{0f,0f,5f},0,30f,45f,3f);
            entityDic.Add(e9.id, e9);
             SkillEntity e10 = new SkillEntity(10021,0,0f,5,113f,0f,new float[]{0f,0f,5f},0,30f,45f,3f);
            entityDic.Add(e10.id, e10);
             SkillEntity e11 = new SkillEntity(10022,0,0f,5,113f,0f,new float[]{0f,0f,5f},0,30f,45f,3f);
            entityDic.Add(e11.id, e11);
             SkillEntity e12 = new SkillEntity(20011,0,0f,5,20f,0f,new float[]{0f,0f,0.3f},0,2f,0f,3f);
            entityDic.Add(e12.id, e12);
             SkillEntity e13 = new SkillEntity(20012,0,0f,5,20f,0f,new float[]{0f,0f,0.35f},0,2f,0f,3f);
            entityDic.Add(e13.id, e13);
             SkillEntity e14 = new SkillEntity(20013,0,0f,5,28f,0f,new float[]{0f,0f,0.37f},0,2f,0f,3f);
            entityDic.Add(e14.id, e14);
             SkillEntity e15 = new SkillEntity(20014,0,0f,5,39f,0f,new float[]{0f,0f,2f},0,2f,0f,3f);
            entityDic.Add(e15.id, e15);
             SkillEntity e16 = new SkillEntity(20015,0,0f,5,43f,0f,new float[]{0f,0f,3f},0,2f,0f,3f);
            entityDic.Add(e16.id, e16);
             SkillEntity e17 = new SkillEntity(20016,0,3f,5,70f,0f,new float[]{0f,3f,3.5f},1,3f,0f,3f);
            entityDic.Add(e17.id, e17);
             SkillEntity e18 = new SkillEntity(20017,0,5f,5,85f,0f,new float[]{0f,0f,3.8f},0,2f,0f,3f);
            entityDic.Add(e18.id, e18);
             SkillEntity e19 = new SkillEntity(20018,0,6.5f,5,98f,0f,new float[]{0f,0f,5f},0,2f,0f,3f);
            entityDic.Add(e19.id, e19);
             SkillEntity e20 = new SkillEntity(30011,0,7.8f,5,113f,0f,new float[]{0f,0f,0.3f},0,3f,0f,3f);
            entityDic.Add(e20.id, e20);
             SkillEntity e21 = new SkillEntity(30012,0,0f,5,20f,0f,new float[]{0f,0f,0.35f},0,3f,0f,3f);
            entityDic.Add(e21.id, e21);
             SkillEntity e22 = new SkillEntity(30013,0,0f,5,28f,0f,new float[]{0f,0f,0.37f},0,3f,0f,3f);
            entityDic.Add(e22.id, e22);
             SkillEntity e23 = new SkillEntity(30014,0,0f,5,39f,0f,new float[]{0f,0f,2f},0,3f,0f,3f);
            entityDic.Add(e23.id, e23);
             SkillEntity e24 = new SkillEntity(30015,0,0f,5,43f,0f,new float[]{0f,0f,3f},0,3f,0f,3f);
            entityDic.Add(e24.id, e24);
             SkillEntity e25 = new SkillEntity(30016,0,3f,5,70f,0f,new float[]{0f,3f,3.5f},1,3f,0f,3f);
            entityDic.Add(e25.id, e25);
             SkillEntity e26 = new SkillEntity(30017,0,5f,5,85f,0f,new float[]{0f,0f,3.8f},0,3f,0f,3f);
            entityDic.Add(e26.id, e26);
             SkillEntity e27 = new SkillEntity(30018,0,6.5f,5,98f,0f,new float[]{0f,0f,5f},0,3f,0f,3f);
            entityDic.Add(e27.id, e27);
             SkillEntity e28 = new SkillEntity(40011,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e28.id, e28);
             SkillEntity e29 = new SkillEntity(40012,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e29.id, e29);
             SkillEntity e30 = new SkillEntity(40013,0,0f,5,28f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e30.id, e30);
             SkillEntity e31 = new SkillEntity(40014,0,0f,5,39f,0f,new float[]{0f,0f,2f},0,1.7f,0f,3f);
            entityDic.Add(e31.id, e31);
             SkillEntity e32 = new SkillEntity(40015,0,0f,5,43f,0f,new float[]{0f,0f,3f},0,1.7f,0f,3f);
            entityDic.Add(e32.id, e32);
             SkillEntity e33 = new SkillEntity(40016,0,3f,5,70f,0f,null,1,1.7f,0f,3f);
            entityDic.Add(e33.id, e33);
             SkillEntity e34 = new SkillEntity(40017,0,5f,5,85f,0f,new float[]{0f,0f,3.8f},0,5f,0f,3f);
            entityDic.Add(e34.id, e34);
             SkillEntity e35 = new SkillEntity(40018,0,6.5f,5,98f,0f,new float[]{0f,0f,5f},0,5f,0f,3f);
            entityDic.Add(e35.id, e35);
             SkillEntity e36 = new SkillEntity(50011,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e36.id, e36);
             SkillEntity e37 = new SkillEntity(50012,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e37.id, e37);
             SkillEntity e38 = new SkillEntity(50013,0,0f,5,28f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e38.id, e38);
             SkillEntity e39 = new SkillEntity(50014,0,0f,5,39f,0f,new float[]{0f,0f,2f},0,1.7f,0f,3f);
            entityDic.Add(e39.id, e39);
             SkillEntity e40 = new SkillEntity(50015,0,0f,5,43f,0f,new float[]{0f,0f,3f},0,1.7f,0f,3f);
            entityDic.Add(e40.id, e40);
             SkillEntity e41 = new SkillEntity(50016,0,3f,5,70f,0f,null,1,1.7f,0f,3f);
            entityDic.Add(e41.id, e41);
             SkillEntity e42 = new SkillEntity(50017,0,5f,5,85f,0f,new float[]{0f,0f,3.8f},0,5f,0f,3f);
            entityDic.Add(e42.id, e42);
             SkillEntity e43 = new SkillEntity(50018,0,6.5f,5,98f,0f,new float[]{0f,0f,5f},0,5f,0f,3f);
            entityDic.Add(e43.id, e43);
             SkillEntity e44 = new SkillEntity(60011,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e44.id, e44);
             SkillEntity e45 = new SkillEntity(60012,0,0f,5,20f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e45.id, e45);
             SkillEntity e46 = new SkillEntity(60013,0,0f,5,28f,0f,null,0,1.7f,0f,3f);
            entityDic.Add(e46.id, e46);
             SkillEntity e47 = new SkillEntity(60014,0,0f,5,39f,0f,new float[]{0f,0f,2f},0,1.7f,0f,3f);
            entityDic.Add(e47.id, e47);
             SkillEntity e48 = new SkillEntity(60015,0,0f,5,43f,0f,new float[]{0f,0f,3f},0,1.7f,0f,3f);
            entityDic.Add(e48.id, e48);
             SkillEntity e49 = new SkillEntity(60016,0,3f,5,70f,0f,null,1,1.7f,0f,3f);
            entityDic.Add(e49.id, e49);
             SkillEntity e50 = new SkillEntity(60017,0,5f,5,85f,0f,new float[]{0f,0f,3.8f},0,5f,0f,3f);
            entityDic.Add(e50.id, e50);
             SkillEntity e51 = new SkillEntity(60018,0,6.5f,5,98f,0f,new float[]{0f,0f,5f},0,5f,0f,3f);
            entityDic.Add(e51.id, e51);

        }

       
        
        public static Dictionary<int, SkillEntity> all {
            get {
                return entityDic;
            }
        }
		static Dictionary<int, SkillEntity> entityDic;
		public static SkillEntity Get(int id)
		{
            if (entityDic!=null&&entityDic.TryGetValue(id,out var entity))
			{
				return entity;
			}
            return null;
		}
    }

    
    public class SkillEntity
    {
        //TemplateMember
		public int id;//技能ID
		public int tag;//技能标签
		public float cd;//CD
		public int hit_max;//伤害目标上限
		public float phy_damage;//技能物理伤害
		public float magic_damage;//技能魔法伤害
		public float[] add_fly;//附加击飞效果
		public int ignor_collision;//无视单位碰撞
		public float atk_distance;//施法距离
		public float fx_angle;//修正范围
		public float block_distance;//可被格挡的距离

        public SkillEntity(){}
        public SkillEntity(int id,int tag,float cd,int hit_max,float phy_damage,float magic_damage,float[] add_fly,int ignor_collision,float atk_distance,float fx_angle,float block_distance){
           
           this.id = id;
           this.tag = tag;
           this.cd = cd;
           this.hit_max = hit_max;
           this.phy_damage = phy_damage;
           this.magic_damage = magic_damage;
           this.add_fly = add_fly;
           this.ignor_collision = ignor_collision;
           this.atk_distance = atk_distance;
           this.fx_angle = fx_angle;
           this.block_distance = block_distance;

        }
    }
}
