using System.Collections.Generic;
using UnityEngine;

public class HardPunch : AttackV5
{
    public HardPunch()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.HardPunch,
            AnimationStates.ChainHardPunch
        };
        inertia = new Vector2(1, 4);
        timesDamageApplied = 1;
        hitFreeze = true;
        coolDown = 0.4f;
        hitFreezeTimer = 0.25f;
        hitStun = 71;
        damage = 7;
        force = new Vector2(13, 0);
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (currentClip == clips[1] && character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.1f)
            return character.States.Iddle;

        if (currentClip == clips[1] && Input.GetKeyDown(KeyCode.P) && character.HitsChained >= 3)
            return character.States.SpecialPunch;

        return null;
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Golpe fuerte");
    }


}
