using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] private BoxCollider2D player;
    [SerializeField] private BoxCollider2D enemy;
    [SerializeField] private BoxCollider2D normalHitbox;
    [SerializeField] private GameObject Reference;
    private Components components;
    private Coroutine dash_chance;
    void Awake()
    {
        components = GetComponent<Components>();
    }

    void Update()
    {
        if (transform.localScale.x != KeepLooking())
            components.msng.NeedTurn = true;
    }

    #region Walk
    private int maxSpeed = 5;
    private float walkForce = 100;
    public void Walk(int direction)
    {
        bool canDo = components.msng.IsOnGround && !(
            components.msng.IsAttacking || components.msng.IsCrouching ||
            components.msng.IsDashing || components.msng.IsDashingBack ||
            components.msng.IsTakingDamage || components.msng.IsTurning ||
            components.msng.IsJumping
        );

        if (canDo)
        {
            if (components.msng.IsJumping)
                print("Entro al canDo");
            if (direction != 0)
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
            else if (components.msng.IsWalking || components.msng.IsRunning)
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
        // Como esta acción solo potencia el caminar, la acción de caminar valida a esta acción también
        bool canDo = components.msng.IsWalking;

        if (canDo && !components.msng.IsRunning)
        {
            components.msng.IsWalking = false;
            components.msng.IsRunning = true;
        }
    }
    #endregion

    #region Jump
    // El salto se podría mejorar aplicando fuerza en función de si lleva velocidad o no
    [SerializeField] private float jumpxForce;
    [SerializeField] private float jumpForce;
    private float lastJump = 0f;
    private readonly float waitingBetweenJumps = 0.5f;
    public void Jump()
    {
        bool canDo = components.msng.IsOnGround && Time.time - lastJump >= waitingBetweenJumps && !(
            components.msng.IsAttacking || components.msng.IsTakingDamage ||
            components.msng.IsDashing || components.msng.IsDashingBack || components.msng.IsTurning
        );

        if (canDo)
        {
            float asd = components.phys.velocity.x != 0 ? jumpxForce : 0;
            asd *= Mathf.Sign(components.phys.velocity.x);
            components.phys.velocity = Vector2.zero;
            components.phys.AddForce(new Vector2(asd, jumpForce), ForceMode2D.Impulse);
            lastJump = Time.time;
            components.msng.IsJumping = true;
            components.coll.CanCheckGround = false;
            Physics2D.IgnoreCollision(player, enemy);
        }
    }
    #endregion

    #region Crouch
    public void Crouch()
    {
        bool canDo = components.msng.IsOnGround && !(
            components.msng.IsAttacking || components.msng.IsTurning ||
            components.msng.IsDashing || components.msng.IsDashingBack ||
            components.msng.IsTakingDamage
        );

        if (canDo)
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
    public void Dash(bool negativeForce = true)
    {
        bool canDo = components.msng.IsOnGround && !(
            components.msng.IsCrouching || components.msng.IsAttacking ||
            components.msng.IsRunning || components.msng.IsDashing ||
            components.msng.IsDashingBack || components.msng.IsTakingDamage);

        if (!components.msng.DashTimer && canDo)
        {
            components.msng.DashTimer = true;
            dash_chance = StartCoroutine(DASH_CAHNCE());
        }
        else if (canDo)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashing = true;
            Physics2D.IgnoreCollision(player, enemy);
            components.phys.AddForce(new Vector2(negativeForce ? -dashForce : dashForce, 0), ForceMode2D.Impulse);
            StartCoroutine(RESTORE_IGNORED_COLLISION());
            StopCoroutine(dash_chance);
            components.msng.DashTimer = false;
        }
    }
    private IEnumerator DASH_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.DashTimer = false;
    }
    private IEnumerator RESTORE_IGNORED_COLLISION()
    {
        yield return new WaitWhile(() => components.msng.IsDashing || components.msng.IsDashingBack);
        Physics2D.IgnoreCollision(player, enemy, false);
    }
    #endregion

    #region DashBack
    public void DashBack(bool negativeForce = false)
    {
        bool canDo = components.msng.IsOnGround && !(
            components.msng.IsCrouching || components.msng.IsAttacking ||
            components.msng.IsRunning || components.msng.IsDashingBack ||
            components.msng.IsDashing || components.msng.IsTakingDamage
        );

        if (!components.msng.DashBackTimer && canDo)
        {
            components.msng.DashBackTimer = true;
            dash_chance = StartCoroutine(DASHBACK_CAHNCE());
        }
        else if (canDo)
        {
            components.msng.IsWalking = false;
            components.msng.IsDashingBack = true;
            Physics2D.IgnoreCollision(player, enemy);
            components.phys.AddForce(new Vector2(negativeForce ? dashForce : -dashForce, 0), ForceMode2D.Impulse);
            StartCoroutine(RESTORE_IGNORED_COLLISION());
            StopCoroutine(dash_chance);
            components.msng.DashBackTimer = false;
        }
    }
    private IEnumerator DASHBACK_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        components.msng.DashBackTimer = false;
    }

    #endregion}

    #region Turn
    [SerializeField] private BoxCollider2D crouchHitbox;
    private int KeepLooking()
    {
        // Aqui la comprobación se hace con las transiciones de los estados, es decir, esta función solo debería detechat
        // que se ocupa girar y no estar validando si se puede girar por lo tanto esta primera validación no va

        if (!components.msng.IsAttacking)
        {
            if (transform.position.x - Reference.transform.position.x <= 0 && transform.localScale.x < 0)
                return 1;
            else if (transform.position.x - Reference.transform.position.x > 0 && transform.localScale.x > 0)
                return -1;
            else return (int)transform.localScale.x;
        }
        else return (int)transform.localScale.x;
    }
    #endregion
}
