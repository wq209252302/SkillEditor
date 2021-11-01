using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MondelType
{
    Null, Normal, Gatther, DartCar, NPC
}

public class MondelsType
{
    public bool Toggle;
    public string name;
    public MondelsType type;
    public int TypeIndex;
    public V3 pos; 
    public V3 rot;
    public MondelsType(bool _Toggle, string _name, int _TypeIndex, Vector3 _pos, Vector3 _rot)
    {
        name = _name;
        Toggle = _Toggle;
        TypeIndex = _TypeIndex;
        pos = new V3(_pos);
        rot =  new V3(_rot) ; ;
    
       // type = (MondelType)_TypeIndex;
    }


  

}


public class V3
{
    public float x;
    public float y;
    public float z;

    public V3()
    {
        
    }
    public V3(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    public V3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void  RefreshPos(float x,float y)
    {

        this.x = x;
        this.y = y;
    }


}

