using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Google.Protobuf.GS;
using Google.Protobuf.Collections;
using Debug = UnityEngine.Debug;

public class GameSave
{
    private static GameSave instance = new GameSave();
    public static GameSave Instance => instance;

    public string root = Path.Combine(Application.persistentDataPath, "Save");
    public const int SaveID = 1000;
    public const string bake = "bake";
    public const string Chest_Root = "Chest_Root";
    public void Save(long versionId,string scene)
    {
        long version = 0;
        //�ҵ��Ѵ浵��Ŀ¼ Ȼ�󸲸�ԭ�е��ļ�
        if (versionId > 0)
        {
            version = versionId;
        }
        else
        {
            //�½�һ������
            version = SaveID;// TimeHelper.Now();
        }

        var version_root = Path.Combine(root, version.ToString());
        if (Directory.Exists(version_root) == false)
        {
            Directory.CreateDirectory(version_root);
        }
        //ȫ���ļ�
        var v_main = Path.Combine(version_root, "main.txt");
        //�ؿ��ļ�
        var v_scene = Path.Combine(version_root, $"{scene}.txt");
        //��������
        SaveInfoMain saveInfoMain = new SaveInfoMain();
        saveInfoMain.Scene = scene;
        saveInfoMain.Player = SavePlayer(UnitManager.Instance.player);
        //��������
        SetSaveInfo_BagGrild(saveInfoMain.Grilds);

        //д��ȫ�����ݽ��б���
        var m_data = ProtoHelper.ToBytes(saveInfoMain);
        File.WriteAllBytes(v_main, m_data);

        //��ȡ�ؿ������� ���б���
        SaveLevelInfo saveLevelInfo = new SaveLevelInfo();

        var npc_root = GameObject.Find("NPC");
        //����NPC
        if (npc_root != null)
        {
            var fsm = npc_root.transform.GetComponentsInChildren<FSM>(true);
            if (fsm != null && fsm.Length > 0)
            {
                foreach (var item in fsm)
                {
                    saveLevelInfo.Ai.Add(item.gameObject.name, SavePlayer(item));
                }
            }
        }

        //����
        var chest_root = GameObject.Find(Chest_Root);
        if (chest_root != null)
        {
            ChestInfo[] chestInfos = chest_root.GetComponentsInChildren<ChestInfo>(true);
            if (chestInfos != null && chestInfos.Length > 0)
            {
                foreach (var item in chestInfos)
                {
                    saveLevelInfo.Chest.Add(item.gameObject.name, new SaveChestInfo()
                    {
                        State = item.state
                    });
                }
            }

        }


        //�ؿ�ʣ�����սʱ��
        var t = GameObject.Find("Timer/T001");
        if (t != null)
        {
            var t_com = t.GetComponent<Timer>();
            saveLevelInfo.Time = t_com.GetSaveInfo();
        }

        //��ʱ�� todo..

        var sceneBytes = ProtoHelper.ToBytes(saveLevelInfo);
        File.WriteAllBytes(v_scene, sceneBytes);
    }

    public SavePlayerInfo SavePlayer(FSM fsm) {
        SavePlayerInfo savePlayerInfo = new SavePlayerInfo();
        if (fsm != null&&fsm.yet)
        {
            savePlayerInfo.State = fsm.currentState.id;
            savePlayerInfo.Hp = (int)fsm.att_crn.hp;
            savePlayerInfo.Pos.Add(fsm._transform.position.x);
            savePlayerInfo.Pos.Add(fsm._transform.position.y);
            savePlayerInfo.Pos.Add(fsm._transform.position.z);

            savePlayerInfo.Rot.Add(fsm._transform.eulerAngles.x);
            savePlayerInfo.Rot.Add(fsm._transform.eulerAngles.y);
            savePlayerInfo.Rot.Add(fsm._transform.eulerAngles.z);

            savePlayerInfo.Active = fsm._gameObject.activeSelf;
        }
        return savePlayerInfo;



    }


    public void SetSaveInfo_BagGrild(MapField<int, SaveGrildInfo> dct)
    {
        dct.Clear();
        foreach (var item in BagData.Instance.dct)
        {
            SaveGrildInfo info = new SaveGrildInfo();
            info.Id = item.Key;
            info.PropId = item.Value.id;
            info.Count = item.Value.count;
         
            dct[item.Key] = info;
        }
    }

