using System.Collections;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool CanCheckGround;
    public CircleCollider2D damage;
    private bool gameStarted;
    private Components components;
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask enemyLayer;
    private Vector2 feetsPosition;
    private Vector2 feetsSize;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        feetsPosition = new Vector2(0, 0.297f);
        feetsSize = new Vector2(1.4f, 0.05f);
        groundLayer = LayerMask.GetMask("Ground");
        gameStarted = true;
        CanCheckGround = true;
    }
    void Update()
    {
        components.msng.enemy = DamageDetection();
    }
    void FixedUpdate()
    {
        if (!CanCheckGround)
            StartCoroutine(IGNORE_GROUND());

        components.msng.IsOnGround = GroundDetection() && CanCheckGround;

        if (components.msng.IsOnGround && components.msng.IsJumping)
            components.msng.IsJumping = false;
    }
    void OnDrawGizmos()
    {
        if (gameStarted)
        {
            // if (components.msng.IsOnGround)
            //     Gizmos.color = Color.green;
            // else
            //     Gizmos.color = Color.red;

            // Gizmos.DrawWireCube((Vector2)transform.localPosition + feetsPosition, feetsSize);
            if (components.msng.enemy != null)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(damage.bounds.center, damage.radius);
        }
    }
    private bool GroundDetection()
    {
        return Physics2D.OverlapBox((Vector2)transform.localPosition + feetsPosition, feetsSize, 0f, groundLayer) != null;
    }
    private Collider2D DamageDetection()
    {
        return Physics2D.OverlapCircle(damage.bounds.center, damage.radius, enemyLayer);
    }
    private IEnumerator IGNORE_GROUND()
    {
        yield return new WaitForSeconds(0.2f);
        CanCheckGround = true;
    }
}
