using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGather : UIBase
{
    private Button m_gatherBtn;
    private Slider m_gatherSlider;
    //private
    private Notification notify;
    public float time;
    public float newtime;
    public float oldtime;
    bool isOpen = false;
    public void Init()
    {

        DoCreat("gather");
        m_gatherBtn = m_go.transform.Find("Button").GetComponent<Button>();
        m_gatherSlider = m_go.transform.Find("Slider").GetComponent<Slider>();
        m_gatherSlider.gameObject.SetActive(false);

        MsgCenter.Ins.AddListener("Startgather", (notify) =>
        {
            time = (float)notify.data[0] ;
            isOpen = true;
            m_gatherSlider.gameObject.SetActive(true);
            m_gatherSlider.value = 0;
            oldtime = Time.time;
            Debug.Log("开始采集,时间" + Time.time);
            

        });

        UIMgr.Ins.Updates.Add(Update);
        DoShow(false);

        m_gatherBtn.onClick.AddListener(() =>
        {
            notify = new Notification();
            notify.Refresh("gather", World.Ins.m_player.m_insID);

            MsgCenter.Ins.SendMsg("ServerMsg", notify);
        });
    }

    public void SendEnd()
    {
        notify = new Notification();
        
        notify.Refresh("gatherEnd", World.Ins.m_player.m_insID, 100, 1);
        isOpen = false;
        m_gatherSlider.gameObject.SetActive(false);
        Debug.Log(Time.time);

        MsgCenter.Ins.SendMsg("DelGather", notify);


        MsgCenter.Ins.SendMsg("ServerMsg", notify);

        this.DoShow(false);
    }

    public void Update(float timer)
    {
        if(isOpen)
        {
            if (m_gatherSlider.value < 1)
            {
                newtime = Time.time - oldtime;
                m_gatherSlider.value = newtime / time;
            }
            else
            {
                SendEnd();
            }
        }
      
    }


}
