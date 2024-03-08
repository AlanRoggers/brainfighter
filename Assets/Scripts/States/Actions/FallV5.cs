using System.Collections.Generic;
using UnityEngine;

public class Fall : PlayerState
{
    public Fall()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartFalling,
            AnimationState.Fall
        };
    }

    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (character.OverlapDetector.GroundDetection(character.Body, LayerMask.GetMask("Ground")))
            return character.States.Iddle;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (character.OverlapDetector.GroundDetection(character.Body, LayerMask.GetMask("Ground")))
            return character.States.Iddle;

        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Cayendo");
    }
}