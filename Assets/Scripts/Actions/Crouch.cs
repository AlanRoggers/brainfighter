using System.Collections.Generic;
using UnityEngine;

public class Crouch : Action
{
    private readonly Vector2 sizeColl = new(1.098f, 2.642f);
    private readonly Vector2 offsetColl = new(0.071f, 1.762f);
    public Crouch(List<AnimationStates> actionStates) : base(actionStates) { }
    public override void Execute(HandlerComp components)
    {
        components.CharacterColl.size = sizeColl;
        components.CharacterColl.offset = offsetColl;
    }
    public override void Transitions(StateMachine animator, Messenger msng)
    {
        int auxiliarIndex = ActionStates.IndexOf(animator.CurrentClip);
        if (auxiliarIndex != -1)
        {
            if (animator.CurrentTime() > 1.0f && auxiliarIndex + 1 <= ActionStates.Count - 1)
            {
                animator.ChangeAnimation(ActionStates[auxiliarIndex + 1]);
            }
        }
        else
        {
            animator.ChangeAnimation(ActionStates[^1]);
        }
    }
}
