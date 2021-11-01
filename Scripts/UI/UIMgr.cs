using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMgr:SingTon<UIMgr>
{
    public GameObject m_root;
    public GameObject m_hudroot;

    public Dictionary<string, UIBase> m_uiDic;

    public List<Action<float>> Updates;

    public void Init(GameObject root , GameObject hud)
    {
        m_root = root;
        m_hudroot = hud;
        Updates = new List<Action<float>>();
        m_uiDic = new Dictionary<string, UIBase>();
        //m_uiDic.Add("Lobby",new UIBase());
       // m_uiDic.Add("battle", new Battlesys());
        m_uiDic.Add("minimap", new MinimapSys());
        //m_uiDic.Add("taskPanel", new TaskSys());
        m_uiDic.Add("gather",new UIGather());
        m_uiDic.Add("dialogue", new Dialogue());
        m_uiDic.Add("taskList", new TaskList());

        Open("minimap");

        (m_uiDic["gather"] as UIGather).Init();
        (m_uiDic["dialogue"] as Dialogue).Init(TaskMgr.Ins.dic[1]);
        (m_uiDic["taskList"] as TaskList).Init();
    }


    public void Open(string key)
    {
        UIBase ui;
        if(m_uiDic.TryGetValue(key,out ui))
        {
            ui.DoCreat(key);
        }
    }
}
