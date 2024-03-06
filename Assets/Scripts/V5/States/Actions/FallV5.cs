using System.Collections.Generic;
using UnityEngine;

public class FallV5 : PlayerState
{
    public FallV5()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.StartFalling,
            AnimationStates.Fall
        };
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (character.OverlapDetector.GroundDetection(character.GetComponent<Collider2D>(), LayerMask.GetMask("Ground")))
            return character.States.Iddle;
        return null;
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Cayendo");
    }
}