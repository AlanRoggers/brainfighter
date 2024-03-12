using System.Collections.Generic;
using UnityEngine;

public class MiddlePunch : Attack
{
    public MiddlePunch()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.MiddlePunch,
            AnimationState.ChainMiddlePunch
        };
        inertia = new Vector2(0.5f, 0);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.2f;
        HitFreezeTimer = 0.25f;
        HitStun = 19;
        Damage = 5;
        Force = new Vector2(0, 11);
    }

    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (character.RequestedBehaviourAction == State.HARD_PUNCH && currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return character.States.HardPunch;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (Input.GetKeyDown(KeyCode.O) && currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return character.States.HardPunch;

        return null;
    }

    public override void Update(Character character)
    {
        // Debug.Log("Golpe ligero");
    }
}
