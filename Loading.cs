using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : IState

{

    private Truck _truck;


    public Loading(Truck truck)
    {
        _truck = truck;
    }
    public void Enter()

    {
        Debug.Log(" Enter  LOADING STATE");
        _truck.setSpeed(0.0f);
    }

    public void Exit()
    {
        Debug.Log(" Exit  LOADING STATE");
    }

    public void Update()
    {
       
    }
}
