using System.Collections.Generic;
using UnityEngine;

public class SpecialKick : Attack
{
    public SpecialKick()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.SpecialKick,
            AnimationState.ChainSpecialKick
        };
        inertia = new Vector2(1, 5);
        Force = new Vector2(0, 7);
        timesDamageApplied = 3;
        HitFreeze = true;
        coolDown = 0.8f;
        HitFreezeTimer = 0.25f;
        Damage = 11;
        HitStun = 0.8f;
    }

    public override PlayerState InputAIHandler(Character character, PPOAgent agent) => SharedActions(character);

    public override PlayerState InputHandler(Character character) => SharedActions(character);

    public override void Update(Character character)
    {
        // Debug.Log("SpecialKick");
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
