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
        inertia = new Vector2(0, 4); //1,4
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.4f;
        HitFreezeTimer = 0.25f;
        HitStun = 1.15f;
        Damage = 7;
        Force = new Vector2(13, 0);
    }

    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        PlayerState shared = SharedActions(character);

        if (shared != null) return shared;

        if (currentClip == clips[1] && agent.RequestedAction == State.SPECIAL_PUNCH && character.HitsChained >= 3)
            return character.States.SpecialPunch;

        return null;
    }
    public override PlayerState InputHandler(Character character)
    {
        PlayerState shared = SharedActions(character);

        if (shared != null) return shared;

        if (currentClip == clips[1] && Input.GetKeyDown(KeyCode.P) && character.HitsChained >= 3)
            return character.States.SpecialPunch;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Golpe fuerte");
    }

    private PlayerState SharedActions(Character character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (character.EntryAttack)
            return character.States.Hurt;

        return null;
    }
}
