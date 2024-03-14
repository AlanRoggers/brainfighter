using System.Collections.Generic;
using UnityEngine;

public class Jump : PlayerState
{
    public Jump()
    {
        clips = new List<AnimationState>()
        {
            AnimationState.StartJumping,
            AnimationState.Jump
        };
    }
    private readonly float jumpForce = 22.5f;
    public override PlayerState InputAIHandler(Character character, PPOAgent agent) => SharedActions(character);
    public override PlayerState InputHandler(Character character) => SharedActions(character);
    public override void OnEntry(Character character)
    {
        base.OnEntry(character);
        Vector2 force = new(Mathf.Round(character.Physics.velocity.x), jumpForce);
        character.Physics.velocity = Vector2.zero;
        character.Physics.AddForce(force, ForceMode2D.Impulse);
    }
    public override void Update(Character character)
    {
        // Debug.Log("Saltando");
    }

    private PlayerState SharedActions(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (character.Physics.velocity.y < 0)
            return character.States.Fall;

        return null;
    }
}
