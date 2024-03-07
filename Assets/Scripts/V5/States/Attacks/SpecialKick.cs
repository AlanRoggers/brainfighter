using System.Collections.Generic;
using UnityEngine;

public class SpecialKick : AttackV5
{
    public SpecialKick()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.SpecialKick,
            AnimationStates.ChainSpecialKick
        };
        inertia = new Vector2(1, 5);
        force = new Vector2(0, 7);
        timesDamageApplied = 3;
        hitFreeze = true;
        coolDown = 0.8f;
        hitFreezeTimer = 0.25f;
        damage = 11;
        hitStun = 35;
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }

    public override void Update(CharacterV5 character)
    {
        // Debug.Log("SpecialKick");
    }




}
