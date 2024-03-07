using System;
using System.Collections;
using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    public bool ArtificialInteligence;
    public int Health { get; private set; }
    public int Resistance { get; private set; }
    public readonly StateStorage States = new();
    public readonly Overlap OverlapDetector = new();
    public LayerMask CharacterLayer;
    public Transform EnemyTransform;
    public CircleCollider2D Hitbox;
    public Collider2D Body;
    [HideInInspector] public PhysicsMaterial2D Friction;
    [HideInInspector] public int HitsChained;
    [HideInInspector] public Coroutine CoolDownCor;
    [HideInInspector] public Coroutine HurtCor;
    [HideInInspector] public bool OnColdoown;
    [HideInInspector] public float LastVelocity;
    [HideInInspector] public int Layer;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D Physics;
    [HideInInspector] public PlayerState currentState;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Physics = GetComponent<Rigidbody2D>();
        Body = GetComponent<BoxCollider2D>();
        currentState = States.Iddle;
        Layer = (int)Mathf.Pow(2, gameObject.layer);
        Friction = new()
        {
            friction = 1
        };
        Physics.sharedMaterial = Friction;
        Health = 100;
        Resistance = 50;
    }
    void Update()
    {
        if (gameObject.layer == 6)
            Debug.Log(Resistance);
        currentState.Update(this);
        PlayerState auxiliar = currentState.InputHandler(this);
        if (auxiliar != null)
        {
            currentState.OnExit(this);
            currentState = auxiliar;
            currentState.OnEntry(this);
        }
        Orientation();
        // Debug.Log(Mathf.Pow(2, gameObject.layer));
    }
    private void OnDrawGizmos()
    {
        // OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
        // OverlapDetector.DrawEnemyOverlapping(transform, Layer == 64 ? 128 : 64);
        // Gizmos.DrawWireCube((Vector2)transform.position + enemyDetectorPos, enemyDetectorSize);
        OverlapDetector.DrawHitBox(Layer == 64 ? 128 : 64, Hitbox);
    }
    public IEnumerator CoolDown(float cd)
    {
        // Debug.Log("Corrutina entrante");
        OnColdoown = true;
        yield return new WaitForSeconds(cd);
        // Debug.Log("Corrutina de salida");
        OnColdoown = false;
    }
    private void Orientation()
    {
        float signDistance = MathF.Sign(transform.localPosition.x - EnemyTransform.localPosition.x);
        if (MathF.Sign(transform.localScale.x) == signDistance && TurnPermitedStates())
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
    private bool TurnPermitedStates()
    {
        return currentState == States.Walk || currentState == States.Back || currentState == States.Jump || currentState == States.Fall ||
            currentState == States.Crouch || currentState == States.Iddle;
    }
    public void EntryAttack(AttackV5 attack)
    {
        if (currentState == States.Back || currentState == States.Block)
        {
            currentState.OnExit(this);
            if (Resistance > 0)
            {
                States.Block.AttackReceived = attack;
                currentState = States.Block;
                currentState.OnEntry(this);
                return;
            }
            else
            {
                currentState = States.Stun;
                currentState.OnEntry(this);
                return;
            }
        }

        if (Health - attack.Damage >= 0)
            Health -= attack.Damage;
        else
            Health = 0;

        currentState.OnExit(this);
        States.Hurt.AttackReceived = attack;
        currentState = States.Hurt;

        currentState.OnEntry(this);
    }
    public void ReduceResistance(int damage)
    {
        if (Resistance - damage > 0)
            Resistance -= damage;
        else
            Resistance = 0;
    }
    public void IncrementResistance(int value)
    {
        if (Resistance + value < 50)
            Resistance += value;
        else
            Resistance = 50;
    }
}
