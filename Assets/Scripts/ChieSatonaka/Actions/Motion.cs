using System.Collections;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D normalHitbox;
    [SerializeField]
    private BoxCollider2D crouchHitbox;
    private Components components;
    void Awake()
    {
        Application.targetFrameRate = 60; //Mover de aquí
        components = GetComponent<Components>();
    }

    #region Walk
    private readonly int maxSpeed = 5;
    private readonly float walkForce = 100;
    public void Walk(int direction)
    {
        if (!components.msng.isDashing)
        {
            if (direction != 0)
            {
                if (!components.msng.isKicking && !components.msng.isCrouching && !components.msng.isRunning)
                {
                    components.msng.isWalking = true;
                    if (Mathf.Abs(components.phys.velocity.x) < maxSpeed)
                    {
                        float speed = components.phys.velocity.x + (1 * walkForce * direction * Time.deltaTime);
                        components.phys.velocity = new Vector2(speed, components.phys.velocity.y);
                    }
                    else components.phys.velocity = new Vector2(maxSpeed * direction, components.phys.velocity.y);
                }
                else
                {
                    components.msng.isWalking = false;
                }
            }
            else
            {
                components.msng.isWalking = false;
                if (!components.msng.isKicking) // Esto hay que repensarlo
                    components.phys.velocity = new Vector2(0, components.phys.velocity.y);
                // print("Se esta cancelando el movimiento");
            }
        }
    }
    #endregion

    #region Run
    private readonly int runMaxSpeed = 7;
    private readonly float runWalkForce = 150;
    public void Run(bool negativeVelocity)
    {
        if (!components.msng.isKicking && !components.msng.isCrouching && components.msng.onGround)
        {
            if (components.msng.isWalking) components.msng.isWalking = false;
            components.msng.isRunning = true;
            if (Mathf.Abs(components.phys.velocity.x) < runMaxSpeed)
            {
                float speed = components.phys.velocity.x + (negativeVelocity ? -runWalkForce * Time.deltaTime : runWalkForce * Time.deltaTime);
                components.phys.velocity = new Vector2(speed, components.phys.velocity.y);
            }
            else components.phys.velocity = new Vector2(negativeVelocity ? -runMaxSpeed : runMaxSpeed, components.phys.velocity.y);
        }
    }
    #endregion

    #region Jump
    private readonly int jumpForce = 800;
    private float lastJump = 0f;
    private readonly float waitingBetweenJumps = 0.5f;
    public void Jump()
    {
        bool canJump = components.msng.onGround && Time.time - lastJump >= waitingBetweenJumps && !components.msng.isKicking && !components.msng.isCrouching;

        if (canJump)
        {
            components.phys.AddForce(new Vector2(0, jumpForce));
            lastJump = Time.time;
            components.msng.isJumping = true;
            components.coll.CanCheckGround = false;
        }
    }
    #endregion

    #region Crouch
    public void Crouch()
    {
        if (components.msng.onGround && !components.msng.isKicking)
        {
            Hitboxes(false, true);
            components.msng.isCrouching = true;
            components.phys.velocity = Vector2.zero;
        }

    }
    public void Hitboxes(bool normal, bool crouch)
    {
        normalHitbox.enabled = normal;
        crouchHitbox.enabled = crouch;
    }
    public void StandUp()
    {
        Hitboxes(true, false);
        components.msng.isCrouching = false;
    }
    #endregion

    #region Dash
    [SerializeField]
    private float dashForce;
    public void Dash(bool negativeForce)
    {
        bool canDash = components.msng.onGround && !components.msng.isCrouching &&
                        !components.msng.isKicking && !components.msng.isRunning &&
                        !components.msng.isDashing;

        if (canDash)
        {
            components.msng.isWalking = false;
            components.msng.isDashing = true;
            components.phys.AddForce(new Vector2(negativeForce ? -dashForce : dashForce, 0));
        }
    }
    public IEnumerator NO_DASHING()
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        components.msng.isDashing = false;
    }
    #endregion
}
