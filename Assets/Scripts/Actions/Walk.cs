using System;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Action
{
    private readonly float maxSpeed;
    private readonly float maxForce;
    public Walk(float maxSpeed, float maxForce, List<AnimationStates> actionStates) : base(actionStates)
    {
        this.maxSpeed = maxSpeed;
        this.maxForce = maxForce;
    }

    public override void Execute(HandlerComp components)
    {
        Debug.Log($"[Caminando]");

        if (MathF.Sign(components.Physics.velocity.x) < 0)
            components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);

        float force = components.Physics.mass * maxForce * (maxSpeed - components.Physics.velocity.x) * Time.deltaTime;

        components.Physics.AddForce(new Vector2(force, 0));
    }
}
