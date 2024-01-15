using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StateMachine
{
    public Dictionary<Type, IState> stateMap;
    private IState _currentState;
    private Truck _truck;
    public MonoBehaviour dynamicOdject;
    
    public StateMachine(Truck truck) {
        _truck = truck;   
    }

   public void Start()
    {
        this.InitState();
        this.SetStateByDefault();
    }
    public void InitState()
    {
        stateMap = new Dictionary<Type, IState>();
        this.stateMap[typeof(MovingState)] = new MovingState(_truck);
        this.stateMap[typeof(WaitingState)] = new WaitingState(_truck);
        this.stateMap[typeof(Loading)] = new Loading(_truck);
        this.stateMap[typeof(IdleState)] = new IdleState(_truck);
        this.stateMap[typeof(TruckOutState)] = new TruckOutState(_truck);
    }

  
     public void Update()
    {
        if (this._currentState != null)
        {
            this._currentState.Update();
        }
    }

    public void SetState(IState newState)
    {
        if (this._currentState != null & this._currentState!= newState)
        {
            this._currentState.Exit();
            
        }
       if (this._currentState != newState) {
            this._currentState = newState;
            this._currentState.Enter();
        }
        
    }
     
    public IState GetState<T>() where T : IState
    {
        var type = typeof(T);
        return this.stateMap[type];
    }

    private void SetStateByDefault()
    {
        setStateMoving();
    }
    public void setIdleState()
    {
        var state = this.GetState<IdleState>();
        this.SetState(state);
    }
    public void setStateMoving()
    {
        var state = this.GetState<MovingState>();
        this.SetState(state);
    }

    public void setStateWaiting()
    {
        var state = this.GetState<WaitingState>();
        this.SetState(state);
    }

    public void setStateLoading()
    {
        var state = this.GetState<Loading>();
        this.SetState(state);
    }

    public void setStateOut()
    {
        var state = this.GetState<TruckOutState>();
        this.SetState(state);
    }

    public IState getCurrentState() { 
        return this._currentState;
    }
}
