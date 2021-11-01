using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MsgCenter : SingTon<MsgCenter>
{
    Dictionary<string, Action<Notification>> m_MsgDicts = new Dictionary<string, Action<Notification>>();

    public void AddListener(string msg,Action<Notification> action)
    {
        if(!m_MsgDicts.ContainsKey(msg))
        {
            m_MsgDicts.Add(msg,null);
        }
        m_MsgDicts[msg] += action;
    }

    public void RemoveListener(string msg,Action<Notification> action)
    {
        if(m_MsgDicts.ContainsKey(msg))
        {
            m_MsgDicts[msg] -= action;
            if(m_MsgDicts[msg] == null)
            {
                m_MsgDicts.Remove(msg);
            }
        }
    }

    public void SendMsg(string msg,Notification notif)
    {
        if(m_MsgDicts.ContainsKey(msg))
        {
            m_MsgDicts[msg].Invoke(notif);
        }
    }
}
