using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : ObjectBase
{

    //基础信息
    public player_info m_info;

    public PlayerObj(player_info info)
    {
        m_info = info;
    }

    //设置位置
    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }

    //生成
    public override void OnCreat()
    {
        base.OnCreat();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.m_gather.SetActive(false);
        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_hpMax, m_info.m_MP / m_info.m_mpMax);

    }

}

public class HostPlayer : PlayerObj
{
    //控件
    Player player;
    public HostPlayer(player_info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;

    }

    //生成角色并设置位置
    public override void CreatObj(MondelType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }

    public void Voluntarily(int index)
    {
        player.motionWay(index);
    }

    public void  StopMove()
    {
        if(player.isMove!=false)
        {
            player.isMove = false;
            player.anim.SetBool("isRun", true);
        }
        
    }

    //生成控件 并初始化
    public override void OnCreat()
    {
        base.OnCreat();
        player = m_go.gameObject.GetComponent<Player>();
        if(player==null)
        {
            player = m_go.AddComponent<Player>();
        }
     
        player.InitData();

        MsgCenter.Ins.AddListener("PlayerMove", (notify) =>
         {
             Vector3 rot = (Vector3)notify.data[0];
             Vector3 pos = (Vector3)notify.data[1];
             MoveByTranslate(rot,pos);
             if(!player.isMove)
             {
                 player.isMove = true;
             }
             player.Run();

         });


        MsgCenter.Ins.AddListener("playerhitend",(notify)=> 
        {
            float hp = (float)notify.data[0];
            m_pate.SetHp(hp);
        });


        MsgCenter.Ins.AddListener("skill",(notify)=> 
        {
            bool use =(bool) notify.data[0];
            int id = (int)notify.data[1];

            JoyButtonHandler(id.ToString());
            
        });

    }

    Notification notify = new Notification();

    public void JoystickHandlerMoving(float h, float v)
    {
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            //player.
           // MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 3);
            notify.Refresh("Player", new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 3);
            MsgCenter.Ins.SendMsg("MovePos", notify);
            
        }

    }

    public void JoyButtonHandler(string name)
    {
       
        switch (name)
        {
            case "1":
                player.SetData("1");

                player.Play();
                break;
            case "2":
                player.SetData("2");
                player.Play();
                break;
            case "3":
                player.SetData("3");
                player.Play();
                break;
            case "4":
                player.SetData("4");
                player.Play();
                break;
            case "5":
                player.SetData("5");
                player.Play();
                break;
            case "-1":
                Debug.Log("没有解锁此技能");
                break;
        }
    }

      //case "attack":
      //          player.SetData("1");
      //          player.Play();
      //          break;
      //      case "skill1":
      //          player.SetData("2");
      //          player.Play();
      //          break;
      //      case "skill2":
      //          player.SetData("3");
      //          player.Play();
      //          break;
      //      case "skill3":
      //          player.SetData("4");
      //          player.Play();
      //          break;
      //      case "skill4":
      //          player.SetData("5");
      //          player.Play();

      //          break;


    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }
}


public class OtherPlayer : PlayerObj
{
    public OtherPlayer(player_info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;

    }

    public override void CreatObj(MondelType type)
    {
        SetPos(m_info.m_pos);
        base.CreatObj(type);
    }
}


