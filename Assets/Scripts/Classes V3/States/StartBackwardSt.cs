using System.Collections.Generic;
using UnityEngine;

public class StartBackwardSt : State
{
    public StartBackwardSt()
    {
        StateName = AnimationStates.StartGoingBackwards;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Iddle)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
        else if (msng.Walking > 0)
            animator.ChangeAnimation(states[AnimationStates.Walk]);
        else if (animator.CurrentTime() > 1.0f)
            animator.ChangeAnimation(states[AnimationStates.GoingBackwards]);
    }
}
