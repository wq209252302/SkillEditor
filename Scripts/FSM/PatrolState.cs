using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{

    private List<Transform> path = new List<Transform>();

    private int index=0;

    private Transform playerTransform;

    public PatrolState (FSMSystem fsm):base(fsm)
    {
        stateID = StateID.Patrol;

        Transform pathTranform = GameObject.Find("Path").transform;

       // Transform[] children = pathTranform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < pathTranform.childCount; i++)
        {
            if(pathTranform.GetChild(i)!= pathTranform)
            {
                path.Add(pathTranform.GetChild(i));
            }
        }

        playerTransform = GameObject.Find("0").transform;

    }



    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(path[index].position);
        npc.transform.Translate(Vector3.forward*Time.deltaTime*3);
        if(Vector3.Distance(npc.transform.position,path[index].position)<=3)
        {
            index++;
            index %= path.Count;
        }
    }

    public override void Reason(GameObject npc)
    {
        if(Vector3.Distance(playerTransform.position,npc.transform.position)<=3)
        {
            fsm.PreformTransition(Transition.SellPlayer);
        }
    }

    
}
