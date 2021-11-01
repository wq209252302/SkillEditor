using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class GatherTask : MonoBehaviour
{

    public Button ok;
    public Text info;
    public int id;
    public TaskType type;
    public Button way;

    Notification notify = new Notification();

    private void Awake()
    {
        ok = gameObject.transform.Find("ok").GetComponent<Button>();
        info = transform.Find("info").GetComponent<Text>();
        way = transform.Find("way").GetComponent<Button>();
        ok.onClick.AddListener(() =>
        {
            notify.Refresh("deltask", World.Ins.m_player.m_insID, id,type);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
            notify.Refresh("",id);
            MsgCenter.Ins.SendMsg("DelTask", notify);

            
             (UIMgr.Ins.m_uiDic["dialogue"] as Dialogue).Init(TaskMgr.Ins.dic[2]);

        });
        way.onClick.AddListener(()=> 
        {
            int index = 0;
            if(type==TaskType.atk)
            {
                index = 1;
            }
            else
            {
                index = 2;
            }
            World.Ins.m_player.Voluntarily(index);
        });

        ok.gameObject.SetActive(false);
    }
    public void Refresh(int num)
    {
        if (num <= 0)
        {
            num = 0;
            ok.gameObject.SetActive(true);
            way.gameObject.SetActive(false);
        }

        info.text = "还需要" + num + "个目标物";

    }

    public void Init(int id, TaskType type)
    {
        this.id = id;
        this.type = type;
        way.gameObject.SetActive(true);
    }

    public void onDestroy()
    {
        Destroy(gameObject);
    }

}
