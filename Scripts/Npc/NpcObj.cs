using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObj : ObjectBase
{
    public npc_info m_info;


    public NpcObj(npc_info info)
    {
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public NpcObj(int plot, object_Info info)
    {
        m_info = new npc_info(plot, info);
        m_insID = info.ID;
        m_modelPath = info.m_res;

    }

    Notification notify = new Notification();
    public override void CreatObj(MondelType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
     


    }

    public override void OnCreat()
    {
        base.OnCreat();
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        check.m_target = World.Ins.m_player.m_go;
        check.m_call = (isenter) =>
        {
            //notify.Refresh("NPCSell",this.m_insID);
           
            UIMgr.Ins.m_uiDic["dialogue"].DoShow(true);

        };
    }

    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }
}
