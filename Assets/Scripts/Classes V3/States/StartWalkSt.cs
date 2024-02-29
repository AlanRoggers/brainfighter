using System.Collections.Generic;
using UnityEngine;

public class StartWalkSt : State
{
    public StartWalkSt()
    {
        StateName = AnimationStates.StartWalking;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Iddle)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
        else if (animator.CurrentTime() > 1.0f)
            animator.ChangeAnimation(states[AnimationStates.Walk]);
    }
}
