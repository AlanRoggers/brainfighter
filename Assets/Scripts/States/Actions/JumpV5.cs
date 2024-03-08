using System.Collections.Generic;
using UnityEngine;

public class JumpV5 : PlayerState
{
    public JumpV5()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartJumping,
            AnimationState.Jump
        };
    }
    private readonly float jumpForce = 22.5f;
    public override PlayerState InputHandler(Character character)
    {
        if (character.Physics.velocity.y < 0)
            return character.States.Fall;
        return null;
    }
    public override void OnEntry(Character character)
    {
        base.OnEntry(character);
        float hasVelocity = character.LastVelocity;
        Vector2 force = new(Mathf.Round(hasVelocity), jumpForce);
        character.Physics.velocity = Vector2.zero;
        character.Physics.AddForce(force, ForceMode2D.Impulse);
    }
    public override void Update(Character character)
    {
        // Debug.Log("Saltando");
    }

    public override PlayerState InputAIHandler(Character character)
    {
        throw new System.NotImplementedException();
    }
}
