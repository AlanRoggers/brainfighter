using System.Collections.Generic;
using UnityEngine;

public class MiddleKick : Attack
{
    public MiddleKick()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.MiddleKick,
            AnimationState.ChainMiddleKick
        };
        inertia = new Vector2(0, 8);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.25f;
        HitFreezeTimer = 0.25f;
        HitStun = 22;
        Damage = 6;
        Force = new Vector2(0, 12);
    }
    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        PlayerState shared = SharedActions(character);

        if (shared != null) return shared;

        if (agent.RequestedAction == State.HARD_KICK && currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return character.States.HardKick;

        return null;

    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (Input.GetKeyDown(KeyCode.L) && currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return character.States.HardKick;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("MiddleKick");
    }
    private PlayerState SharedActions(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }
}
