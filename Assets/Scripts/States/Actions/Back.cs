using System;
using System.Collections.Generic;
using UnityEngine;

public class Back : PlayerState
{
    private readonly float maxForce = 500f;
    private readonly float maxSpeed = 10f;
    private bool jumpTransition;
    public Back()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartGoingBackwards,
            AnimationState.GoingBackwards,
        };
    }

    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        if (character.EntryAttack)
            return character.States.Block;

        if (agent.RequestedAction == State.IDDLE)
            return character.States.Iddle;

        if (agent.RequestedAction == State.JUMP)
        {
            jumpTransition = true;
            return character.States.Jump;
        }

        if (!character.OnColdoown)
        {
            switch (agent.RequestedAction)
            {
                case State.LOW_PUNCH:
                    return character.States.LowPunch;
                case State.MIDDLE_PUNCH:
                    return character.States.MiddlePunch;
                case State.HARD_PUNCH:
                    return character.States.HardPunch;
                case State.LOW_KICK:
                    return character.States.LowKick;
                case State.MIDDLE_KICK:
                    return character.States.MiddleKick;
                case State.HARD_KICK:
                    return character.States.HardKick;
            }
        }

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Block;

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            return character.States.Iddle;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTransition = true;
            return character.States.Jump;
        }

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
    public override void OnExit(Character character)
    {
        base.OnExit(character);
        if (!jumpTransition)
            character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
        else
            jumpTransition = false;
    }
    public override void Update(Character character)
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
}