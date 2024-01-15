using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using static UnityEngine.GraphicsBuffer;

public class Truck : MonoBehaviour
{
    //public GameObject truck;
    private StateMachine _stateMachine;
    //public Find_path find_Path;
    //public Show_path show_path;
    public float _speed;
    private int _routeToGo;
    private bool _canMove;
    public bool _isLoadingFinished;
    private InstantiateTruck _instTruck;
    private Truck [] _trucks;
    public String _currentState;
    public bool _runOnce;
    public bool isWaiting;
    private Truck truck;

    [Range(0, 1)]
    public float tParam;

    void Start()
    {
        _stateMachine = new StateMachine(this);
        _instTruck = FindObjectOfType<InstantiateTruck>();
        _stateMachine.Start();
        isWaiting = false;
        _canMove = true;
        _instTruck.TruckSpawn();
        _runOnce = false;

    }

    void Update()
    {
        Sensor();
  
       _currentState = GetCurrentStateOfTruck();   


        if ((_stateMachine.getCurrentState().Equals(_stateMachine.GetState<Loading>())))
        {
            _canMove = false;
        }

        if (_isLoadingFinished & (_stateMachine.getCurrentState().Equals(_stateMachine.GetState<Loading>())) & !isWaiting)
        {
            _canMove = true;
            _runOnce = false;

        }
        if ((_stateMachine.getCurrentState().Equals(_stateMachine.GetState<WaitingState>())))
        {
            if (!isAnyTruckLoaded())
            {
                _canMove = true;
            }
            else {
                _canMove = false;
            }

            if (!_runOnce)
            {
                _instTruck.Inst();
                _runOnce = true;
            }

            isWaiting = true;      
        }

        if (_canMove)
        {
            _stateMachine.SetState(_stateMachine.GetState<MovingState>());
            _stateMachine.getCurrentState().Update();            
        }
        if (isWaiting)
        {
           
        }
    }

    public float getSpeed()
    {
        return _speed;
    }

    public void setSpeed(float speed)
    {
        this._speed = speed;
    }
    public float getTparam()
    {

        return tParam;
    }

    public void setTparam(float time)
    {
        this.tParam += time * this._speed;
    }

    public int getRouteToGo()
    {
        return this._routeToGo;
    }

    public void addRouteCounter(int routeToGo)
    {
        this._routeToGo += routeToGo;
    }

    public void setRouteCounterToZero(int zero)
    {
        this._routeToGo = zero;
    }
    public void setTparamToZero(float zero)
    {
        this.tParam = zero;
    }
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.tag == "Waiting")
        {
            _stateMachine.setStateWaiting();
        }
        if (collision.gameObject.tag == "Loading")
        {
            Debug.Log("Åñòü ñîïðèêîñíîâåíèå ñî ñôåðîé Loading");
            _stateMachine.setStateLoading();
        }
        if (collision.gameObject.tag == "Truck_exit")
        {
            _stateMachine.setStateOut();
        }
    }
    public String GetCurrentStateOfTruck()
    {

       return _stateMachine.getCurrentState().ToString();
    }
    public bool isAnyTruckLoaded() {
        _trucks = GameObject.FindObjectsOfType<Truck>();
        if (_trucks.Length > 0)
        {
            for (int i = 0; i < _trucks.Length; i++)
            {
   
                if (_trucks[i].GetCurrentStateOfTruck().Equals("Loading"))
                {
                    Debug.Log(" ÍÀ ÇÀÃÐÓÇÊÅ ÍÀÕÎÄÈÒÑß ÃÐÓÇÎÂÈÊ");
                    return true;
                }
            }
        }
        return false;
    }
    private void Sensor()
    {
        var layerMask = 1 << 8;
        Vector3 frontSensor = new Vector3(0f, 7f, 15f);
        RaycastHit truckHit;
        Vector3 sensorStartPosition = transform.position;
        sensorStartPosition += transform.forward * frontSensor.z;
        sensorStartPosition += transform.up * frontSensor.y;
        Debug.DrawRay(sensorStartPosition, transform.forward * 5, Color.red);
        if (Physics.Raycast(sensorStartPosition, transform.forward * 5, out truckHit, 8f))
        {
            Truck target = truckHit.transform.gameObject.GetComponent<Truck>();
            if (target != null)
            {
                setSpeed(0.0f);
                target = null;

            } else if (target != null) {
                setSpeed(0.2f);
            }

        }
    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}



  

   

