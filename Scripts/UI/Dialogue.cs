using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Dialogue : UIBase
{
    public Button Close_Btn;
    public Button take_Btn;
    public Button levae_Btn;
    public Text word;
    public TaksBase task;

    Notification notify = new Notification();

    List<string> words_gather = new List<string>() { "你可以帮我采集一个***吗", "谢谢你啊", "还没有采过来吗", "你好啊" };
    List<string> words_attack = new List<string>() { "你可以帮我干掉一个***吗", "谢谢你啊", "还没有干掉吗", "你好啊" };


    public void Init(TaksBase task)
    {
        DoCreat("Dialogue");
        Close_Btn = m_go.transform.Find("Close").GetComponent<Button>();
        take_Btn = m_go.transform.Find("dialogue/take").GetComponent<Button>();
        levae_Btn = m_go.transform.Find("dialogue/leave").GetComponent<Button>();
        word = m_go.transform.Find("dialogue/Text").GetComponent<Text>();
        if (task.type == TaskType.gather)
        {
            GatherTaskInit(task);

        }
        else if(task.type == TaskType.atk)
        {
            AttackTaskInit(task);
        }

        Close_Btn.onClick.AddListener(() =>
        {
            this.DoShow(false);
        });

        levae_Btn.onClick.AddListener(() =>
        {
            this.DoShow(false);
        });
        this.DoShow(false);
    }

    private void GatherTaskInit(TaksBase task)
    {
        this.task = task;

        word.text = words_gather[0];
        take_Btn.onClick.RemoveAllListeners();
        take_Btn.onClick.AddListener(() =>
        {
            word.text = words_gather[1];
            take_Btn.gameObject.SetActive(false);
            notify.Refresh("Task", World.Ins.m_player.m_insID, task);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
            this.DoShow(false);

        });

    }

    private void AttackTaskInit(TaksBase task)
    {

        this.task = task;

        word.text = words_attack[0];
        take_Btn.onClick.RemoveAllListeners();
        take_Btn.onClick.AddListener(() =>
        {
            word.text = words_attack[1];
            take_Btn.gameObject.SetActive(false);


            notify.Refresh("Task", World.Ins.m_player.m_insID, task);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
            this.DoShow(false);

        });
    }



}
