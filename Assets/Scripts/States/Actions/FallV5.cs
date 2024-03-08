using System.Collections.Generic;
using UnityEngine;

public class FallV5 : PlayerState
{
    public FallV5()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartFalling,
            AnimationState.Fall
        };
    }

    public override PlayerState InputAIHandler(Character character)
    {
        throw new System.NotImplementedException();
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.OverlapDetector.GroundDetection(character.Body, LayerMask.GetMask("Ground")))
            return character.States.Iddle;
        return null;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Cayendo");
    }
}