using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    private Transform playerTransform;
    private float time = 0;
    Notification notify = new Notification();

    public AttackState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Attack;
        playerTransform = GameObject.Find("0").transform;
    }
    public override void Act(GameObject npc)
    {
        if(Time.time-time>=2)
        {
            time = Time.time;
            notify.Refresh("PlayHit",World.Ins.m_player.m_insID,120);
            MsgCenter.Ins.SendMsg("ServerMsg", notify);
        }
    }

    public override void Reason(GameObject npc)
    {
        
        if (playerTransform !=null&& Vector3.Distance(playerTransform.position, npc.transform.position) >=2)
        {
            fsm.PreformTransition(Transition.SellPlayer);
        }
    }
}
