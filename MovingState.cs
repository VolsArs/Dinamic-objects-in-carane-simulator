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
   // private float tParam_lokal;
   // private int routeToGo;
    private List<List<GameObject>> fullPath;

    public MovingState(Truck truck) {
       // _show_path = show_Path;
       // _find_path = find_Path; 
      // _truckManager = truckManager;
       _truck = truck ;
             
    }
    public void Enter()
    {
      //  Debug.Log(" Enter  MOVING STATE");
        //routeToGo = 0;
        //tParam = 0f;
        corutineAllowed = true;
        _truck.setSpeed(0.2f);
        
    }

    public void Exit()
    {
      //  Debug.Log(" Exit  MOVING STATE");
    }
   // Функция update иметирует аналогичную функцию объекта monobeh чтобы потом пробросить на объект движения
   //

        public void Update()
    {
      //  Debug.Log(" Update  MOVING STATE");
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

           // Debug.Log(" Заначнеи ROUTETOGO " +  _truck.getRouteToGo());
            while (_truck.getTparam() < 1)
            {
               // Debug.Log(" Текущая скорость " +  _truck.getSpeed());

              // tParam_lokal  += Time.deltaTime * _truck.getSpeed();

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
    /// <summary>
    /// Используется для составления полного маршрута с учетом добавленных точек
    ///
    /// </summary>
    /// <returns></returns>
        public List<List<GameObject>> FindingFullPath()       
    {
        
        return TruckManager.instance.chucks(TruckManager.instance.listPoints, TruckManager.instance.AddPoints );
        //return _show_path.chucks(_find_path.listPoints, _show_path.AddPoints);
    }

}
