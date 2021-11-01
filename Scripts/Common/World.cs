using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SingTon<World>
{
    public Dictionary<int, ObjectBase> m_insDic = new Dictionary<int, ObjectBase>();

    public Dictionary<string, Normal> enemys = new Dictionary<string, Normal>();
    Notification notify = new Notification();

    public HostPlayer m_player;
    private GameObject npcroot;
    public Camera m_main;

    public float xlength;
    public float ylength;

    public void Init()
    {
#if CWSDK
        
#endif
        TaskMgr.Ins.Init();
        GameObject plan = GameObject.Find("Plane");
        Vector3 length = plan.GetComponent<MeshFilter>().mesh.bounds.size;
        xlength = length.x * plan.transform.lossyScale.x;
        ylength = length.z * plan.transform.lossyScale.z;

        m_main = GameObject.Find("Main Camera").GetComponent<Camera>();
        npcroot = GameObject.Find("NpcRoot");

        UIMgr.Ins.Init(GameObject.Find("UIRoot"), GameObject.Find("HUD"));


        player_info info = new player_info();
        info.ID = 0;  // 为0 会报错
        info.m_name = "tony";
        info.m_level = 9;
        info.m_pos = Vector3.zero;
        info.m_res = "boss_maoyou";
        info.m_HP = 2000;
        info.m_MP = 1800;
        info.m_hpMax = 2000;
        info.m_mpMax = 2000;

        //===技能解析读取   已取出单独解析
        //info.skillList =   

        notify.Refresh("InstantiatePlay", info.ID, 2000);

        MsgCenter.Ins.SendMsg("ServerMsg", notify);

        m_player = new HostPlayer(info);
        m_player.CreatObj(MondelType.Null);
        // m_player.OnCreat(); //没有初始化 其中的player
        JoyStickMgr.Ins.SetJoyArg(m_main, m_player);
        JoyStickMgr.Ins.JoyAction = true;


        CreateIns();

        MsgCenter.Ins.AddListener("hitend", (notify) =>
         {
             int id = (int)notify.data[0];
             float hp = (float)notify.data[1];
             enemys[id.ToString()].m_pate.SetHp(hp);

         });

        MsgCenter.Ins.AddListener("deadEnemy", (notify) =>
        {
            int id = (int)notify.data[0];
            enemys[id.ToString()].Destory();

        });



    }


    private void CreateIns()
    {
        List<MondelsType> data = MonsterCfg.Ins.GetJsonData();
        object_Info info;

        for (int i = 0; i < data.Count; i++)
        {
            info = new object_Info();
            info.ID = m_insDic.Count + 1;
            info.m_res = data[i].name;
            info.m_pos = MonsterCfg.Ins.AsVector3(data[i].pos);
            info.m_type = (MondelType)data[i].TypeIndex;
            CreatObj(info);
        }

    }

    ObjectBase monster = null;

    private void CreatObj(object_Info info)
    {
        monster = null;
        if (info != null)
        {
            if (info != null)
            {
                if (info.m_type == MondelType.Normal)
                {
                    monster = new Normal(info);

                    notify.Refresh("InstantiateEnemy", info.ID, 1000);

                    MsgCenter.Ins.SendMsg("ServerMsg", notify);
                }
                else if (info.m_type == MondelType.Gatther)
                {
                    monster = new Gather(info);
                }
                else if (info.m_type == MondelType.NPC)
                {
                    monster = new NpcObj(1, info);
                    //monster
                }
            }

            if (monster != null)
            {
                monster.CreatObj(info.m_type);
                monster.m_go.transform.SetParent(npcroot.transform, false);
                m_insDic.Add(info.ID, monster);
                if (monster is Normal)
                {
                    enemys.Add(monster.m_go.name, (monster as Normal));
                }

            }
            else
            {
                Debug.LogError("生成失败");
            }
        }
    }


    public void AutoMoveByInsId(int target, Vector3 pos)
    {
        using (var tmp = m_insDic.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (target == tmp.Current.Key)
                {
                    //TODO  让实例移动
                    tmp.Current.Value.SetPos(pos);
                }
            }
        }
    }
}
