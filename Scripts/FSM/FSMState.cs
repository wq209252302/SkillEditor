using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    Null,
    SellPlayer,
    LostPlayer,
    AtcPlayer
}

public enum StateID
{
    Null,
    Patrol,
    Chase,
    Attack
}

public abstract class FSMState
{
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    protected Dictionary<Transition, StateID> behavior = new Dictionary<Transition, StateID>();

    protected FSMSystem fsm;

    public FSMState(FSMSystem fsm)
    {
        this.fsm = fsm;

    }

    public void AddTranition(Transition trans, StateID id)
    {
        if (trans == Transition.Null)
        {
            Debug.LogError("不允许NullTransition"); return;
        }
        if (id == StateID.Null)
        {
            Debug.LogError("不允许NullStateID"); return;
        }
        if (behavior.ContainsKey(trans))
        {
            Debug.LogError("已存在该行为" + trans); return;
        }
        behavior.Add(trans, id);
    }


    public void DelTransition(Transition trans)
    {

        if (trans == Transition.Null)
        {
            Debug.LogError("不允许NullTransition"); return;
        }

        if (!behavior.ContainsKey(trans))
        {
            Debug.LogError("不存在该行为" + trans); return;
        }
        behavior.Remove(trans);
    }


    public StateID GetOutputState(Transition trans)
    {
        if (behavior.ContainsKey(trans))
        {
            return behavior[trans];
        }

        return StateID.Null;
    }

    public abstract void Act(GameObject npc);

    public abstract void Reason(GameObject npc);

}
