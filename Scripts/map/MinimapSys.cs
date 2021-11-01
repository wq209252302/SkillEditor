using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSys : UIBase
{
    public MapControl m_map;

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void DoCreat(string path)
    {
        base.DoCreat(path);
        //m_go.transform.Find("minimap").localScale = new Vector3(0.3f, 0.3f, 0.3f);
       
        Transform map = m_go.transform.Find("minimap/map");
      
        m_map = map.gameObject.AddComponent<MapControl>();
    }

    public override void DoShow(bool active)
    {
        GameObject.Destroy(m_map);
        base.DoShow(active);

    }
}
