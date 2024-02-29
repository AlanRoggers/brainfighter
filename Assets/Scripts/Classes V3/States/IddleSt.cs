using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleSt : State
{
    public IddleSt()
    {
        StateName = AnimationStates.Iddle;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Walking > 0)
            animator.ChangeAnimation(states[AnimationStates.StartWalking]);
        else if (msng.Walking < 0)
            animator.ChangeAnimation(states[AnimationStates.StartGoingBackwards]);
    }
}
