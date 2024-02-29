using System.Collections.Generic;
public abstract class State
{
    public AnimationStates StateName;
    public abstract void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states);
}
