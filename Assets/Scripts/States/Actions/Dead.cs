public class Dead : PlayerState
{
    public override void OnEntry(Character character)
    {
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
