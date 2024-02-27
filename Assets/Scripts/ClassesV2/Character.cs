using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public bool ControlledByUser;
    public string Name { get; private set; }
    public int Health { get; private set; }
    protected List<Attack> attacks;
    private Rigidbody2D phys;
    protected override void Awake()
    {
        base.Awake();
        phys = GetComponent<Rigidbody2D>();
    }
    protected abstract void AssignAttacks();
    protected void Walk(int direction, float maxSpeed)
    {
        float force = phys.mass * 200 * (maxSpeed - Mathf.Abs(phys.velocity.x)) * Time.deltaTime;
        phys.AddForce(new Vector2(force * direction, 0));
    }
    protected void StopWalk()
    {
        phys.velocity = Vector2.zero;
    }
    // protected void Jump(float jumpForce, float moveSpeed)
    // {

    // }
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
