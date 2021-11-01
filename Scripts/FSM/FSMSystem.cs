using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private Dictionary<StateID, FSMState> states = new Dictionary<StateID, FSMState>();

    private StateID currentStateID;
    private FSMState currentState;


    public void Init()
    {
        if (currentState != null)
        {
            currentState.AddTranition(Transition.LostPlayer, StateID.Patrol);
            currentState.AddTranition(Transition.SellPlayer, StateID.Chase);
            currentState.AddTranition(Transition.AtcPlayer, StateID.Attack);

        }
    }

    public void Update(GameObject npc)
    {
        currentState.Act(npc);
        currentState.Reason(npc);
    }

    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("FSMState不能为空");
        }
        if (currentState == null)
        {
            currentState = s;
            currentStateID = s.ID;
        }
        if (states.ContainsKey(s.ID))
        {
            Debug.LogError(s.ID + "此状态已存在"); return;
        }

        states.Add(s.ID, s);

    }


    public void DelState(StateID id)
    {
        if (id == StateID.Null)
        {
            Debug.LogError("无法删除空状态"); return;
        }
        if (!states.ContainsKey(id))
        {
            Debug.LogError("无法删除不存在的状态"); return;
        }
        states.Remove(id);
    }

    public void PreformTransition(Transition trans)
    {
        if (trans == Transition.Null)
        {
            Debug.LogError("无法转换为空"); return;
        }
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.Null)
        {
            Debug.LogError("当前状态为Null无法转换为空");
            return;
        }
        if (!states.ContainsKey(id))
        {
            Debug.LogError("当前状态不存在无法转换为空");
            return;
        }

        currentState = states[id];
        currentStateID = id;
    }
}
