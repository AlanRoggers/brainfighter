using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    private PlayerState currentState;
    public Dictionary<State, PlayerState> States;
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        try
        {
            CreateStates();
            currentState = States[State.IDDLE];

        }
        catch { Debug.Log("Todo personaje debe tener un estado Iddle por lo menos"); }
    }
    private void CreateStates()
    {
        States = new Dictionary<State, PlayerState>()
        {
            { State.IDDLE, new Iddle() },
            { State.WALK, new WalkV5() }
        };
    }
    void Start()
    {

    }
    void Update()
    {
        currentState.Update(this);
        PlayerState auxiliar = currentState.InputHandler(this);
        if (auxiliar != null)
        {
            currentState.OnExit(this);
            currentState = auxiliar;
            currentState.OnEntry(this);
        }
    }
}
