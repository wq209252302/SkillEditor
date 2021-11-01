using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public float xMpa, yMap;
    public float xoffset, yoffset;

    private Transform player;
    Dictionary<MondelType, Transform> monsterdic = new Dictionary<MondelType, Transform>();

    List<ObjectBase> otherGoPos = new List<ObjectBase>();
    Vector3 playerpos = new Vector3(0,0,0);
    List<Vector3> otherpos = new List<Vector3>();

    private void Awake()
    {
        xMpa = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        yMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        xoffset = xMpa / World.Ins.xlength;
        yoffset = yMap / World.Ins.ylength;

        player = transform.Find("player");

        monsterdic.Add(MondelType.Gatther, transform.Find("gather"));
        monsterdic.Add(MondelType.Normal, transform.Find("monster"));
        monsterdic.Add(MondelType.NPC, transform.Find("npc"));
    }

    private void Update()
    {
        if(World.Ins.m_insDic.Count!=otherGoPos.Count)
        {
            otherGoPos.Clear();
            otherpos.Clear();
            foreach (var item in World.Ins.m_insDic)
            {
                otherGoPos.Add(item.Value);
                otherpos.Add(new Vector3(0,0,0));
            }
        }

        if(player&&World.Ins.m_player.m_go)
        {
            playerpos.Set(World.Ins.m_player.m_go.transform.position.x*xoffset, World.Ins.m_player.m_go.transform.position.z*yoffset,0);
            player.localPosition = playerpos;
        }

        if(otherGoPos!=null && otherGoPos.Count>0)
        {
            for (int i = 0; i < otherGoPos.Count; i++)
            {
                if (otherGoPos[i].m_go==null)
                {
                    monsterdic[otherGoPos[i].m_type].gameObject.SetActive(false);
                }
                else
                {
                    otherpos[i] = new Vector3(otherGoPos[i].m_go.transform.position.x * xoffset, otherGoPos[i].m_go.transform.position.z * yoffset, 0);
                    monsterdic[otherGoPos[i].m_type].transform.localPosition = otherpos[i];
                }

            }
        }
    }

    private void OnDestroy()
    {

        CancelInvoke("UpdateMap");
    }

}
