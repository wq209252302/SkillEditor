using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskList : UIBase
{
    public Transform content;
    public GameObject item;
    public Dictionary<int,GatherTask> dic;
    //public List<>
    public void Init()
    {
        dic = new Dictionary<int, GatherTask>();
        DoCreat("TaskList");
        content = m_go.transform.Find("Scroll View/Viewport/Content");
        item = Resources.Load<GameObject>("taskItem");
        MsgCenter.Ins.AddListener("AddTask",(notif)=> 
        {
            int id = (int)notif.data[0];
            int count = (int)notif.data[1];
            TaskType type = (TaskType)notif.data[2];
            GameObject obj = GameObject.Instantiate(item,content,false);
            GatherTask item1 = obj.AddComponent<GatherTask>();
            item1.Init(id,type);
            dic.Add(id,item1);
            dic[id].Refresh(count);

        });


        MsgCenter.Ins.AddListener("DelTask",(notify)=> 
        {
            int id = (int)notify.data[0];
            dic[id].onDestroy();
            dic.Remove(id);
        });

        //MsgCenter.Ins.AddListener("Refresh",(notify)=> 
        //{
            
        //});

        MsgCenter.Ins.AddListener("taskNeed100", (notify) =>
        {
            int id = (int)notify.data[0];
            int num = (int)notify.data[1];
            dic[id].Refresh(num);
        });

        MsgCenter.Ins.AddListener("taskNeed1", (notify) =>
        {
            int id = (int)notify.data[0];
            int num = (int)notify.data[1];
            dic[id].Refresh(num);
        });
    }



}




