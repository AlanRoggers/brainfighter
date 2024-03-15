public class Dead : PlayerState
{
    public delegate void AgentDead(bool whichAgent);
    public event AgentDead OnDead;
    public override void OnEntry(Character character)
    {
        OnDead?.Invoke(character.gameObject.layer == 6);
        character.Animator.Play(AnimationState.Dead.ToString());
    }

    public override PlayerState InputHandler(Character character)
    {
        return null;
    }

    public override void Update(Character character)
    {
        return;
    }

    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        return null;
    }
}
