public class Dead : PlayerState
{
    public override void OnEntry(Character character)
    {
        character.Animator.Play(AnimationState.Dead.ToString());
    }
    public override PlayerState InputAIHandler(Character character)
    {
        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        return null;
    }

    public override void Update(Character character)
    {
        return;
    }
}
