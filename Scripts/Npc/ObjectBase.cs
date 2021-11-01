using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase
{
    public GameObject m_go;
    public Vector3 m_localPos;
    public Animator m_anim;
    public UIPate m_pate;
    public MondelType m_type;

    public int m_insID;
    public string m_modelPath;

    public virtual void CreatObj(MondelType type)
    {
        m_type = type;
        if(!string.IsNullOrEmpty(m_modelPath)&&m_insID>=0)
        {
            m_go = (GameObject)GameObject.Instantiate(Resources.Load(m_modelPath));
           // m_go.AddComponent<AttackSkill>();
           // m_go = (GameObject)GameObject.Instantiate(Resources.Load(m_modelPath));
            m_go.name = m_insID.ToString();
            m_go.transform.position = m_localPos;
            if(m_go)
            {
                OnCreat();
            }

        }
    }

    public virtual void OnCreat()
    {

    }

    public virtual void SetPos(Vector3 pos)
    {
        m_localPos = pos;
    }

    public  void MoveByTranslate(Vector3 look,Vector3 pos)
    {
        
        m_go.transform.LookAt(look);
        m_go.transform.Translate(pos);
    }

    public virtual void Destory()
    {
        if (m_pate)
        {
            GameObject.Destroy(m_pate.m_go);
        }

        GameObject.Destroy(m_go);
        m_localPos = Vector3.zero;
        m_anim = null;
        m_insID = -1;
    }


}
