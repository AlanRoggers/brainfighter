using System;
using System.Collections;
using Unity.MLAgents;
using UnityEngine;

public class Character : MonoBehaviour
{
    private PlayerState futureState;

    #region AIEvents
    public Character Enemy;
    #endregion

    #region AI
    public bool IsAI;
    [HideInInspector] public State RequestedBehaviourAction;
    [HideInInspector] public State RequestedMotionAction;
    #endregion

    #region Character Properties
    public int Health { get; private set; }
    public int Resistance { get; private set; }
    public readonly StateStorage States = new();
    public readonly Overlap OverlapDetector = new();
    public Transform EnemyTransform;
    public CircleCollider2D Hitbox;
    public Collider2D Body;
    [HideInInspector] public int CharacterLayer;
    [HideInInspector] public PhysicsMaterial2D Friction;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D Physics;
    [HideInInspector] public PlayerState currentState;
    #endregion

    #region Character Handlers
    [HideInInspector] public bool EndGame;
    [HideInInspector] public Attack AttackReceived;
    [HideInInspector] public bool EntryAttack;
    [HideInInspector] public bool AttackBlocked;
    [HideInInspector] public bool Damaged;
    [HideInInspector] public bool OnColdoown;
    [HideInInspector] public bool Stuned;
    [HideInInspector] public int HitsChained;
    [HideInInspector] public float LastVelocity;
    [HideInInspector] public Coroutine CoolDownCor;
    [HideInInspector] public Coroutine HurtCor;
    #endregion

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Physics = GetComponent<Rigidbody2D>();
        Body = GetComponent<BoxCollider2D>();
        Friction = new()
        {
            friction = 1
        };
        Physics.sharedMaterial = Friction;
    }
    private void Start()
    {
        EndGame = false;
        currentState = States.Iddle;
        CharacterLayer = (int)Mathf.Pow(2, gameObject.layer);
        Health = 100;
        Resistance = 50;
        currentState.OnEntry(this);
    }
    void Update()
    {
        if (gameObject.layer == 6)
            Debug.Log(currentState);
        if (!EndGame)
        {

            if (!IsAI)
                futureState = currentState.InputHandler(this);
            else
                futureState = currentState.InputAIHandler(this);

            if (futureState != null)
            {
                currentState.OnExit(this);
                currentState = futureState;
                currentState.OnEntry(this);
            }
            Orientation();
        }
    }
    private void FixedUpdate()
    {
        if (!EndGame)
            currentState.Update(this);
    }
    private void OnDrawGizmos()
    {
        // OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
        // OverlapDetector.DrawEnemyOverlapping(Body, gameObject.layer == 6 ? 7 : 6);
        // Gizmos.DrawWireCube((Vector2)transform.position + enemyDetectorPos, enemyDetectorSize);
        OverlapDetector.DrawHitBox(CharacterLayer == 64 ? 128 : 64, Hitbox);
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
            currentState == States.Iddle;
    }
    public void SetAttack(Attack attack)
    {
        AttackReceived = attack;
        EntryAttack = true;
    }
    public void ReduceHealth(int value)
    {
        if (Health - value >= 0)
            Health -= value;
        else
            Health = 0;
        // OnWin.Invoke(gameObject.layer != 6);
    }
    public void ReduceResistance(int value)
    {
        if (Resistance - value > 0)
            Resistance -= value;
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
    public void Reset()
    {
        Physics.velocity = Vector2.zero;
        currentState = States.Iddle;
        currentState.OnEntry(this);
        Friction.friction = 1;
        Health = 100;
        Resistance = 50;
        EndGame = false;
    }
}
