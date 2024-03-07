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
        hitStun = 25;
        damage = 10;
        force = new Vector2(6, 15);
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;
        return null;
    }

    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Golpe especial");
    }




}
