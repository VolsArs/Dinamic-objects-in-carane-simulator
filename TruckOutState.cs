using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckOutState :IState
{
    private Truck _truck;


    public TruckOutState(Truck truck)
    {
        _truck = truck;
    }
    public void Enter()

    {
        Debug.Log(" Enter  OUT STATE");
        _truck.OnDestroy();
    }

    public void Exit()
    {
        Debug.Log(" Exit  OUT STATE");
    }

    public void Update()
    {

    }
}
