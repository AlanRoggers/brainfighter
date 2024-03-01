using System.Collections.Generic;

public class ChieMPST : State
{
    public ChieMPST()
    {
        StateName = AnimationStates.MiddlePunch;
    }

    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        // if (animator.CurrentClip == AnimationStates.ChainMiddlePunch && animator.CurrentTime() > 1.0f)
        //     animator.ChangeAnimation(states[AnimationStates.Iddle]);
    }
}
