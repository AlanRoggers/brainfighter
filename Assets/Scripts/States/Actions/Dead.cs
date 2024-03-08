public class Dead : PlayerState
{
    public delegate void AgentWin(bool whichAgent);
    public event AgentWin OnWin;
    public override void OnEntry(Character character)
    {
        OnWin(character.gameObject.layer != 6);
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
