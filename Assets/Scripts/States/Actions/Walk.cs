using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : PlayerState
{
    private bool normalAnim;
    private readonly float maxForce = 500f;
    private readonly float maxSpeed = 10f;
    private bool jumpTransition;
    public Walk()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartWalking,
            AnimationState.Walk,
            AnimationState.StartGoingBackwards,
            AnimationState.GoingBackwards,
        };
    }
    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        if (character.EntryAttack && character.transform.localScale.x < 0)
            return character.States.Block;

        if (character.EntryAttack)
            return character.States.Hurt;

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

        if (agent.RequestedAction == State.JUMP)
        {
            jumpTransition = true;
            return character.States.Jump;
        }

        if (agent.RequestedAction == State.IDDLE)
            return character.States.Iddle;

        return null;
    }
    public override PlayerState InputHandler(Character character)
    {

        if (character.EntryAttack)
            return character.States.Hurt;

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.D))
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
    public override void OnEntry(Character character)
    {
        base.OnEntry(character);
    }
    public override void OnExit(Character character)
    {
        base.OnExit(character);
        if (!jumpTransition)
            character.Physics.velocity = new Vector2(0, character.Physics.velocity.y);
        else jumpTransition = false;
    }
    public override void Update(Character character)
    {
        if (character.transform.localScale.x > 0 && !normalAnim || character.transform.localScale.x < 0 && normalAnim)
            animationCor = character.StartCoroutine(HandleMultipleAnimations(character));
        // Debug.Log("Walk");
        if (!character.OverlapDetector.EnemyOverlapping(character.Body, character.gameObject.layer == 6 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1")) || character.transform.localScale.x < 0)
        {
            float force = character.Physics.mass * maxForce * (maxSpeed - Mathf.Abs(character.Physics.velocity.x)) * Time.deltaTime;
            character.Physics.AddForce(new Vector2(force, 0), ForceMode2D.Force);
        }
        else if (character.transform.localScale.x > 0)
            character.Physics.velocity = new Vector2(2, character.Physics.velocity.y);
    }

    public override IEnumerator HandleMultipleAnimations(Character character)
    {
        if (character.transform.localScale.x > 0)
        {
            normalAnim = true;
            for (int i = 0; i < 2; i++)
            {
                character.Animator.Play(clips[i].ToString());
                yield return new WaitWhile(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
            }
        }
        else
        {
            normalAnim = false;
            for (int i = 2; i < clips.Count; i++)
            {
                character.Animator.Play(clips[i].ToString());
                yield return new WaitWhile(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
            }
        }
    }
}
