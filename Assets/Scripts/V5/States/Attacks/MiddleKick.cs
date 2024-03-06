using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleKick : AttackV5
{
    public MiddleKick()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.MiddleKick,
            AnimationStates.ChainMiddleKick
        };
        inertia = new Vector2(0, 8);
        timesDamageApplied = 1;
        hitFreeze = true;
        coolDown = 0.25f;
        hitFreezeTimer = 0.25f;
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
        Debug.Log("MiddleKick");
    }
    protected override void Freeze(CharacterV5 character)
    {
        Debug.Log("Freeze");
    }
    protected override void UnFreeze(CharacterV5 character, Vector2 current)
    {
        Debug.Log("Unfreeze"); throw new System.NotImplementedException();
    }
}
