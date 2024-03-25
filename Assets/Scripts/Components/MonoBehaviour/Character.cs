
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool AcceptInput;
    private PlayerState futureState;
    public PlayerState FutureStateSet { set => futureState = value; }
    public bool Reset;

    #region AI
    public bool IsAI;
    [HideInInspector] public PPOAgent Agent;
    #endregion

    #region Character Properties
    public int Health { get; private set; }
    public int HealthSet { set => Health = value; }
    public int Resistance { get; private set; }
    public int ResistanceSet { set => Resistance = value; }
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
    [HideInInspector] public Attack AttackReceived;
    [HideInInspector] public bool EntryAttack;
    [HideInInspector] public bool OnColdoown;
    [HideInInspector] public Coroutine CoolDownCor { get; private set; }
    public Coroutine CoolDownSet { set => CoolDownCor = value; }
    [HideInInspector] public int HitsChained;
    public Vector2 Spawn;

    #endregion

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Physics = GetComponent<Rigidbody2D>();
        Body = GetComponent<BoxCollider2D>();
        Agent = GetComponent<PPOAgent>();

    }
    private void Start()
    {
        CharacterLayer = (int)Mathf.Pow(2, gameObject.layer);
        States = new StateStorage();
        OverlapDetector = new Overlap();
        Friction = new() { friction = 1 };
        Physics.sharedMaterial = Friction;
        Health = 50;
        Resistance = 50;
        transform.localPosition = Spawn;
        CurrentState = States.Iddle;
        CurrentState.OnEntry(this);
    }
    void Update()
    {
        // if (Physics.velocity.y > 15)
        // {
        //     Debug.Log($"Algo ocurrio: {CurrentState}");

        // }
        // Debug.Log(MathF.Round(Physics.velocity.x, 2, MidpointRounding.AwayFromZero));
        // Manejar las acciones requeridoas por la IA o por el jugador
        futureState = !Reset ? (IsAI ? CurrentState.InputAIHandler(this, Agent) : CurrentState.InputHandler(this)) : futureState;

        if (futureState != null)
        {
            Reset = false;
            CurrentState.OnExit(this);
            CurrentState = futureState;
            CurrentState.OnEntry(this);
        }

        Orientation();
    }
    private void FixedUpdate()
    {
        CurrentState.Update(this);
    }
    // private void OnDrawGizmos()
    // {
    //     // OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
    //     // OverlapDetector.DrawEnemyOverlapping(Body, gameObject.layer == 6 ? 7 : 6);
    //     // Gizmos.DrawWireCube((Vector2)transform.position + enemyDetectorPos, enemyDetectorSize);
    //     // OverlapDetector.DrawHitBox(gameObject.layer == 6 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1"), Hitbox);
    // }
    public IEnumerator CoolDown(float cd)
    {
        // Debug.Log("Corrutina entrante");
        OnColdoown = true;
        yield return new WaitForSeconds(cd);
        // Debug.Log("Corrutina de salida");
        OnColdoown = false;
        CoolDownCor = null;
    }
    private void Orientation()
    {
        float signDistance = Mathf.Sign(transform.localPosition.x - EnemyTransform.localPosition.x);

        bool canTurn = CurrentState == States.Walk
            || CurrentState == States.Back
            || CurrentState == States.Jump
            || CurrentState == States.Fall
            || CurrentState == States.Iddle;

        if (Mathf.Sign(transform.localScale.x) == signDistance && canTurn)
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
    public void ResetParams()
    {
        CurrentState.OnExit(this);
        AttackReceived = null;
        EntryAttack = false;
        OnColdoown = false;
        HitsChained = 0;
        Physics.velocity = Vector2.zero;
        if (CoolDownCor != null)
        {
            StopCoroutine(CoolDownCor);
            CoolDownSet = null;
        }
        HealthSet = 100;
        // HealthSet = Random.Range(40, 30);
        ResistanceSet = 50;
        Friction.friction = 1; // Tal vez no
        // transform.localPosition = Spawn;
        Animator.speed = 1;
        FutureStateSet = States.Iddle;
        Reset = true;
    }
}
