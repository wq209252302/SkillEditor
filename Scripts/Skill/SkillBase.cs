using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SkillBase 
{
    public string name = string.Empty;
    public string tirgger = "0";
    public float startTime = 0;
    public bool isBgein ;

    public virtual void Play()
    {
        isBgein = true;
        startTime = Time.time;

    }

    public virtual void Stop()
    {

    }

    public virtual void Init()
    {

    }
    public virtual void  Bgein ()
    {
        
    }



    public virtual void Update(float timer)
    {
        if( isBgein && (timer- startTime ) >float.Parse(tirgger))
        {
            isBgein = false;
            Bgein();
        }

    }
}
