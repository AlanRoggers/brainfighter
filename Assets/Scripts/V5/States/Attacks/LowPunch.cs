using System;
using System.Collections.Generic;
using UnityEngine;

public class LowPunch : AttackV5
{
    public LowPunch()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.LowPunch,
            AnimationStates.ChainLowPunch
        };
        inertia = new Vector2(-1.5f, 0);
        timesDamageApplied = 1;
        hitFreeze = false;
        coolDown = 0.1f;
        hitFreezeTimer = 0.25f;
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKeyDown(KeyCode.I) && currentClip == clips[1])
            return character.States.MiddlePunch;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            return character.States.Iddle;

        return null;
    }

    public override void Update(CharacterV5 character)
    {
        Debug.Log("Golpe Débil");
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
