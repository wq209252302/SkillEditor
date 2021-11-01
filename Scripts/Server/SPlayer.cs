using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SPlayer
{
    public long m_insid;

    public Vector3 m_pos;
    public float Hp;
    public float Mp;
    public float Atk;

    public List<int> buffs;
    //public List<SkillProp> skills;
    public Dictionary<string, SkillProp> skills;
    public Dictionary<int, int> bag;

    public Dictionary<ComponentType, SComponent> components;

    Notification notify = new Notification();

    public void InitPlayer()
    {
        //ComponentType a = ComponentType.


        buffs = new List<int>();
        skills = new Dictionary<string, SkillProp>();
        components = new Dictionary<ComponentType, SComponent>();
        bag = new Dictionary<int, int>();

        skills.Add("attack", new SkillProp(1,"attack", 15));
        skills.Add("skill1", new SkillProp(2,"skill1", 30));
        skills.Add("skill2", new SkillProp(3,"skill2", 55));
        skills.Add("skill3", new SkillProp(4,"skill3", 60));
        skills.Add("skill4", new SkillProp(5,"skill4", 120));

    }

    public int HostNum(int id)
    {
        if (bag.ContainsKey(id))
        {
            return bag[id];
        }
        return 0;
    }

    public void TackEnd(int itemid, int count)
    {

        if ((components[ComponentType.task] as GatherTaskComponent).dic.ContainsKey(itemid))
        {
            GatherTaskComponent task = components[ComponentType.task] as GatherTaskComponent;
            //neednum = task.dic[itemid].need;

            notify.Refresh("", itemid, task.dic[itemid].need - count);

            MsgCenter.Ins.SendMsg("taskNeed" + itemid.ToString(), notify);

        }

        if ((components[ComponentType.battle] as BattleComponent).dic.ContainsKey(itemid))
        {
            BattleComponent task = components[ComponentType.battle] as BattleComponent;
            //neednum = task.dic[itemid].need;

            notify.Refresh("", itemid, task.dic[itemid].need - count);

            MsgCenter.Ins.SendMsg("taskNeed" + itemid.ToString(), notify);

        }

        // if()


    }


    public void submitTask(int id, TaskType type)
    {
        if(type==TaskType.gather)
        {
            GatherTaskComponent task = components[ComponentType.task] as GatherTaskComponent;
            task.dic[id].end = true;
            task.olddic.Add(task.dic[id].tackid, task.dic[id]);
            task.dic.Remove(id);

        }
        else if(type==TaskType.atk)
        {
            BattleComponent task = components[ComponentType.battle] as BattleComponent;
            task.dic[id].end = true;
            task.olddic.Add(task.dic[id].tackid, task.dic[id]);
            task.dic.Remove(id);
        }
      

    }

    public void ProOperation(int type, float value)
    {
        switch (type)
        {
            case 1:
                Hp += value;
                break;
            case 2:
                Mp += value;
                break;
            default:
                break;
        }

        Notification m_notify = new Notification();
        m_notify.Refresh("ByServer", type, value);
        MsgCenter.Ins.SendMsg("propchange", m_notify);
    }


    public void SendSkill(string name)
    {


        bool use = false;
        int id = -1;
        if (skills.ContainsKey(name))
        {
            use = true;
            id = skills[name].id;
        }
        notify = new Notification();
        notify.Refresh("skill", use, id);
        MsgCenter.Ins.SendMsg("skill", notify);
    }
}

public class SComponent
{
    public Func<long, SPlayer> GetPlayerBtld;

    Notification m_notify;
    public virtual void S2CMsg(string cmd, object value)
    {
        if (m_notify == null)
        {
            m_notify = new Notification();
        }
        m_notify.Refresh("ByServer", value);
        MsgCenter.Ins.SendMsg(cmd, m_notify);
    }

    public virtual void Init() { }
}

public class SkillProp
{
    public string name;
    public float range;
    public int id;
    public SkillProp(int id, string name , float range)
    {
        this.id = id;
        this.name = name;
        this.range = range;
    }
}
