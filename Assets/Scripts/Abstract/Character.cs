using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    private readonly Vector2 xSignHelper = new(-1, 1);
    public Vector2 forceHelper;
    public Vector2 inertiaHelper;
    public Transform TurnReferece;
    public LayerMask ground;
    private Vector2 bottomPos;
    private Vector2 bottomSize;
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
                collision = new OverlapDetector(),
                Transform = transform
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
        bottomSize = new Vector2(1.1f, 0.01f);
        bottomPos = new Vector2(12.29f, -0.15f);

    }
    protected virtual void Update()
    {
        if (components.Messenger.InGround)
        {
            // Debug.Log("[En piso]");
        }
    }
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        currentCommand?.Transitions(components.Machine, components.Messenger);
        // Estados virtuales
        Iddle();
        Fall();
        Orientation();
        Damaged();

    }
    protected virtual void FixedUpdate()
    {
        components.Messenger.InGround = components.collision.GroundDetection(transform, bottomPos, bottomSize, ground) && !components.Messenger.Jumping;
        if (components.Messenger.InGround)
            components.Messenger.Falling = false;

    }
    protected virtual void StopWalk()
    {
        // Debug.Log("[StopWalk]");
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
        if (transform.localScale.x > 0)
            components.Physics.AddForce(attack.Inertia, ForceMode2D.Impulse);
        else
            components.Physics.AddForce(attack.Inertia * xSignHelper, ForceMode2D.Impulse);
        while (components.Machine.CurrentTime() < 1.0f)
        {
            if (attack.TimesDamageApplied > 0)
            {
                Collider2D enemy = components.collision.AttackHit(components.ContactLayer, components.CircleHitBox);
                if (enemy)
                {
                    Debug.Log("[Enemigo golpeado]");
                    // enemy.GetComponent<CharacterHealth>().ReduceHealth(attack.Damage);
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
    private void Iddle()
    {
        bool iddle = !components.Messenger.Attacking && !components.Messenger.Hurt &&
        components.Messenger.Walking == 0 && !components.Messenger.Jumping &&
        !components.Messenger.Falling;

        // Estado virtual
        if (iddle)
        {
            // Debug.Log("[Iddle]");
            components.Machine.ChangeAnimation(AnimationStates.Iddle);
            currentCommand = null;
            StopWalk();
        }
    }
    private void Fall()
    {
        // Debug.Log("[Cayendo]");
        components.Messenger.Falling = components.Physics.velocity.y < 0 && !components.Messenger.Hurt && !components.Messenger.Attacking;

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
    }
    private void Orientation()
    {
        float signDistance = MathF.Sign(transform.localPosition.x - TurnReferece.localPosition.x);
        if (MathF.Sign(transform.localScale.x) == signDistance && !components.Messenger.Attacking)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
    private void Damaged()
    {
        if (components.Messenger.Hurt)
        {
            if (!components.Messenger.Crouching)
                components.Machine.ChangeAnimation(AnimationStates.Damage);
            else
                components.Machine.ChangeAnimation(AnimationStates.DamageWhileCrouch);
            currentCommand = null;
        }
    }
    void OnDrawGizmos()
    {
        if (components != null)
        {
            if (components.Messenger.InGround)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawWireCube((Vector2)transform.localPosition + bottomPos, bottomSize);
            // if (components.msng.EnemyCollider != null)
            //     Gizmos.color = Color.green;
            // else
            //     Gizmos.color = Color.red;

            // if (damage.enabled)
            //     Gizmos.DrawWireSphere(damage.bounds.center, damage.radius);
        }
        else
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((Vector2)transform.localPosition + bottomPos, bottomSize);
        }
    }
}