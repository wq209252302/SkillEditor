using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_Info 
{
    public int ID;
    public string m_name;
    public Vector3 m_pos;
    public string m_res;
    public MondelType m_type;
}


public class player_info :object_Info
{
    public int m_level;
    public float m_HP;
    public float m_hpMax;
    public float m_MP;
    public float m_mpMax;
    public List<SkillXml> skillList;
}


public class npc_info :object_Info
{
    public int m_plotId = 0;
    public npc_info(int plot,object_Info info)

    {
        // m_plotId = plot;
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = MondelType.NPC;

    }
}


public class monster_info :object_Info
{
    public monster_info(MondelType type,object_Info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = type;

    }
}

