using System.Collections.Generic;
using UnityEngine;

public class Crouch : Action
{
    public Crouch(List<AnimationStates> actionStates) : base(actionStates) { }

    public override void Execute(HandlerComp components)
    {
        components.CharacterColl.size = new Vector2(1.098f, 2.642f);
        components.CharacterColl.offset = new Vector2(0.071f, 1.762f);
    }
    public override void Transitions(StateMachine animator, Messenger msng)
    {
        Debug.Log("Transiciones de Crouch");
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
