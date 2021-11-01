using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MonsterCfg:SingTon<MonsterCfg>
{
    public MonsterCfg()
    {
        Init();
    }
    public List<MondelsType> Data;
    string path;


    void Init()
    {
        path = Application.streamingAssetsPath + @"/monster.json";
        if (path != null)
        {
            string json = File.ReadAllText(path);
            Data = JsonConvert.DeserializeObject<List<MondelsType>>(json);

        }
    }


    public List<MondelsType> GetJsonData()
    {
        return Data;
    }

    public Vector3 AsVector3(V3 v3)
    {
        return new Vector3(v3.x, v3.y, v3.z);
    }
}
