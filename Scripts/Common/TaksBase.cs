using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskType
{
    atk,gather
}
public class TaksBase 
{
    public bool end = false;
    public int tackid;
    public int need;
    public int count;
    public int needId;
    public int enemyid;
    public TaskType type;
    //public List<string> str;

    public TaksBase(int tackid, int need,int count ,int needId, TaskType type)
    {
        this.tackid = tackid;
        this.need = need;
        this.count = count;
        this.needId = needId;
        this.type = type;
     
    }


}
