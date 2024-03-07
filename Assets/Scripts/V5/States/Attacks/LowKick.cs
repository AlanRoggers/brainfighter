using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowKick : AttackV5
{
    public LowKick()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.LowKick,
            AnimationState.ChainLowKick
        };
        inertia = new Vector2(-0.5f, 7.5f);
        timesDamageApplied = 1;
        HitFreeze = true;
        coolDown = 0.1f;
        HitFreezeTimer = 0.25f;
        HitStun = 7;
        Damage = 4;
        Force = new Vector2(4.5f, 0);
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            if (Input.GetKeyDown(KeyCode.K))
                return character.States.MiddleKick;

            if (Input.GetKeyDown(KeyCode.L))
                return character.States.HardKick;

            if (Input.GetKeyDown(KeyCode.LeftArrow) && character.gameObject.layer == 7)
                return character.States.MiddleKick;
        }

        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        return null;
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Ataque debil");
    }
}
