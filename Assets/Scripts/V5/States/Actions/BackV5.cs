using System;
using System.Collections.Generic;
using UnityEngine;

public class BackV5 : PlayerState
{
    private readonly float maxForce = 500f;
    private readonly float maxSpeed = 10f;
    public BackV5()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.StartGoingBackwards,
            AnimationStates.GoingBackwards,
        };
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            return character.States.Iddle;

        if (Input.GetKeyDown(KeyCode.Space))
            return character.States.Jump;

        return null;
    }
    public override void OnExit(CharacterV5 character)
    {
        base.OnExit(character);
        character.LastVelocity = character.Physics.velocity.x;
        character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Back");
        if (Mathf.Sign(character.transform.localScale.x) == 1)
        {
            if (MathF.Sign(character.Physics.velocity.x) > 0)
                character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
        }
        else
        {
            if (MathF.Sign(character.Physics.velocity.x) < 0)
                character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
        }
        float force = character.Physics.mass * maxForce * (maxSpeed - Mathf.Abs(character.Physics.velocity.x)) * Time.deltaTime * Mathf.Sign(character.transform.localScale.x) * -1;
        character.Physics.AddForce(new Vector2(force, 0), ForceMode2D.Force);
    }
    public override void OnEntry(CharacterV5 character)
    {
        base.OnEntry(character);
        character.LastVelocity = 0;
    }
}