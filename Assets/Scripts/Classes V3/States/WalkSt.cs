using System.Collections.Generic;

public class WalkSt : State
{
    public WalkSt()
    {
        StateName = AnimationStates.StartWalking;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (msng.Iddle)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
        else if (msng.Walking < 0)
            animator.ChangeAnimation(states[AnimationStates.GoingBackwards]);
        else if (animator.CurrentClip != AnimationStates.Walk && animator.CurrentTime() > 1.0f)
            animator.ChangeAnimation(AnimationStates.Walk);
    }
}
