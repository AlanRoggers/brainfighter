using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public Transform TurnReferece;
    public LayerMask ground;
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
        if (components.Messenger.InGround)
        {
            Debug.Log("[En piso]");
        }
    }
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        currentCommand?.Transitions(components.Machine, components.Messenger);

        bool iddle = !components.Messenger.Attacking && !components.Messenger.Hurt && components.Messenger.Walking == 0 && !components.Messenger.Jumping
                        && !components.Messenger.Falling;

        components.Messenger.Falling = components.Physics.velocity.y < 0 && !components.Messenger.Hurt;

        if (iddle)
        {
            // Debug.Log("[Iddle]");
            components.Machine.ChangeAnimation(AnimationStates.Iddle);
            currentCommand = null;
            StopWalk();
        }

        if (components.Messenger.Falling)
        {
            if (currentCommand != actions[AnimationStates.Fall])
            {
                components.Machine.ChangeAnimation(actions[AnimationStates.Fall].ActionStates[0]);
                actions[AnimationStates.Fall].Execute(components);
                currentCommand = actions[AnimationStates.Fall];
                components.Messenger.Jumping = false;
            }
        }

        // Estado virtual turn;
        float signDistance = MathF.Sign(transform.localPosition.x - TurnReferece.localPosition.x);
        if (MathF.Sign(transform.localScale.x) == signDistance)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
    protected virtual void FixedUpdate()
    {
        components.Messenger.InGround = components.collision.GroundDetection(transform, feetsPos, feetsSize, ground) && !components.Messenger.Jumping;
        if (components.Messenger.InGround)
            components.Messenger.Falling = false;

    }
    protected virtual void StopWalk()
    {
        Debug.Log("[StopWalk]");
        components.Messenger.Walking = 0;
        components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);
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