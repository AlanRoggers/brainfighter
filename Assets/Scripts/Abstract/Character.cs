using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected abstract void InitAttacks();
    protected abstract void InitActions();

    #region Variables Publicas
    public Character Enemy;
    public HandlerComp Components { get; protected set; }
    public int Health { get; private set; }
    public int Resistance { get; private set; }
    public string Name;
    public Transform TurnReferece;
    public Vector2 forceHelper;
    #endregion

    #region Variables Privadas
    private LayerMask ground;
    private readonly Vector2 xSignHelper = new(-1, 1);
    private readonly Vector2 sizeColl = new(1.098f, 4.328f);
    private readonly Vector2 offsetColl = new(0.071f, 2.605f);
    private Vector2 bottomPos;
    private Vector2 bottomSize;
    private Command auxiliar = null;
    #endregion

    #region Variables Protegidas
    protected Command currentCommand;
    protected Coroutine attackC;
    protected Coroutine enemyHurtC;
    protected Dictionary<AnimationStates, Action> actions;
    protected Dictionary<AnimationStates, Attack> attacks;
    #endregion

    #region Métodos de Unity
    protected virtual void Awake()
    {
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
                GetComponent<BoxCollider2D>()
            );
            Health = 100;
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
        if (gameObject.layer == 6)
            Debug.Log($"Estado de Mexico {currentCommand}");
        // Condiciones para que el personaje bloquee en caso de que apriete el Input (Input Manager involucrado tambien)
        Components.Messenger.DistanceForBlock = BlockOrNot();

        if (!Components.Messenger.Crouching && Components.CharacterColl.size != sizeColl)
        {
            Components.CharacterColl.size = sizeColl;
            Components.CharacterColl.offset = offsetColl;
        }
        if (gameObject.layer == 7)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Components.Messenger.Attacking = true;
                attackC = StartCoroutine(Attack(attacks[AnimationStates.SpecialPunch]));
            }
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

    protected void OnDrawGizmos()
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
    #endregion

    #region Acciones Virtuales
    private void Iddle()
    {
        bool iddle = !Components.Messenger.Attacking && !Components.Messenger.Hurt &&
        Components.Messenger.Walking == 0 && !Components.Messenger.Jumping &&
        !Components.Messenger.Falling && !Components.Messenger.Blocking &&
        !Components.Messenger.Crouching;

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
        if (MathF.Sign(transform.localScale.x) == signDistance && !Components.Messenger.Attacking && !Components.Messenger.Blocking)
        {
            Debug.Log("[Orientacion]");
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

    }

    private bool BlockOrNot()
    {
        // Debug.Log($"Distancia entre los changos: {Vector2.Distance(transform.localPosition, Enemy.transform.localPosition)}");
        return Vector2.Distance(transform.localPosition, Enemy.transform.localPosition) < 3.2f && Enemy.Components.Messenger.Attacking;
    }

    protected virtual void StopWalk()
    {
        // Debug.Log("[StopWalk]");
        Components.Messenger.Walking = 0;
        Components.Physics.velocity = new Vector2(0, Components.Physics.velocity.y);
    }

    public IEnumerator Hurt(float stun, bool freeze, int damage, float freezeTime, Vector2 force)
    {
        Debug.Log($"[{Name} Herido]");
        if (attackC != null)
            StopCoroutine(attackC);
        Components.Messenger.Hurt = true;
        Components.Physics.velocity = Vector2.zero;
        ReduceHealth(damage);

        if (Components.Messenger.Crouching && auxiliar == null)
            auxiliar = currentCommand;

        currentCommand = null;
        if (!Components.Messenger.Crouching)
            Components.Machine.ChangeAnimation(AnimationStates.Damage);
        else
        {
            Debug.Log("daño agachado");
            Components.Machine.ChangeAnimation(AnimationStates.DamageWhileCrouch);
        }
        yield return new WaitForEndOfFrame();

        if (transform.localScale.x < 0)
            Components.Physics.AddForce(force, ForceMode2D.Impulse);
        else
            Components.Physics.AddForce(force * xSignHelper, ForceMode2D.Impulse);

        yield return new WaitForEndOfFrame();

        if (freeze)
        {
            Vector2 current = Components.Physics.velocity;
            Components.Machine.Freeze(Components);
            yield return new WaitForSeconds(freezeTime);
            Components.Machine.UnFreeze(Components, current);
        }

        yield return new WaitUntil(() => Components.Machine.CurrentTime() > 1.0f);
        yield return new WaitForSeconds(stun);
        Components.Messenger.Hurt = false;

        if (auxiliar != null)
            currentCommand = auxiliar;

        auxiliar = null;
    }

    #endregion

    protected void InterruptAttack()

    {
        if (Components.Messenger.Hurt)
            Components.Messenger.Attacking = false;
        Components.Messenger.InCooldown = false;
    }

    protected void InterruptHurt()
    {

    }
    private void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;
    }

    private void ReduceResistance(int value)
    {
        Resistance -= value;
    }

    private void GainResistance()
    {
        Resistance += 10;
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
            if (times > 0 && !Enemy.Components.Messenger.Blocking)
            {
                if (Components.Collision.AttackHit(Components.ContactLayer, Components.CircleHitBox) && Components.CircleHitBox.enabled)
                {
                    if (enemyHurtC != null)
                        StopCoroutine(enemyHurtC);
                    Components.CircleHitBox.enabled = false;
                    if (!Enemy.Components.Messenger.Blocking)
                    {
                        Debug.Log("[Enemigo golpeado]");
                        enemyHurtC = StartCoroutine(Enemy.Hurt(attack.HitStun, attack.HitFreeze, attack.Damage, attack.HitFreezeTimer, attack.Force));
                        if (attack.HitFreeze)
                        {
                            Vector2 current = Components.Physics.velocity;
                            Components.Machine.Freeze(Components);
                            yield return new WaitForSeconds(attack.HitFreezeTimer);
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
        yield return new WaitForSeconds(attack.CoolDown);
        Components.Messenger.InCooldown = false;

    }
}