using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public string Name;
    protected Dictionary<AnimationStates, Attack> attacks;
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
            Physics = GetComponent<Rigidbody2D>(),
            CircleHitBox = GetComponentInChildren<CircleCollider2D>(),
            collision = new OverlapDetector()
        };
        InitAnimations();
        InitAttacks();
        components.Machine.CurrentState = animations[AnimationStates.Iddle];
    }
    protected virtual void Update()
    {
        components.Messenger.Iddle = components.Messenger.Walking == 0 && !components.Messenger.Attacking &&
                                        components.Messenger.Hurt
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
        try
        {
            components.Messenger.Attacking = true;
            components.Machine.ChangeAnimation(animations[attack.initState]);
            yield return new WaitForEndOfFrame();
            while (components.Machine.CurrentTime() < 1.0f)
            {
                if (attack.timesDamageApplied > 0)
                {
                    Collider2D enemy = components.collision.AttackHit(components.ContactLayer, components.CircleHitBox);
                    if (enemy)
                    {
                        enemy.GetComponent<CharacterHealth>().ReduceHealth(attack.Damage);
                        attack.timesDamageApplied--;
                    }
                }
                yield return null;
            }
            components.Machine.ChangeAnimation(attack.endState);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => components.Machine.CurrentTime() > 1.0f);
            components.Messenger.Attacking = false;
            components.Messenger.InCooldown = true;
            yield return new WaitForSecondsRealtime(attack.CoolDown);
            components.Messenger.InCooldown = false;
        }
        finally
        {
            if (components.Messenger.Hurt)
                components.Messenger.Attacking = false;

            components.Messenger.InCooldown = false;
        }
    }
    protected abstract void InitAttacks();
    protected abstract void InitAnimations();
}