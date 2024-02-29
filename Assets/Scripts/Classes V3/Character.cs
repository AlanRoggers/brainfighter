using System;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public string Name;
    protected List<Attack> attacks;
    protected Dictionary<AnimationStates, State> animations;
    protected HandlerComp components;
    protected override void Awake()
    {
        base.Awake();
        components = new HandlerComp
        {
            Machine = GetComponent<StateMachine>(),
            Messenger = new Messenger(),
            ContactLayer = gameObject.layer == 6 ? 7 : 6,
            Physics = GetComponent<Rigidbody2D>()
        };
        InitAnimations();
        InitAttacks();
        components.Machine.CurrentState = animations[AnimationStates.Iddle];
    }
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        if (components.Machine.CurrentState != null)
        {
            components.Machine.CurrentState.Transitions(components.Machine, components.Messenger, animations);
            Debug.Log("[CurrentState]");
        }

    }
    protected virtual void Walk(int direction)
    {
        components.Messenger.Iddle = false;
        Debug.Log($"[Caminando]");
        if (Mathf.Sign(direction) != MathF.Sign(components.Physics.velocity.x))
            components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);
        float force = components.Physics.mass * 500 * (10 - Mathf.Abs(components.Physics.velocity.x)) * Time.deltaTime * Mathf.Sign(direction);
        components.Physics.AddForce(new Vector2(force, 0));
        components.Messenger.Walking = direction;
    }
    protected virtual void StopWalk()
    {
        Debug.Log("[No caminar]");
        components.Messenger.Iddle = true;
        components.Messenger.Walking = 0;
        components.Physics.velocity = Vector2.zero;
    }
    protected virtual void Jump()
    {
        Debug.Log("[Saltar]");
    }
    protected virtual void Dash()
    {
        Debug.Log("[Dash]");
    }
    protected virtual void Block()
    {
        Debug.Log("[Bloquear]");
    }
    protected virtual void Crouch()
    {
        Debug.Log("[Agacharse]");
    }
    protected abstract void InitAttacks();
    protected abstract void InitAnimations();
    protected virtual void DoDamage(int damage, Collider2D enemy)
    {
        enemy.GetComponent<CharacterHealth>().ReduceHealth(damage);
    }
}
