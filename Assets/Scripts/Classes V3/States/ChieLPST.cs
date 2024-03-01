using System.Collections.Generic;

public class ChieLPST : State
{
    public ChieLPST()
    {
        StateName = AnimationStates.LowPunch;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        if (animator.CurrentClip == AnimationStates.ChainLowPunch && animator.CurrentTime() > 1.0f)
            animator.ChangeAnimation(states[AnimationStates.Iddle]);
    }
}
