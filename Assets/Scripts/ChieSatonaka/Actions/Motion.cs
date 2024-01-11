using System.Collections;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D crouchHitbox;
    [SerializeField]
    private BoxCollider2D normalHitbox;
    private Components components;
    private Coroutine dash_chance;
    void Awake()
    {
        components = GetComponent<Components>();
    }

    #region Walk
    private int maxSpeed = 5;
    private float walkForce = 100;
    public void Walk(int direction)
    {
        if (!components.msng.IsDashing && !components.msng.IsDashingBack && !components.msng.IsTakingDamage && !components.msng.IsAttacking)
        {
            if (direction != 0)
            {
                if (!components.msng.IsAttacking && !components.msng.IsCrouching)
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
            }
            else if (components.msng.IsWalking)
            {
                components.msng.IsWalking = false;
                components.phys.velocity = new Vector2(0, components.phys.velocity.y);
            }
        }
    }
    #endregion

    #region Run
    public void Run()
    {
        if (!components.msng.IsAttacking && !components.msng.IsCrouching && components.msng.IsOnGround && !components.msng.IsTakingDamage)
        {
            if (components.msng.IsWalking) components.msng.IsWalking = false;

            if (components.phys.velocity.x > 0) // Esta comprobación va dar bug asi como esta
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
        bool canJump = components.msng.IsOnGround && Time.time - lastJump >= waitingBetweenJumps &&
                        !components.msng.IsAttacking && !components.msng.IsCrouching &&
                        !components.msng.IsTakingDamage;

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
        if (components.msng.IsOnGround && !components.msng.IsAttacking && !components.msng.IsTakingDamage)
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
    [SerializeField] private float dashForce;
    public void Dash(bool negativeForce)
    {
        bool canDash = components.msng.IsOnGround && !components.msng.IsCrouching &&
                        !components.msng.IsAttacking && !components.msng.IsRunning &&
                        !components.msng.IsDashing && !components.msng.IsDashingBack &&
                        !components.msng.IsTakingDamage;

        if (!components.msng.DashTimer && canDash)
        {
            components.msng.DashTimer = true;
            dash_chance = StartCoroutine(DASH_CAHNCE());
        }
        else if (canDash)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashing = true;
            components.phys.AddForce(new Vector2(negativeForce ? -dashForce : dashForce, 0), ForceMode2D.Impulse);
            StopCoroutine(dash_chance);
            components.msng.DashTimer = false;
        }
    }
    private IEnumerator DASH_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.DashTimer = false;
    }
    #endregion

    #region DashBack
    public void DashBack(bool positiveForce)
    {
        bool canDash = components.msng.IsOnGround && !components.msng.IsCrouching &&
                        !components.msng.IsAttacking && !components.msng.IsRunning &&
                        !components.msng.IsDashingBack && !components.msng.IsDashing &&
                        !components.msng.IsTakingDamage;

        if (!components.msng.DashBackTimer && canDash)
        {
            components.msng.DashBackTimer = true;
            dash_chance = StartCoroutine(DASHBACK_CAHNCE());
        }
        else if (canDash)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashingBack = true;
            components.phys.AddForce(new Vector2(positiveForce ? dashForce : -dashForce, 0), ForceMode2D.Impulse);
            StopCoroutine(dash_chance);
            components.msng.DashBackTimer = false;
        }
    }
    private IEnumerator DASHBACK_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.DashBackTimer = false;
    }

    public IEnumerator NO_DASHINGBACK()
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        components.msng.IsDashingBack = false;
    }
    #endregion}
}
