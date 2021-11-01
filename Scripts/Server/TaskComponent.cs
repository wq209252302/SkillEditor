using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherTaskComponent : SComponent
{
    // public Dictionary<int,int> Tasks;
    public Dictionary<int, TaksBase> dic;
    public Dictionary<int, TaksBase> olddic;

    Notification notify = new Notification();
    public override void Init()
    {
        //Tasks = new Dictionary<int, int>();
        dic = new Dictionary<int, TaksBase>();
        olddic = new Dictionary<int, TaksBase>();
    }

    public void AddTask(int id, int num)
    {
        TaksBase task = dic[id];
        task.count = num;
        ;      //  Tasks.Add(id, neednum-count);
        Debug.Log("采集物品任务已添加");
        notify.Refresh("", task.tackid, task.need - num,TaskType.gather);
        MsgCenter.Ins.SendMsg("AddTask", notify);
    }

    public void CallBack()
    {
        Notification notify = new Notification();
        notify.Refresh("", 3f);

        MsgCenter.Ins.SendMsg("Startgather", notify);
    }
}


public class BattleComponent : SComponent
{
    public Dictionary<int, TaksBase> dic;
    public Dictionary<int, TaksBase> olddic;
    Notification notify = new Notification();
    public override void Init()
    {
        dic = new Dictionary<int, TaksBase>();
        olddic = new Dictionary<int, TaksBase>();
    }

    public void AddTask(int id, int num)
    {
        TaksBase task = dic[id];
        task.count = num;
        ;      //  Tasks.Add(id, neednum-count);
        Debug.Log("打怪任务已添加");
        notify.Refresh("", task.tackid, task.need - num,TaskType.atk);
        MsgCenter.Ins.SendMsg("AddTask", notify);
    }


}