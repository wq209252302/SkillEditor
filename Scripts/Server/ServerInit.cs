using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalProps
{
    public static Dictionary<long, SPlayer> players = new Dictionary<long, SPlayer>();
}

public enum ComponentType
{
    nil = 0,
    task,
    battle,
    gather
}

public class ServerInit : MonoBehaviour
{
    public Vector3 m_playerPos;
    public Vector3 m_playerRot;
    public Dictionary<int, Vector3> m_otherPosDic;
    public Dictionary<int, int> enemy;

    private void Awake()
    {
        MsgCenter.Ins.AddListener("MovePos", (notify) =>
         {
             if (notify.msg.Equals("Player"))
             {
                 m_playerPos = (Vector3)notify.data[0];
                 m_playerRot = (Vector3)notify.data[1];
                 notify.Refresh("", m_playerPos, m_playerRot);
                 MsgCenter.Ins.SendMsg("PlayerMove", notify);
             }
             else if (notify.msg.Equals("Other"))
             {
                 if (m_otherPosDic == null)
                 {
                     m_otherPosDic = new Dictionary<int, Vector3>();
                 }
                 int insid = (int)notify.data[0];
                 Vector3 pos = (Vector3)notify.data[1];
                 if (!m_otherPosDic.ContainsKey(insid))
                 {
                     m_otherPosDic.Add(insid, pos);
                 }
                 else
                 {
                     m_otherPosDic[insid] = pos;
                 }
             }
         });


        MsgCenter.Ins.AddListener("ServerMsg", (notif) =>
         {
             if (notif.msg.Equals("InstantiatePlay"))
             {
                 int id = (int)notif.data[0];
                 int hp = (int)notif.data[1];
                 LocalProps.players[id].Hp = hp;

                 //LocalProps.players[id].SendSkill(info);

             }

             if (notif.msg.Equals("gather"))
             {
                 Debug.Log("点击采集按钮");

                 int insid = (int)notif.data[0];

                 GatherTaskComponent gather = LocalProps.players[insid].components[ComponentType.task] as GatherTaskComponent;
                 gather.CallBack();


             }

             if (notif.msg.Equals("AcceptTask"))
             {
                 int takid = (int)notif.data[0];
                 foreach (var item in LocalProps.players)
                 {
                     if (item.Key == 1)
                     {
                         item.Value.components.Add(ComponentType.task, new GatherTaskComponent());
                         item.Value.components[ComponentType.task].Init();
                     }
                 }
             }

             if (notif.msg.Equals("Skill"))
             {
                 int id = (int)notif.data[0];
                 string info = notif.data[1].ToString();

                 LocalProps.players[id].SendSkill(info);

             }

             if (notif.msg.Equals("Task"))
             {
                 int insid = (int)notif.data[0];//1
                 TaksBase Task = notif.data[1] as TaksBase;
                 if(Task.type == TaskType.gather)
                 {
                     GatherTaskComponent task = LocalProps.players[insid].components[ComponentType.task] as GatherTaskComponent;
                     task.dic.Add(Task.tackid, Task);
                     task.AddTask(Task.tackid, LocalProps.players[insid].HostNum(Task.needId));
                 }
                 else
                 {
                     BattleComponent task = LocalProps.players[insid].components[ComponentType.battle] as BattleComponent;
                     task.dic.Add(Task.tackid, Task);
                     task.AddTask(Task.tackid, LocalProps.players[insid].HostNum(Task.needId));
                 }
               

             }

             if (notif.msg.Equals("gatherEnd"))
             {
                 int insid = (int)notif.data[0];
                 int itemid = (int)notif.data[1];
                 int count = (int)notif.data[2];
                 if (!LocalProps.players[insid].bag.ContainsKey(itemid))
                 {
                     LocalProps.players[insid].bag.Add(itemid, 0);
                 }

                 LocalProps.players[insid].bag[itemid] += count;

                 LocalProps.players[insid].TackEnd(itemid, LocalProps.players[insid].bag[itemid]);


             }

             if (notif.msg.Equals("deltask"))
             {
                 int insid = (int)notif.data[0];
                 int itemid = (int)notif.data[1];
                 TaskType type = (TaskType)notif.data[2];
                 LocalProps.players[insid].submitTask(itemid,type);
             }

             if (notif.msg.Equals("InstantiateEnemy"))
             {
                 int id = (int)notif.data[0];
                 int hp = (int)notif.data[1];
                 if (enemy.ContainsKey(id))
                 {
                     enemy[id] = hp;
                 }
                 else
                 {
                     enemy.Add(id, hp);
                 }

             }

             if (notif.msg.Equals("Hit"))
             {
                 int playid = (int)notif.data[0];
                 int enemyid = (int)notif.data[1];
                 int hp = (int)notif.data[2];
                 if (enemy.ContainsKey(enemyid))
                 {
                     enemy[enemyid] -= hp;
                 }
                 if (enemy[enemyid] <= 0)
                 {
                     notif.Refresh("", enemyid);
                     MsgCenter.Ins.SendMsg("deadEnemy", notif);
                     LocalProps.players[playid].TackEnd(enemyid,1);
                 }
                 else
                 {
                     notif.Refresh("", enemyid, enemy[enemyid] / 1000f);
                     MsgCenter.Ins.SendMsg("hitend", notif);
                 }

             }

             if (notif.msg.Equals("PlayHit"))
             {
                 int id = (int)notif.data[0];
                 int hp = (int)notif.data[1];
                 LocalProps.players[id].Hp -= hp;

                 notif.Refresh("", LocalProps.players[id].Hp/2000);
                 MsgCenter.Ins.SendMsg("playerhitend", notif);
                 //LocalProps.players[id].SendSkill(info);

             }


         });

        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 0;
        splayer.Hp = 100;

        splayer.components.Add(ComponentType.battle, new BattleComponent());
        splayer.components.Add(ComponentType.task, new GatherTaskComponent());
        splayer.components.Add(ComponentType.gather, new GatherComponent());

        enemy = new Dictionary<int, int>();

        LocalProps.players.Add(splayer.m_insid, splayer);

        if (LocalProps.players == null) return;

        foreach (var item in LocalProps.players)
        {
            foreach (var ite in item.Value.components)
            {
                ite.Value.GetPlayerBtld = GetPlayer;
                ite.Value.Init();
            }
        }
    }

    //private SPlayer GetPlayer (long id)
    //{
    //    using (var tmp = LocalProps.players.GetEnumerator())
    //    {
    //        while(tmp.MoveNext())
    //        {
    //            if(tmp.Current.Key==id)
    //            {
    //                return tmp.Current.Value;
    //            }
    //        }
    //    }

    //    return null;
    //}

    private SPlayer GetPlayer(long id)
    {
        using (var tmp = LocalProps.players.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (tmp.Current.Key == id)
                {
                    return tmp.Current.Value;
                }
            }
        }
        return null;
    }
}
