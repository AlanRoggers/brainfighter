using System;
using System.Collections;
using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    // public Vector2 a;
    public float b;
    public PhysicsMaterial2D Friction;
    public int Healt { get; private set; }
    public LayerMask CharacterLayer;
    public bool ArtificialInteligence;
    public Transform EnemyTransform;
    [HideInInspector] public int HitsChained;
    [HideInInspector] public Coroutine CoolDownCor;
    [HideInInspector] public Coroutine HurtCor;
    [HideInInspector] public bool OnColdoown;
    [HideInInspector] public float LastVelocity;
    public CircleCollider2D Hitbox;
    [HideInInspector] public int Layer;
    public readonly Overlap OverlapDetector = new();
    public Collider2D Body;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D Physics;
    public readonly StateStorage States = new();
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
        Healt = 100;
    }
    void Start()
    {

    }
    void Update()
    {
        print(Healt);
        currentState.Update(this);
        PlayerState auxiliar = currentState.InputHandler(this);
        if (auxiliar != null)
        {
            if (gameObject.layer == 6)
                Debug.Log(auxiliar);
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
    public void EntryAttack(int damage, Vector2 force, float hitStun, bool hitFreeze)
    {
        if (currentState == States.Back || currentState == States.Block)
        {
            currentState.OnExit(this);
            States.Block.AttackFreeze = hitFreeze;
            States.Block.Force = force;
            currentState = States.Block;
            currentState.OnEntry(this);
            return;
        }

        if (Healt - damage >= 0)
            Healt -= damage;
        else
            Healt = 0;

        currentState.OnExit(this);
        States.Hurt.AttackForce = force;
        States.Hurt.AttackStun = hitStun;
        States.Hurt.AttackFreeze = hitFreeze;
        currentState = States.Hurt;

        currentState.OnEntry(this);
    }
}
