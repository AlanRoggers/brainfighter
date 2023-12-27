using UnityEngine;

public class MotionActions : MonoBehaviour
{
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
        if (direction != 0)
        {
            if (!components.msng.isKicking && !components.msng.isCrouching)
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
            if (!components.msng.isKicking)
                components.phys.velocity = new Vector2(0, components.phys.velocity.y);
            // print("Se esta cancelando el movimiento");
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
            // Ajustar o prender el hitbox de cuando esta agachado
            components.msng.isCrouching = true;
            components.phys.velocity = Vector2.zero;
        }

    }
    public void StandUp()
    {
        // Ajustar o prender el hitbox de cuando esta agachado
        components.msng.isCrouching = false;
    }
    #endregion
}
