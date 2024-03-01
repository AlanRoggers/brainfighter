using System.Collections.Generic;
using UnityEngine;

public class Jump : Action
{
    private readonly float jumpForce;
    public Jump(float jumpForce, List<AnimationStates> actionStates) : base(actionStates)
    {
        this.jumpForce = jumpForce;
    }

    public override void Execute(HandlerComp components)
    {
        // float asd = components.phys.velocity.x != 0 ? jumpxForce : 0;
        // asd *= Mathf.Sign(components.phys.velocity.x);
        Debug.Log("[Saltando]");
        components.Physics.velocity = Vector2.zero;
        components.Physics.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        // lastJump = Time.time;
        // components.coll.CanCheckGround = false;
        // Physics2D.IgnoreCollision(player, enemy);
    }
}
