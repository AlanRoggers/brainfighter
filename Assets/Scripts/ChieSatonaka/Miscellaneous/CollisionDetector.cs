using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool CanCheckGround;
    private bool gameStarted;
    private Components components;
    private LayerMask groundLayer;
    [SerializeField]
    private Vector2 hitboxPosition;
    [SerializeField]
    private Vector2 hitboxSize;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        hitboxPosition = new Vector2(0, 0.297f);
        hitboxSize = new Vector2(1.4f, 0.05f);
        groundLayer = LayerMask.GetMask("Ground");
        gameStarted = true;
        CanCheckGround = true;
    }
    void FixedUpdate()
    {
        if (!CanCheckGround)
            StartCoroutine(IGNORE_GROUND());

        components.msng.onGround = GroundDetection() && CanCheckGround;

        if (components.msng.onGround && components.msng.isJumping)
            components.msng.isJumping = false;
    }
    void OnDrawGizmos()
    {
        if (gameStarted)
        {
            if (components.msng.onGround)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.localPosition + hitboxPosition, hitboxSize);
        }
    }
    private bool GroundDetection()
    {
        return Physics2D.OverlapBox((Vector2)transform.localPosition + hitboxPosition, hitboxSize, 0f, groundLayer) != null;
    }
    private IEnumerator IGNORE_GROUND()
    {
        yield return new WaitForSeconds(0.2f);
        CanCheckGround = true;
    }
}
