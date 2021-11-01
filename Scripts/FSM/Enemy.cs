using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private FSMSystem fsm;
    // Start is called before the first frame update
    void Start()
    {
        InitFSM();
    }

    void InitFSM()
    {
        fsm = new FSMSystem();
        ChaseState chase = new ChaseState(fsm);
        PatrolState patrol = new PatrolState(fsm);
        AttackState attack = new AttackState(fsm);
        chase.AddTranition(Transition.LostPlayer, StateID.Patrol);
        patrol.AddTranition(Transition.SellPlayer, StateID.Chase);
        chase.AddTranition(Transition.AtcPlayer, StateID.Attack);
        attack.AddTranition(Transition.SellPlayer,StateID.Chase);

        fsm.AddState(chase);
        fsm.AddState(patrol);
        fsm.AddState(attack);



    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update(gameObject);
    }
}
