using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{

    private Transform playerTransition;

    public ChaseState(FSMSystem fsm):base(fsm)
    {
        stateID = StateID.Chase;
        playerTransition = GameObject.Find("0").transform;
    }

    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(playerTransition.position);
        npc.transform.Translate(Vector3.forward * 2 *Time.deltaTime);
    }

    public override void Reason(GameObject npc)
    {
        if(Vector3.Distance(playerTransition.position,npc.transform.position)>=6)
        {
            fsm.PreformTransition(Transition.LostPlayer);
        }

        if (Vector3.Distance(playerTransition.position, npc.transform.position) <=2)
        {
            fsm.PreformTransition(Transition.AtcPlayer);
        }

    }
}
