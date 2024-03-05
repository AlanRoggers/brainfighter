using UnityEngine;

public class CharacterV5 : MonoBehaviour
{
    public CircleCollider2D Hitbox;
    [HideInInspector] public int Layer;
    public readonly Overlap OverlapDetector = new();
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D Physics;
    public readonly StateStorage States = new();
    private PlayerState currentState;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Physics = GetComponent<Rigidbody2D>();
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
    }
    private void OnDrawGizmos()
    {
        OverlapDetector.DrawGroundDetection(GetComponent<BoxCollider2D>(), LayerMask.GetMask("Ground"));
    }
}
