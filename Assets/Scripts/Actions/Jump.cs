using System.Collections.Generic;
using UnityEngine;

public class Jump : Action
{
    private readonly float jumpForce;
    public Jump(float jumpForce, List<AnimationStates> actionStates) : base(actionStates) => this.jumpForce = jumpForce;
    public override void Execute(HandlerComp components)
    {
        Vector2 force = new(components.Physics.velocity.x != 0 ? Mathf.Round(components.Physics.velocity.x) : 0, jumpForce);
        components.Physics.velocity = Vector2.zero;
        components.Physics.AddForce(force, ForceMode2D.Impulse);
    }
}
