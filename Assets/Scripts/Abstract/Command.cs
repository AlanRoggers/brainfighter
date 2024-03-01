using System.Collections.Generic;

public abstract class Command
{
    // public AnimationStates state;
    public List<AnimationStates> ActionStates { get; private set; }
    public Command(List<AnimationStates> actionStates)
    {
        ActionStates = actionStates;
    }
    public virtual void Transitions(StateMachine animator, Messenger msng)
    {
        int nextClip = ActionStates.IndexOf(animator.CurrentClip) + 1;

        if (animator.CurrentTime() > 1.0f && nextClip <= ActionStates.Count - 1)
        {
            animator.ChangeAnimation(ActionStates[nextClip]);
        }
    }
}
