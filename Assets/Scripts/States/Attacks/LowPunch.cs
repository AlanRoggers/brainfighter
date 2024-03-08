using System;
using System.Collections.Generic;
using UnityEngine;

public class LowPunch : AttackV5
{
    public LowPunch()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.LowPunch,
            AnimationState.ChainLowPunch
        };
        inertia = new Vector2(-1.5f, 0);
        timesDamageApplied = 1;
        HitFreeze = false;
        coolDown = 0.1f;
        HitFreezeTimer = 0.25f;
        HitStun = 5;
        Damage = 3;
        Force = new Vector2(1f, 4.5f);
    }

    public override PlayerState InputAIHandler(Character character)
    {
        throw new NotImplementedException();
    }

    public override PlayerState InputHandler(Character character)
    {
        if (!character.IsAI)
        {
            if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                if (Input.GetKeyDown(KeyCode.I))
                    return character.States.MiddlePunch;

                if (Input.GetKeyDown(KeyCode.O))
                    return character.States.HardPunch;
            }
        }
        else
        {
            if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                if (character.RequestedBehaviourAction == State.MIDDLE_PUNCH)
                    return character.States.MiddlePunch;

                if (character.RequestedBehaviourAction == State.HARD_PUNCH)
                    return character.States.HardPunch;
            }
        }

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Golpe Débil");
    }



}