    public void Load()//long versionId
    {
        //var version_root = Path.Combine(root, versionId.ToString());
        var version_root = Path.Combine(root, SaveID.ToString());

        //ȫ���ļ�
        var v_main = Path.Combine(version_root, "main.txt");

        var main_bytes= File.ReadAllBytes(v_main);
        SaveInfoMain saveInfoMain = ProtoHelper.ToObject<SaveInfoMain>(main_bytes);
        
        //�ؿ��ļ�
        var v_scene = Path.Combine(version_root, $"{saveInfoMain.Scene}.txt");
        var scene_bytes = File.ReadAllBytes(v_scene);
        SaveLevelInfo saveLevelInfo = ProtoHelper.ToObject<SaveLevelInfo>(scene_bytes);

        GameSystem.Instance.SceneController.Load(saveInfoMain.Scene, false, () => {
            //�ָ�����
            UnitManager.Instance.player.Recover(saveInfoMain.Player);
            //�ָ�����
            BagData.Instance.Recover(saveInfoMain);


            //�ָ�������NPC 

            var npc_root = GameObject.Find("NPC");
            //����NPC
            if (npc_root != null)
            {
                var fsm = npc_root.transform.GetComponentsInChildren<FSM>(true);
                if (fsm != null && fsm.Length > 0)
                {
                    foreach (var item in fsm)
                    {
                        if (saveLevelInfo.Ai.ContainsKey(item.gameObject.name)) {
                            var data = saveLevelInfo.Ai[item.gameObject.name];
                            item.Recover(data);
                        }
                      
                    }
                }
            }

            //����
            var chest_root = GameObject.Find(Chest_Root);
            if (chest_root != null)
            {
                ChestInfo[] chestInfos = chest_root.GetComponentsInChildren<ChestInfo>(true);
                if (chestInfos != null && chestInfos.Length > 0)
                {
                    foreach (var item in chestInfos)
                    {
                        if (saveLevelInfo.Chest.ContainsKey(item.gameObject.name))
                        {
                            var data = saveLevelInfo.Chest[item.gameObject.name];
                            item.Recover(data);
                        }
                        //saveLevelInfo.Chest.Add(item.gameObject.name, new SaveChestInfo()
                        //{
                        //    State = item.state
                        //});
                    }
                }
            }

            //ʣ��Ĺؿ�ʱ��
            //�ؿ�ʣ�����սʱ��
            var t = GameObject.Find("Timer/T001");
            if (t != null)
            {
                var t_com = t.GetComponent<Timer>();
                if (t_com != null) {
                    t_com.Recover(saveLevelInfo.Time);
                }
                //saveLevelInfo.Time = t_com.GetSaveInfo();
            }

        });

       
    }

    public bool HasSaveVersion() {

        var version_root = Path.Combine(root, SaveID.ToString());
        if (Directory.Exists(version_root))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void NewGame()
    {
        var version_root = Path.Combine(root, SaveID.ToString());
        //bake
        var bake_root = Path.Combine(root, bake);
        //Debug.LogError(bake_root);
        if (Directory.Exists(bake_root)==false)
        {
            Directory.CreateDirectory(bake_root);
        }
        else
        {
            var sub= Directory.GetFiles(bake_root);
            foreach (var file in sub) {
                File.Delete(file);
            }
        }
     

        if (Directory.Exists(version_root))
        {
            //FileSystem.CopyDirectory(version_root, bake_root, true);
            var sub = Directory.GetFiles(version_root);
            foreach (var file in sub)
            {
                //Debug.LogError(file);
                //using (var read=new StreamReader(Path.Combine(version_root, file),encoding:System.Text.Encoding.UTF8))
                //{
                //    var s = read.ReadToEnd();
                //    using (var writer = new StreamWriter(Path.Combine(bake_root, file),false, System.Text.Encoding.UTF8)) {
                //        writer.Write(s);
                //    }
                //}
                File.Copy(file,Path.Combine(bake_root,Path.GetFileName(file)),true);
            }
            Directory.Delete(version_root, true);
        }
    }

}
