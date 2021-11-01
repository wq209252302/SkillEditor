using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectBase
{
    public monster_info m_info;
    public Monster(MondelType type, monster_info info)
    {
        info.m_type = type;
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void OnCreat()
    {
        base.OnCreat();
    }

}

public class Normal : Monster
{
    public Normal(monster_info info) : base(MondelType.Normal, info)
    {


    }
    public Normal(object_Info info) : base(MondelType.Normal, new monster_info(MondelType.Normal, info))
    {

    }

    public FSMSystem fsm;

    public override void CreatObj(MondelType type)
    {

        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

   // public bool isHit = false;

    public bool Hit()
    {
        if(m_go==null)
        {
            return false;
        }
        if (Vector3.Distance(m_go.transform.position,World.Ins.m_player.m_go.transform.position)<=2)
        {
            return true;
        }
        return false;
    }

    public override void OnCreat()
    {
        m_go.AddComponent<Enemy>();
       
        
        base.OnCreat();
        //设置状态显示

       m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.SetData(m_info.m_name,1000,100);


        m_pate.m_name.gameObject.SetActive(true);
        m_pate.m_hp.gameObject.SetActive(true);
        m_pate.m_mp.gameObject.SetActive(true);
        m_pate.m_gather.gameObject.SetActive(false);
    }
}


public class Gather : Monster
{
    public Gather(monster_info info) : base(MondelType.Gatther, info)
    {


    }
    public Gather(object_Info info) : base(MondelType.Gatther, new monster_info(MondelType.Gatther, info))
    {

    }


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
            if(m_pate.m_gathers.Count>0)
            {
                UIMgr.Ins.m_uiDic["gather"].DoShow(isenter);
            }
            else
            {
                Debug.Log("次数已用光");
            }
        };


        MsgCenter.Ins.AddListener("DelGather",(notify)=>
        {
            m_pate.DesGather();
        });

        
       
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(false);
        m_pate.m_hp.gameObject.SetActive(false);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(true);



        //static
        //显示采集
    }
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            int insID = (int)obj.data[0];
            //if (insID == m_insID)//逻辑上是需要判断是不是当前的采集物品
            //{
            m_pate.SetData((int)obj.data[1]);
            //}
        }
    }

}


public class DartCar : Monster
{
    public DartCar(monster_info info) : base(MondelType.DartCar, info)
    {


    }
    public DartCar(object_Info info) : base(MondelType.DartCar, new monster_info(MondelType.Normal, info))
    {

    }

    public override void CreatObj(MondelType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public override void OnCreat()
    {
        base.OnCreat();
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        check.m_call = (isenter) =>
            {
                Debug.Log("进入检测范围");
            };
    }
}





