using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private Truck _truck;


    public IdleState(Truck truck)
    {
        _truck = truck;
    }


    public void Enter()
    {
        Debug.Log(" Enter  Idle STATE");
        _truck.setSpeed(0.0f);
    }

    public void Exit()
    {
        Debug.Log(" Exit  Idle  STATE");
    }

    public void Update()
    {
        Debug.Log(" UPDATE  Idle  STATE");
        // _truck.setSpeed(0.0f);
    }
}

