using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class TaskMgr : SingTon<TaskMgr>
{
    public Dictionary<int, TaksBase> dic = new Dictionary<int, TaksBase>();

    public void Init()
    {
        string str = File.ReadAllText("Assets/TaskList.txt");
        dic = JsonConvert.DeserializeObject<Dictionary<int, TaksBase>>(str);
    }
}
