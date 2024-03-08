using System.Collections.Generic;
using UnityEngine;

public class HardPunch : Attack
{
    public HardPunch()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.HardPunch,
            AnimationState.ChainHardPunch
        };
        inertia = new Vector2(1, 4);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.4f;
        HitFreezeTimer = 0.25f;
        HitStun = 15;
        Damage = 7;
        Force = new Vector2(13, 0);
    }

    public override PlayerState InputAIHandler(Character character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.RequestedBehaviourAction == State.SPECIAL_PUNCH && character.HitsChained >= 3)
            return character.States.SpecialPunch;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (currentClip == clips[1] && Input.GetKeyDown(KeyCode.P) && character.HitsChained >= 3)
            return character.States.SpecialPunch;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Golpe fuerte");
    }


}
