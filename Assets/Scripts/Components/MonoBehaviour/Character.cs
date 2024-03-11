using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private PlayerState futureState;
    #region AI
    public bool IsAI;
    [HideInInspector] public State RequestedBehaviourAction;
    [HideInInspector] public State RequestedMotionAction;
    private bool reset;
    #endregion

    #region Character Properties
    public int Health { get; private set; }
    public int Resistance { get; private set; }
    public StateStorage States { get; private set; }
    public Overlap OverlapDetector { get; private set; }
    public Transform EnemyTransform;
    public CircleCollider2D Hitbox;
    public Collider2D Body { get; private set; }
    public int CharacterLayer { get; private set; }
    [HideInInspector] public PhysicsMaterial2D Friction;
    public Animator Animator { get; private set; }
    public Rigidbody2D Physics { get; private set; }
    public PlayerState CurrentState { get; private set; }
    #endregion

    #region Character Handlers
    [HideInInspector] public bool EndGame;
    [HideInInspector] public Attack AttackReceived;
    [HideInInspector] public bool EntryAttack;
    [HideInInspector] public bool OnColdoown;
    [HideInInspector] public int HitsChained;
    [HideInInspector] public Coroutine CoolDownCor { get; private set; }
    #endregion

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Physics = GetComponent<Rigidbody2D>();
        Body = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        EndGame = false;
        CharacterLayer = (int)Mathf.Pow(2, gameObject.layer);
        Health = 100;
        Resistance = 50;
        States = new StateStorage();
        OverlapDetector = new Overlap();
        Friction = new()
        {
            friction = 1
        };
        Physics.sharedMaterial = Friction;
        CurrentState = States.Iddle;
        CurrentState.OnEntry(this);
        States.Dead.OnDead += FinshGame;
    }
    void Update()
    {
        // if (gameObject.layer == 6)
        //     Debug.Log(RequestedBehaviourAction);
        if (!EndGame)
        {
            if (!reset)
            {

                if (!IsAI)
                    futureState = CurrentState.InputHandler(this);
                else
                    futureState = CurrentState.InputAIHandler(this);
            }
            else reset = false;

            if (futureState != null)
            {
                CurrentState.OnExit(this);
                CurrentState = futureState;
                CurrentState.OnEntry(this);
            }
            Orientation();
        }
    }
    private void FixedUpdate()
    {
        if (!EndGame)
            CurrentState.Update(this);
    }
    private void OnDrawGizmos()
    {
        // OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
        // OverlapDetector.DrawEnemyOverlapping(Body, gameObject.layer == 6 ? 7 : 6);
        // Gizmos.DrawWireCube((Vector2)transform.position + enemyDetectorPos, enemyDetectorSize);
        // OverlapDetector.DrawHitBox(CharacterLayer == 64 ? 128 : 64, Hitbox);
    }
    public IEnumerator CoolDown(float cd)
    {
        // Debug.Log("Corrutina entrante");
        OnColdoown = true;
        yield return new WaitForSeconds(cd);
        // Debug.Log("Corrutina de salida");
        OnColdoown = false;
        CoolDownCor = null;
    }
    public void SetCoolDownCor(Coroutine routine) => CoolDownCor = routine;
    private void Orientation()
    {
        float signDistance = MathF.Sign(transform.localPosition.x - EnemyTransform.localPosition.x);

        bool canTurn = CurrentState == States.Walk
            || CurrentState == States.Back
            || CurrentState == States.Jump
            || CurrentState == States.Fall
            || CurrentState == States.Iddle;

        if (MathF.Sign(transform.localScale.x) == signDistance && canTurn)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
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
        CurrentState.OnExit(this);
        EndGame = false;
        AttackReceived = null;
        EntryAttack = false;
        OnColdoown = false;
        HitsChained = 0;
        Physics.velocity = Vector2.zero;
        if (CoolDownCor != null)
        {
            StopCoroutine(CoolDownCor);
            CoolDownCor = null;
        }
        Health = 100;
        Resistance = 50;
        Friction.friction = 1; // Tal vez no
        futureState = States.Iddle;
        reset = true;
    }
    private void FinshGame(bool _) => EndGame = true;
}
