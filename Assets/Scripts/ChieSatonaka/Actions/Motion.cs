using System.Collections;
using UnityEngine;

public class Motion : MonoBehaviour
{
    private Coroutine dash_chance;
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
    private int maxSpeed = 5;
    private float walkForce = 100;
    public void Walk(int direction)
    {
        if (!components.msng.IsDashing && !components.msng.IsDashingBack)
        {
            if (direction != 0)
            {
                if (!components.msng.IsKicking && !components.msng.IsCrouching)
                {
                    components.msng.IsWalking = !components.msng.IsRunning;
                    maxSpeed = components.msng.IsRunning ? 7 : 5;
                    walkForce = components.msng.IsRunning ? 150 : 100;
                    if (Mathf.Abs(components.phys.velocity.x) < maxSpeed)
                    {
                        float speed = components.phys.velocity.x + (1 * walkForce * direction * Time.deltaTime);
                        components.phys.velocity = new Vector2(speed, components.phys.velocity.y);
                    }
                    else components.phys.velocity = new Vector2(maxSpeed * direction, components.phys.velocity.y);
                }
                else
                {
                    components.msng.IsWalking = false;
                }
            }
            else
            {
                components.msng.IsWalking = false;
                if (!components.msng.IsKicking) // Esto hay que repensarlo
                    components.phys.velocity = new Vector2(0, components.phys.velocity.y);
            }
        }
    }
    #endregion

    #region Run
    public void Run()
    {
        if (!components.msng.IsKicking && !components.msng.IsCrouching && components.msng.IsOnGround)
        {
            if (components.msng.IsWalking) components.msng.IsWalking = false;
            components.msng.IsRunning = true;
        }
    }
    #endregion

    #region Jump
    private readonly int jumpForce = 800;
    private float lastJump = 0f;
    private readonly float waitingBetweenJumps = 0.5f;
    public void Jump()
    {
        bool canJump = components.msng.IsOnGround && Time.time - lastJump >= waitingBetweenJumps && !components.msng.IsKicking && !components.msng.IsCrouching;

        if (canJump)
        {
            components.phys.AddForce(new Vector2(0, jumpForce));
            lastJump = Time.time;
            components.msng.IsJumping = true;
            components.coll.CanCheckGround = false;
        }
    }
    #endregion

    #region Crouch
    public void Crouch()
    {
        if (components.msng.IsOnGround && !components.msng.IsKicking)
        {
            Hitboxes(false, true);
            components.msng.IsCrouching = true;
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
        components.msng.IsCrouching = false;
    }
    #endregion

    #region Dash
    [SerializeField]
    private float dashForce;
    public void Dash(bool negativeForce)
    {
        bool canDash = components.msng.IsOnGround && !components.msng.IsCrouching &&
                        !components.msng.IsKicking && !components.msng.IsRunning &&
                        !components.msng.IsDashing && !components.msng.IsDashingBack;

        if (!components.msng.dashTimer && canDash)
        {
            components.msng.dashTimer = true;
            dash_chance = StartCoroutine(DASH_CAHNCE());
        }
        else if (canDash)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashing = true;
            components.phys.AddForce(new Vector2(negativeForce ? -dashForce : dashForce, 0));
            StopCoroutine(dash_chance);
            components.msng.dashTimer = false;
        }
    }
    private IEnumerator DASH_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.dashTimer = false;
    }

    public IEnumerator NO_DASHING()
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        components.msng.IsDashing = false;
    }
    #endregion

    #region DashBack
    public void DashBack(bool positiveForce)
    {
        bool canDash = components.msng.IsOnGround && !components.msng.IsCrouching &&
                        !components.msng.IsAttacking && !components.msng.IsRunning &&
                        !components.msng.IsDashingBack && !components.msng.IsDashing;

        if (!components.msng.dashBackTimer && canDash)
        {
            components.msng.dashBackTimer = true;
            dash_chance = StartCoroutine(DASHBACK_CAHNCE());
        }
        else if (canDash)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashingBack = true;
            components.phys.AddForce(new Vector2(positiveForce ? dashForce : -dashForce, 0));
            StopCoroutine(dash_chance);
            components.msng.dashBackTimer = false;
        }
    }
    private IEnumerator DASHBACK_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.dashBackTimer = false;
    }

    public IEnumerator NO_DASHINGBACK()
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        components.msng.IsDashingBack = false;
    }
    #endregion
}
