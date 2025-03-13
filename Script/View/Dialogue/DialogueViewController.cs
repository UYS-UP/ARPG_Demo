using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueViewController : ViewController<DialogueViewController, DialogueView>
{
    public override void AddListener()
    {
        base.AddListener();
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
    }


    Dictionary<string, List<DialogItem>> dialg_data = new Dictionary<string, List<DialogItem>>();
    public List<DialogItem> Txt2DilogData(string p) {
        if (dialg_data.TryGetValue(p,out var v)) {
            return v;
        }
        else
        {
            var lst=new List<DialogItem>();
            TextAsset textAsset = ResourcesManager.Instance.Load<TextAsset>(p);
            var t = textAsset.text;

            var line_lst=t.Split(Environment.NewLine);

            foreach (var line in line_lst)
            {
                //空行或者注释 则跳出循环
                if (string.IsNullOrEmpty(line) ||line.StartsWith("##"))
                {
                    break ;
                }

                var str = line.Split("&");

                DialogItem dialogItem = new DialogItem();
                dialogItem.id = int.Parse(str[0]);
                dialogItem.name = str[1];
                dialogItem.content = str[2];
                dialogItem.cmd = new DialogCmd[str.Length - 3];
                if (str.Length > 3)
                {
                    for (int i = 0; i < dialogItem.cmd.Length; i++)
                    {
                        string[] cmd_str = str[i+3].Split("_");
                        DialogCmd cmd = new DialogCmd();
                        cmd.cmd_name = cmd_str[0];
                        cmd.cmd = int.Parse(cmd_str[1]);

                        dialogItem.cmd[i] = cmd;
                    }
                }
                lst.Add(dialogItem); 
            }
            dialg_data[p]=lst;
            return lst;
        }
        
    }


    public void DODialogue(string id) {
        var data= Txt2DilogData(id);
        if (data != null)
        {
            Open();
            view.DODialogue(data);
        }
    }


}


public class DialogItem
{
    public int id;
    public string name;
    public string content;
    public DialogCmd[] cmd;
}

public class DialogCmd
{
    public string cmd_name;
    public int cmd;

}