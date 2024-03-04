using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    public PlayerState currentState;
    public Animator animator;
    public static Iddle iddle = new();
    public static WalkV5 walk = new();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = iddle;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentState.InputHandler(this);
        currentState.Update(this);
    }
}
