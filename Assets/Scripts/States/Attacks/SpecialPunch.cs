using System.Collections.Generic;
using UnityEngine;

public class SpecialPunch : Attack
{
    public SpecialPunch()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.SpecialPunch,
            AnimationState.ChainSpecialPunch
        };
        inertia = new Vector2(6, 13.3f);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.6f;
        HitFreezeTimer = 0.25f;
        HitStun = 25;
        Damage = 10;
        Force = new Vector2(6, 15);
    }

    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }

    public override void Update(Character character)
    {
        // Debug.Log("Golpe especial" + currentClip);
    }




}
