using System.Collections.Generic;
using UnityEngine;

public class HardKick : Attack
{
    public HardKick()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.HardKick,
            AnimationState.ChainHardKick
        };
        inertia = new Vector2(0.4f, 10);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.45f;
        HitFreezeTimer = 0.25f;
        HitStun = 10;
        Damage = 8;
        Force = new Vector2(2, 8);
    }

    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (currentClip == clips[1] && character.RequestedBehaviourAction == State.SPECIAL_KICK && character.HitsChained >= 3)
            return character.States.SpecialKick;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (currentClip == clips[1] && Input.GetKeyDown(KeyCode.Semicolon) && character.HitsChained >= 3)
            return character.States.SpecialKick;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("HardKick");
    }
}