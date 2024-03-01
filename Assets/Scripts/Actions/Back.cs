using System;
using System.Collections.Generic;
using UnityEngine;

public class Back : Action
{
    private readonly float maxSpeed;
    private readonly float maxForce;
    public Back(float maxSpeed, float maxForce, List<AnimationStates> actionStates) : base(actionStates)
    {
        this.maxSpeed = maxSpeed;
        this.maxForce = maxForce;
    }
    public override void Execute(HandlerComp components)
    {
        Debug.Log($"[Retrocediendo]");

        if (MathF.Sign(components.Physics.velocity.x) > 0)
            components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);

        float force = components.Physics.mass * maxForce * (maxSpeed - MathF.Abs(components.Physics.velocity.x)) * Time.deltaTime * -1;

        components.Physics.AddForce(new Vector2(force, 0));
    }
}
