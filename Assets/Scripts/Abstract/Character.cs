using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.Sentis.Layers;
using UnityEngine;

public abstract class Character : Agent
{
    public HandlerComp Components { get; protected set; }
    public Character Enemy;
    public string Name;
    public Vector2 forceHelper;
    public Transform TurnReferece;
    private readonly Vector2 xSignHelper = new(-1, 1);
    private LayerMask ground;
    private Vector2 bottomPos;
    private Vector2 bottomSize;
    protected Command currentCommand;
    protected Dictionary<AnimationStates, Attack> attacks;
    protected Dictionary<AnimationStates, Action> actions;
    protected Coroutine attackCoroutine;
    protected Coroutine enemyHurtCoroutine;
    protected override void Awake()
    {
        base.Awake();
        try
        {
            Components = new HandlerComp
            (
                GetComponentInChildren<CircleCollider2D>(),
                new OverlapDetector(),
                GetComponent<StateMachine>(),
                Mathf.Pow(2, gameObject.layer) == 64 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1"),
                new Messenger(),
                GetComponent<Rigidbody2D>(),
                transform,
                new CharacterHealth(100),
                GetComponent<BoxCollider2D>()
            );
            ground = LayerMask.GetMask("Ground");
            Components.Machine.CurrentClip = AnimationStates.Iddle;
            InputManager input = GetComponent<InputManager>();
            input.Messenger = Components.Messenger;
            input.Machine = Components.Machine;
        }
        catch (Exception e)
        {
            Debug.Log($"[Error en la inicialización del personaje] {e}");
        }
        InitActions();
        InitAttacks();
        bottomSize = new Vector2(1.1f, 0.01f);
        bottomPos = new Vector2(0.075f, 0.0425f);

    }
    protected virtual void Update()
    {
        if (Components.Messenger.InGround)
        {
            // Debug.Log("[En piso]");
        }
    }
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        currentCommand?.Transitions(Components.Machine, Components.Messenger);
        // Estados virtuales
        Iddle();
        Fall();
        Orientation();
    }
    protected virtual void FixedUpdate()
    {
        Components.Messenger.InGround = Components.Collision.GroundDetection(transform, bottomPos, bottomSize, ground) && !Components.Messenger.Jumping;
        if (Components.Messenger.InGround)
            Components.Messenger.Falling = false;

    }
    protected virtual void StopWalk()
    {
        // Debug.Log("[StopWalk]");
        Components.Messenger.Walking = 0;
        Components.Physics.velocity = new Vector2(0, Components.Physics.velocity.y);
    }
    protected virtual IEnumerator Attack(Attack attack)
    {
        // Debug.Log($"[Corrutina] {attack.ActionStates[0]}");
        currentCommand = attack;
        Components.Messenger.Attacking = true;
        Components.Messenger.RequestedAttack = AnimationStates.Null;
        Components.Machine.ChangeAnimation(attack.ActionStates[0]);

        yield return new WaitForEndOfFrame();

        if (transform.localScale.x > 0)
            Components.Physics.AddForce(attack.Inertia, ForceMode2D.Impulse);
        else
            Components.Physics.AddForce(attack.Inertia * xSignHelper, ForceMode2D.Impulse);

        int times = attack.TimesDamageApplied;
        while (Components.Machine.CurrentTime() < 1.0f)
        {
            if (times > 0)
            {
                if (Components.Collision.AttackHit(Components.ContactLayer, Components.CircleHitBox) && Components.CircleHitBox.enabled)
                {
                    if (enemyHurtCoroutine != null)
                        StopCoroutine(enemyHurtCoroutine);
                    Components.CircleHitBox.enabled = false;
                    if (!Enemy.Components.Messenger.Blocking)
                    {
                        Debug.Log("[Enemigo golpeado]");
                        enemyHurtCoroutine = StartCoroutine(Enemy.Hurt(attack.HitStun, attack.HitFreeze, attack.Damage, attack.Force));
                        if (attack.HitFreeze)
                        {
                            Vector2 current = Components.Physics.velocity;
                            Components.Machine.Freeze(Components);
                            yield return new WaitForSeconds(0.25f);
                            Components.Machine.UnFreeze(Components, current);
                        }
                        times--;
                    }
                }
            }
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Components.Machine.CurrentTime() > 1.0f);
        Components.Messenger.Attacking = false;
        Components.Messenger.ComboCount = 0;
        Components.Messenger.InCooldown = true;
        yield return new WaitForSecondsRealtime(attack.CoolDown);
        Components.Messenger.InCooldown = false;

    }
    protected abstract void InitAttacks();
    protected abstract void InitActions();
    protected void InterruptCoroutine()
    {
        if (Components.Messenger.Hurt)
            Components.Messenger.Attacking = false;
        Components.Messenger.InCooldown = false;
    }
    private void Iddle()
    {
        bool iddle = !Components.Messenger.Attacking && !Components.Messenger.Hurt &&
        Components.Messenger.Walking == 0 && !Components.Messenger.Jumping &&
        !Components.Messenger.Falling;

        // Estado virtual
        if (iddle)
        {
            // Debug.Log("[Iddle]");
            Components.Machine.ChangeAnimation(AnimationStates.Iddle);
            currentCommand = null;
            StopWalk();
        }
    }
    private void Fall()
    {
        // Debug.Log("[Cayendo]");
        Components.Messenger.Falling = Components.Physics.velocity.y < -1 && !Components.Messenger.Hurt && !Components.Messenger.Attacking;

        if (Components.Messenger.Falling)
        {
            if (currentCommand != actions[AnimationStates.Fall])
            {
                Components.Machine.ChangeAnimation(actions[AnimationStates.Fall].ActionStates[0]);
                // actions[AnimationStates.Fall].Execute(Components);
                currentCommand = actions[AnimationStates.Fall];
                Components.Messenger.Jumping = false;
            }
        }
    }
    private void Orientation()
    {
        float signDistance = MathF.Sign(transform.localPosition.x - TurnReferece.localPosition.x);
        if (MathF.Sign(transform.localScale.x) == signDistance && !Components.Messenger.Attacking)
        {
            Debug.Log("[Orientacion]");
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

    }
    public IEnumerator Hurt(float stun, bool freeze, int damage, Vector2 force)
    {
        Debug.Log($"[{Name} Herido]");
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Components.Messenger.Hurt = true;
        Components.Health.ReduceHealth(damage);
        currentCommand = null;
        if (!Components.Messenger.Crouching)
            Components.Machine.ChangeAnimation(AnimationStates.Damage);
        else
            Components.Machine.ChangeAnimation(AnimationStates.DamageWhileCrouch);
        yield return new WaitForEndOfFrame();

        if (transform.localScale.x > 0)
            Components.Physics.AddForce(force, ForceMode2D.Impulse);
        else
            Components.Physics.AddForce(force * xSignHelper, ForceMode2D.Impulse);

        if (freeze)
        {
            Vector2 current = Components.Physics.velocity;
            Components.Machine.Freeze(Components);
            yield return new WaitForSeconds(0.25f);
            Components.Machine.UnFreeze(Components, current);
        }



        yield return new WaitUntil(() => Components.Machine.CurrentTime() > 1.0f);
        yield return new WaitForSeconds(stun);
        Components.Messenger.Hurt = false;
    }

    void OnDrawGizmos()
    {
        if (Components != null)
        {
            if (Components.Messenger.InGround)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawWireCube((Vector2)transform.position + bottomPos, bottomSize);
            // if (Components.msng.EnemyCollider != null)
            //     Gizmos.color = Color.green;
            // else
            //     Gizmos.color = Color.red;

            // if (damage.enabled)
            //     Gizmos.DrawWireSphere(damage.bounds.center, damage.radius);
        }
        else
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((Vector2)transform.position + bottomPos, bottomSize);
        }
    }
}