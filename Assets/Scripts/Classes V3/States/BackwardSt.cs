using System.Collections.Generic;

public class BackwardSt : State
{
    public BackwardSt()
    {
        StateName = AnimationStates.GoingBackwards;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Iddle)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
        else if (msng.Walking > 0)
            animator.ChangeAnimation(states[AnimationStates.Walk]);
    }
}
