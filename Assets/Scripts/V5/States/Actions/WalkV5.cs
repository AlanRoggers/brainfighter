using System;
using System.Collections.Generic;
using UnityEngine;

public class WalkV5 : PlayerState
{
    private readonly float maxForce = 500f;
    private readonly float maxSpeed = 10f;
    public WalkV5()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartWalking,
            AnimationState.Walk,
        };
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            return character.States.Iddle;

        if (Input.GetKeyDown(KeyCode.Space))
            return character.States.Jump;

        if (!character.OnColdoown)
        {
            if (Input.GetKeyDown(KeyCode.U))
                return character.States.LowPunch;

            if (Input.GetKeyDown(KeyCode.I))
                return character.States.MiddlePunch;

            if (Input.GetKeyDown(KeyCode.O))
                return character.States.HardPunch;

            if (Input.GetKeyDown(KeyCode.J))
                return character.States.LowKick;

            if (Input.GetKeyDown(KeyCode.K))
                return character.States.MiddleKick;

            if (Input.GetKeyDown(KeyCode.L))
                return character.States.HardKick;
        }

        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        base.OnEntry(character);
        character.LastVelocity = 0;
    }
    public override void OnExit(CharacterV5 character)
    {
        base.OnExit(character);
        character.LastVelocity = character.Physics.velocity.x;
        character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Walk");
        if (!character.OverlapDetector.EnemyOverlapping(character.transform, character.Layer == 64 ? 128 : 64))
        {
            if (Mathf.Sign(character.transform.localScale.x) == 1)
            {
                if (MathF.Sign(character.Physics.velocity.x) < 0)
                    character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
            }
            else
            {
                if (MathF.Sign(character.Physics.velocity.x) > 0)
                    character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
            }
            float force = character.Physics.mass * maxForce * (maxSpeed - Mathf.Abs(character.Physics.velocity.x)) * Time.deltaTime * Mathf.Sign(character.transform.localScale.x);
            character.Physics.AddForce(new Vector2(force, 0), ForceMode2D.Force);
        }
        else
            character.Physics.velocity = new Vector2(character.transform.localScale.x > 0 ? 2 : -2, character.Physics.velocity.y);
    }

}
