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


    // public FullPath fullPath;

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(LayerMask.GetMask("UserLayerA", "UserLayerB"));
        _stateMachine = new StateMachine(this);
        _instTruck = FindObjectOfType<InstantiateTruck>();
        _stateMachine.Start();
        isWaiting = false;
        _canMove = true;
        _instTruck.TruckSpawn();
        _runOnce = false;
       //_routeToGo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Sensor();
        // Для просмотра состояния в инспекторе
       _currentState = GetCurrentStateOfTruck();   


        if ((_stateMachine.getCurrentState().Equals(_stateMachine.GetState<Loading>())))
        {
            _canMove = false;
        }

        if (_isLoadingFinished & (_stateMachine.getCurrentState().Equals(_stateMachine.GetState<Loading>())) & !isWaiting)
        {
            //_instTruck.Inst();
            //Instantiate(_instTruck.truckPrefab, new Vector3(-138, 1, 173), new Quaternion(0, 0, -4, 0));
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

  // private void OnTriggerEnter(Collider other)
   // {
  //      if (other.gameObject.tag == "Waiting") {
         //   Debug.Log("Триггер Waiting");
         //   _stateMachine.setStateWaiting();
    //    }
  //  }

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
            Debug.Log("Есть соприкосновение со сферой Waiting");
            _stateMachine.setStateWaiting();
        }

        if (collision.gameObject.tag == "Loading")
        {
            Debug.Log("Есть соприкосновение со сферой Loading");
            _stateMachine.setStateLoading();
        }

        if (collision.gameObject.tag == "Truck_exit")
        {
          //  Debug.Log("Есть соприкосновение со сферой EXIT");
            _stateMachine.setStateOut();
        }
    }

    public String GetCurrentStateOfTruck()
    {
      //  if (_stateMachine.getCurrentState().Equals(_stateMachine.GetState<Loading>()))
       //   return "Loading";
       return _stateMachine.getCurrentState().ToString();
    }

    public bool isAnyTruckLoaded() {
       // IsTree[] trees = GameObject.FindGameObjectsOfType<IsTree>();
        _trucks = GameObject.FindObjectsOfType<Truck>();
        if (_trucks.Length > 0)
        {
            for (int i = 0; i < _trucks.Length; i++)
            {
   
                if (_trucks[i].GetCurrentStateOfTruck().Equals("Loading"))
                {
                    Debug.Log(" НА ЗАГРУЗКЕ НАХОДИТСЯ ГРУЗОВИК");
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
                //_stateMachine.setIdleState();

            } else if (target != null) {
                setSpeed(0.2f);
            }

        }
    }

   
  

    public void OnDestroy()
    {
        Debug.Log("Удаление объекта");
        Destroy(gameObject);
    }
}
//   private static readonly (float, float)[] CubicQuadrature =
//{(-0.7745966F, 0.5555556F), (0, 0.8888889F), (0.7745966F, 0.5555556F)};
//public float arclength(float t) => integrate(x => tangentmagnitude(x), 0, t);

//public static float integrate(func<float, float> f, in float lowerbound, in float uppedbound)
//{
//    var sum = 0f;
//    foreach (var (arg, weight) in cubicquadrature)
//    {
//        var t = mathf.lerp(lowerbound, uppedbound, mathf.inverselerp(-1, 1, arg));
//        sum += weight * f(t);
//    }

//    return sum * (uppedbound - lowerbound) / 2;
//}

//private float parameter(float length)
//{
//    float t = 0 + length / arclength(1);
//    float lowerbound = 0f;
//    float upperbound = 1f;

//    for (int i = 0; i < 100; ++i)
//    {
//        float f = arclength(t) - length;

//        if (mathf.abs(f) < 0.01f)
//            break;

//        float derivative = tangentmagnitude(t);
//        float candidatet = t - f / derivative;

//        if (f > 0)
//        {
//            upperbound = t;
//            if (candidatet <= 0)
//                t = (upperbound + lowerbound) / 2;
//            else
//                t = candidatet;
//        }
//        else
//        {
//            lowerbound = t;
//            if (candidatet >= 1)
//                t = (upperbound + lowerbound) / 2;
//            else
//                t = candidatet;
//        }
//    }
//    return t;


  

   

