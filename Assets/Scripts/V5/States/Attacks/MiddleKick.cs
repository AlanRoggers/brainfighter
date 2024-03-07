using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleKick : AttackV5
{
    public MiddleKick()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.MiddleKick,
            AnimationState.ChainMiddleKick
        };
        inertia = new Vector2(0, 8);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.25f;
        HitFreezeTimer = 0.25f;
        HitStun = 22;
        Damage = 6;
        Force = new Vector2(0, 12);
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKeyDown(KeyCode.L) && currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return character.States.HardKick;

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("MiddleKick");
    }
}
