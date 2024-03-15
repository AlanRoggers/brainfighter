using System.Collections.Generic;
using UnityEngine;

public class LowPunch : Attack
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
        HitStun = 0.6f;
        Damage = 3;
        Force = new Vector2(1f, 4.5f);
    }
    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        PlayerState shared = SharedActions(character);

        if (shared != null) return shared;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            if (agent.RequestedAction == State.MIDDLE_PUNCH)
                return character.States.MiddlePunch;

            if (agent.RequestedAction == State.HARD_PUNCH)
                return character.States.HardPunch;
        }

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        PlayerState shared = SharedActions(character);

        if (shared != null) return shared;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            if (Input.GetKeyDown(KeyCode.I))
                return character.States.MiddlePunch;

            if (Input.GetKeyDown(KeyCode.O))
                return character.States.HardPunch;
        }

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Golpe Débil");
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
