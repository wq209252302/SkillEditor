using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StaticCircleCheck : MonoBehaviour
{
    public float m_checkRangeActive = 3f;
    public float m_tiggerRange = 0.2f;
    public GameObject m_target;
    public Action<bool> m_call;
    public bool isSend = false;


    public bool m_isTirgger = true;

    private V3 criclel1;
    private V3 criclel2;
    void Start()
    {
        criclel1 = new V3(this.transform.position.x, this.transform.position.z,m_tiggerRange);
        criclel2 = new V3(new Vector3(0,0,m_tiggerRange));
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target)
        {
            if(Vector3.Distance(this.transform.position,m_target.transform.position)<=m_checkRangeActive)
            {
                criclel2.RefreshPos(m_target.transform.position.x, m_target.transform.position.z);
                if(CircleCollision.CircleCollisionCheck(criclel1, criclel2))
                {
                    if(m_isTirgger)
                    {
                        m_call(true);
                        m_isTirgger = false;
                        isSend = true;
                    }
                }
                else
                {
                    if(!m_isTirgger)
                    {
                    
                        
                        m_isTirgger = true;
                    }
                }

            }
            else
            {
                if (isSend)
                {
                    m_call(false);
                    isSend = false;
                }
            }
        }
       

           
       
    }


    public bool IsPos()
    {
        if (CircleCollision.CircleCollisionCheck(criclel1, criclel2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


public static class CircleCollision
{
    public static bool CircleCollisionCheck( V3 pos1,V3 pos2)
    {
        bool result = false;

        float distance = Vector2.Distance(new Vector2(pos1.x,pos1.y), new Vector2(pos2.x, pos2.y));

        result = distance <= (pos1.z+pos2.z);

        return result;
    }
}
