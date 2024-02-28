using System;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public LayerMask layerMask;
    public Vector2 feetsPosition;
    public Vector2 feetsSize;
    protected CharacterMachine animationMachine;
    protected Messenger msng;
    public bool ControlledByUser;
    public string Name { get; private set; }
    public int Health { get; private set; }
    protected List<Attack> attacks;
    protected Rigidbody2D phys;
    protected override void Awake()
    {
        base.Awake();
        phys = GetComponent<Rigidbody2D>();
        animationMachine = GetComponent<CharacterMachine>();
        msng = new Messenger();
    }
    protected abstract void AssignAttacks();
    protected void Walk(int direction, float maxSpeed)
    {
        animationMachine.WalkAnimation(direction);
        float force = phys.mass * 200 * (maxSpeed - Mathf.Abs(phys.velocity.x)) * Time.deltaTime;
        phys.AddForce(new Vector2(force * direction, 0));
    }
    protected void StopWalk()
    {
        animationMachine.Transition(AnimationStates.Iddle, isFinalState: false);
        phys.velocity = new Vector2(0, phys.velocity.y);
    }
    protected virtual void FixedUpdate()
    {
        msng.InGround = groundDetection(layerMask);
    }
    protected void Jump(float jumpForce, float moveSpeed)
    {
        // float asd = components.phys.velocity.x != 0 ? jumpxForce : 0;
        // asd *= Mathf.Sign(components.phys.velocity.x);
        animationMachine.JumpAnimation();
        phys.velocity = Vector2.zero;
        phys.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        // lastJump = Time.time;
        // components.msng.IsJumping = true;
        // components.coll.CanCheckGround = false;
        // Physics2D.IgnoreCollision(player, enemy);
    }
    private bool groundDetection(LayerMask ground) =>
        Physics2D.OverlapBox((Vector2)transform.localPosition + feetsPosition, new Vector2(GetComponent<BoxCollider2D>().size.x, feetsSize.y), 0f, layerMask) != null;

    void OnDrawGizmos()
    {
        if (msng.InGround)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube((Vector2)transform.localPosition + feetsPosition, new Vector2(GetComponent<BoxCollider2D>().size.x, feetsSize.y));
        // if (components.msng.EnemyCollider != null)
        //     Gizmos.color = Color.green;
        // else
        //     Gizmos.color = Color.red;

        // if (damage.enabled)
        //     Gizmos.DrawWireSphere(damage.bounds.center, damage.radius);

    }


    // protected void Block()
    // {

    // }
    // protected void Crouch()
    // {

    // }
    // protected void Dash()
    // {

    // }
}
