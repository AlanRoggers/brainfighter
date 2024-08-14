using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineComp : MonoBehaviour
{
    [SerializeField] StateList _stateList;

    AbstractState _currentState;

    Dictionary<StateType, AbstractState> _states;

    void Awake()
    {
        InitializeStates(_stateList);
        _states.TryGetValue(StateType.Idle, out _currentState);
    }

    void Start()
    {
        _currentState?.Start();
    }

    void Update()
    {
        _currentState?.Update();
    }

    void FixedUpdate()
    {
        if (_currentState != null && !_currentState.FixedOneExec)
            _currentState.FixedUpdate();
    }

    /// <summary>
    /// Change the state that is playing by the state machine
    /// </summary>
    /// <param name="newState">Type of the state that needs to be changed</param>
    public void ChangeState(StateType newStateType)
    {
        _states.TryGetValue(newStateType, out var newState);

        if (newState != null && _currentState.CanTransitionTo(newStateType))
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Start();

            if (newState.FixedOneExec)
                StartCoroutine(OneFixedUpdate());
        }
    }

    /// <summary>
    /// Create the states for the gameobject that is attached
    /// </summary>
    /// <param name="stateList">Is a gameobject used to store a list of types of states</param>
    public void InitializeStates(StateList stateList)
    {
        _states = new();

        foreach (var state in stateList.States)
        {
            Debug.Log($"[StateMachine] Creating {state} state");
            _states.Add(state, StateCreator.CreateState(state));
        }
    }

    /// <summary>
    /// Executes a method that involves physics according to FixedUpdate but only one time
    /// </summary>
    /// <returns></returns>
    IEnumerator OneFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        _currentState.FixedUpdate();
    }
}

