using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : IState
{
    private Truck _truck;


    public WaitingState(Truck truck) { 
         _truck = truck;
    }
    public void Enter()
    {
       Debug.Log(" Enter  WAITING STATE");
        _truck.setSpeed(0.0f); 
    }

    public void Exit()
    {
        Debug.Log(" Exit  WAITING STATE");
    }

    public void Update()
    {
        Debug.Log(" UPDATE  WAITING STATE");
       // _truck.setSpeed(0.0f);
    }
}
