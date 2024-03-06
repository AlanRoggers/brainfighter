using System;
using System.Collections;
using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    public bool ArtificialInteligence;
    private readonly Vector2 enemyDetectorSize = new(1, 3);
    private Vector2 enemyDetectorPos = new(0.3f, 2.5f);
    public Transform EnemyTransform;
    [HideInInspector] public int HitsChained;
    [HideInInspector] public Coroutine CoolDownCor;
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
    }
    void Start()
    {

    }
    void Update()
    {
        currentState.Update(this);
        PlayerState auxiliar = currentState.InputHandler(this);
        if (auxiliar != null)
        {
            currentState.OnExit(this);
            currentState = auxiliar;
            currentState.OnEntry(this);
        }
        Orientation();
    }
    private void OnDrawGizmos()
    {
        // OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
        Gizmos.DrawWireCube((Vector2)transform.position + enemyDetectorPos, enemyDetectorSize);
    }
    public IEnumerator CoolDown(float cd)
    {
        Debug.Log("Corrutina entrante");
        OnColdoown = true;
        yield return new WaitForSeconds(cd);
        Debug.Log("Corrutina de salida");
        OnColdoown = false;
    }
    private void Orientation()
    {
        float signDistance = MathF.Sign(transform.localPosition.x - EnemyTransform.localPosition.x);
        if (MathF.Sign(transform.localScale.x) == signDistance && TurnPermitedStates())
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            enemyDetectorPos *= new Vector2(-1, 1);
        }

    }

    private bool TurnPermitedStates()
    {
        return currentState == States.Walk || currentState == States.Back || currentState == States.Jump || currentState == States.Fall ||
            currentState == States.Crouch || currentState == States.Iddle;
    }
}
