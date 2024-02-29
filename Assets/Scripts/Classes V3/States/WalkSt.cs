using System.Collections.Generic;

public class WalkSt : State
{
    public WalkSt()
    {
        StateName = AnimationStates.Walk;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Iddle)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
    }
}
