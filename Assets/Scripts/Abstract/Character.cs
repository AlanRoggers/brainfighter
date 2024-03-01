using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public Vector2 feetsPos;
    public Vector2 feetsSize;
    protected Command currentCommand;
    public string Name;
    protected Dictionary<AnimationStates, Attack> attacks;
    protected Dictionary<AnimationStates, Action> actions;
    protected HandlerComp components;
    protected Coroutine attackCoroutine;
    protected override void Awake()
    {
        base.Awake();
        try
        {
            components = new HandlerComp
            {
                Machine = GetComponent<StateMachine>(),
                Messenger = new Messenger(),
                ContactLayer = gameObject.layer == 6 ? 7 : 6,
                Physics = GetComponent<Rigidbody2D>(),
                CircleHitBox = GetComponentInChildren<CircleCollider2D>(),
                collision = new OverlapDetector()
            };
            components.Machine.CurrentClip = AnimationStates.Iddle;
            InputManager input = GetComponent<InputManager>();
            input.Messenger = components.Messenger;
            input.Machine = components.Machine;
        }
        catch (Exception e)
        {
            Debug.Log($"[Error en la inicialización del personaje] {e}");
        }
        InitActions();
        InitAttacks();

    }
    protected virtual void Update()
    {
        components.Messenger.InGround = components.collision.GroundDetection(transform, feetsPos, feetsSize, 3);
    }
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        currentCommand?.Transitions(components.Machine, components.Messenger);

    }
    protected virtual void StopWalk()
    {
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
    protected virtual IEnumerator Attack(Attack attack)
    {
        Debug.Log($"[Corrutina] {attack.ActionStates[0]}");
        currentCommand = attack;
        components.Messenger.Attacking = true;
        components.Messenger.RequestedAttack = AnimationStates.Null;
        components.Machine.ChangeAnimation(attack.ActionStates[0]);
        yield return new WaitForEndOfFrame();
        while (components.Machine.CurrentTime() < 1.0f)
        {
            if (attack.TimesDamageApplied > 0)
            {
                Collider2D enemy = components.collision.AttackHit(components.ContactLayer, components.CircleHitBox);
                if (enemy)
                {
                    enemy.GetComponent<CharacterHealth>().ReduceHealth(attack.Damage);
                    attack.TimesDamageApplied--;
                }
            }
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => components.Machine.CurrentTime() > 1.0f);
        components.Messenger.Attacking = false;
        components.Messenger.ComboCount = 0;
        components.Messenger.InCooldown = true;
        yield return new WaitForSecondsRealtime(attack.CoolDown);
        components.Messenger.InCooldown = false;

    }
    protected abstract void InitAttacks();
    protected abstract void InitActions();
    protected void InterruptCoroutine()
    {
        if (components.Messenger.Hurt)
            components.Messenger.Attacking = false;
        components.Messenger.InCooldown = false;
    }
}