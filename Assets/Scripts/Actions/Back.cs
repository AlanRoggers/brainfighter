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
    /// <summary>
    /// Esta acción hace que el personaje huya de su enemigo, no esta sujeta a una velocidad positiva o negativa
    /// la dirección dependerá de donde esta el enemigo, por lo tanto la velocidad puede ser o positiva o negativa.
    /// Esta acción frena la velocidad del personaje en caso de que sea la dirección incorrecta
    /// </summary>
    /// <param name="components"></param>
    public override void Execute(HandlerComp components)
    {
        Debug.Log($"[Retrocediendo]");

        if (Mathf.Sign(components.Transform.localScale.x) == 1)
        {
            if (MathF.Sign(components.Physics.velocity.x) > 0)
                components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);
        }
        else
        {
            if (MathF.Sign(components.Physics.velocity.x) < 0)
                components.Physics.velocity = new Vector2(0, components.Physics.velocity.y);
        }

        float force = components.Physics.mass * maxForce * (maxSpeed - MathF.Abs(components.Physics.velocity.x)) * Time.deltaTime * Mathf.Sign(components.Transform.localScale.x) * -1;

        components.Physics.AddForce(new Vector2(force, 0));
    }
}
