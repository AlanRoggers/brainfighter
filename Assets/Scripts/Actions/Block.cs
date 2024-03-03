using System.Collections.Generic;
using UnityEngine;


public class Block : Action
{
    public Block(List<AnimationStates> actionStates) : base(actionStates)
    {
    }

    public override void Execute(HandlerComp components)
    {
        Debug.Log("[Esta acción no se usa asi]");
    }
    public override void Transitions(StateMachine animator, Messenger msng)
    {
        if (ActionStates.Count == 2)
        {
            if (msng.Crouching && animator.CurrentClip != AnimationStates.BlockWhileCrouch)
                animator.ChangeAnimation(ActionStates[1]);
            else if (animator.CurrentClip != AnimationStates.Block && !msng.Crouching)
                animator.ChangeAnimation(ActionStates[0]);
        }
    }
}
