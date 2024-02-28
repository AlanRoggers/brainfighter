using System;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    protected AnimationMachine animationMachine;
    protected Messenger msng;
    public bool ControlledByUser;
    public string Name { get; private set; }
    public int Health { get; private set; }
    protected List<Attack> attacks;
    protected Rigidbody2D phys;
    protected override void Awake()
    {
        base.Awake();
        phys = GetComponent<Rigidbody2D>();
        animationMachine = GetComponent<AnimationMachine>();
        msng = new Messenger();
    }
    protected abstract void AssignAttacks();
    protected void Walk(int direction, float maxSpeed)
    {
        // Animación
        bool initAnimation = animationMachine.CurrentState != AnimationStates.StartWalking && animationMachine.CurrentState != AnimationStates.StartGoingBackwards;
        bool movementAnimation = animationMachine.CurrentState == AnimationStates.Walk || animationMachine.CurrentState == AnimationStates.GoingBackwards;
        Debug.Log($"iniciar animación {initAnimation}. animacion de movimiento {movementAnimation}");
        if (initAnimation && !movementAnimation)
        {
            Debug.Log($"[Walk] Inicio de la animación {animationMachine.CurrentState}");
            if (MathF.Sign(direction) == MathF.Sign(transform.localScale.x))
                animationMachine.Transition(AnimationStates.StartWalking, isFinalState: false);
            else
                animationMachine.Transition(AnimationStates.StartGoingBackwards, isFinalState: false);
        }
        else if (!movementAnimation)
        {
            Debug.Log("[Walk] Loop de la animación");
            if (animationMachine.GetCurrentAnimationTime() > 1.0)
            {
                if (animationMachine.CurrentState == AnimationStates.StartWalking)
                    animationMachine.Transition(AnimationStates.Walk, isFinalState: false);
                else if (animationMachine.CurrentState == AnimationStates.StartGoingBackwards)
                    animationMachine.Transition(AnimationStates.GoingBackwards, isFinalState: false);
            }
        }

        // Lógica del movimiento
        float force = phys.mass * 200 * (maxSpeed - Mathf.Abs(phys.velocity.x)) * Time.deltaTime;
        phys.AddForce(new Vector2(force * direction, 0));
    }
    protected void StopWalk()
    {
        animationMachine.Transition(AnimationStates.Iddle, isFinalState: false);
        phys.velocity = Vector2.zero;
    }
    // protected void Jump(float jumpForce, float moveSpeed)
    // {

    // }
    // protected void Block()
    // {

    // }
    // protected void Crouch()
    // {

    // }
    // protected void Dash()
    // {

    // }
}
