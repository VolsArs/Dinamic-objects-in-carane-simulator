using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingState : IState
{
    //private Show_path _show_path;
    //private Find_path _find_path;
    private TruckManager _truckManager;
    private Truck _truck;
    private bool corutineAllowed;
    private List<List<GameObject>> fullPath;

    public MovingState(Truck truck) {
       _truck = truck ;
    }
    public void Enter()
    {
        corutineAllowed = true;
        _truck.setSpeed(0.2f);   
    }

    public void Exit()
    {
        Debug.Log(" Exit  MOVING STATE");
    }
 
        public void Update()
    {
 
        if (corutineAllowed)
        {
            _truck.StartCoroutine(GoByTheRoute(_truck.getRouteToGo()));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        corutineAllowed = false;

        fullPath = FindingFullPath();

        for (int i = 0; i < fullPath.Count; i++)
        {  
            while (_truck.getTparam() < 1)
            {      
                _truck.setTparam(Time.deltaTime); 

                _truck.transform.position = Bezier.GetPoint(fullPath[routeNumber], _truck.getTparam());

                _truck.transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(fullPath[routeNumber], _truck.getTparam()));

                yield return new WaitForEndOfFrame();
            }

           // tParam = 0f;
            _truck.setTparamToZero(0f);

            _truck.addRouteCounter(1);

            if (_truck.getRouteToGo() > fullPath.Count)
            {
                _truck.setRouteCounterToZero(0);
            }

            corutineAllowed = true;

        }
    }

    /// <returns></returns>
        public List<List<GameObject>> FindingFullPath()       
    {
        
        return TruckManager.instance.chucks(TruckManager.instance.listPoints, TruckManager.instance.AddPoints );
        //return _show_path.chucks(_find_path.listPoints, _show_path.AddPoints);
    }

}
