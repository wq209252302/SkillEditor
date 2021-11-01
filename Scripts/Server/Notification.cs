using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification 
{
    public string msg;
    public object[] data;

    public void Refresh(string msg,params object[] data)
    {
        this.msg = msg;
        this.data = data;
    }

    public void Clear()
    {
        msg = string.Empty;
        data = null;
    }
}
