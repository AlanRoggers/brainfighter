using System.Collections.Generic;
using UnityEngine;

public class SpecialPunch : AttackV5
{
    public SpecialPunch()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.SpecialPunch,
            AnimationStates.ChainSpecialPunch
        };
        inertia = new Vector2(6, 13.3f);
        timesDamageApplied = 1;
        hitFreeze = true;
        coolDown = 0.6f;
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
        Debug.Log("Golpe especial");
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
