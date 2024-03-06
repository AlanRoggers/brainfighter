using System.Collections;
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
        inertia = new Vector2(-0.5f, 5);
        timesDamageApplied = 3;
        hitFreeze = true;
        coolDown = 0.8f;
        hitFreezeTimer = 0.25f;
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }

    public override void Update(CharacterV5 character)
    {
        Debug.Log("SpecialKick");
    }

    protected override void Freeze(CharacterV5 character)
    {
        Debug.Log("Freeze");
    }

    protected override void UnFreeze(CharacterV5 character, Vector2 current)
    {
        Debug.Log("Unfreeze");
    }
}
