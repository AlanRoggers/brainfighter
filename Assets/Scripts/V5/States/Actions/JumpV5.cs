using System.Collections.Generic;
using UnityEngine;

public class JumpV5 : PlayerState
{
    public JumpV5()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.StartJumping,
            AnimationStates.Jump
        };
    }
    private readonly float jumpForce = 22.5f;
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (character.Physics.velocity.y < 0)
            return character.States.Fall;
        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        base.OnEntry(character);
        float hasVelocity = character.States.Walk.lastVelocity != 0 ? character.States.Walk.lastVelocity : character.States.Back.lastVelocity != 0 ? character.States.Back.lastVelocity : 0;
        Vector2 force = new(Mathf.Round(hasVelocity), jumpForce);
        character.Physics.velocity = Vector2.zero;
        character.Physics.AddForce(force, ForceMode2D.Impulse);
    }
    public override void Update(CharacterV5 character)
    {
        Debug.Log("Saltando");
    }

}
