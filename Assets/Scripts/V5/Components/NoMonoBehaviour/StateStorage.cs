public class StateStorage
{
    public readonly WalkV5 Walk = new();
    public readonly Iddle Iddle = new();
    public readonly BackV5 Back = new();
    public readonly JumpV5 Jump = new();
    public readonly FallV5 Fall = new();
    public readonly LowPunch LowPunch = new();
    public readonly MiddlePunch MiddlePunch = new();
}
