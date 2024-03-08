public class StateStorage
{
    public readonly WalkV5 Walk = new();
    public readonly Iddle Iddle = new();
    public readonly Back Back = new();
    public readonly JumpV5 Jump = new();
    public readonly FallV5 Fall = new();
    public readonly LowPunch LowPunch = new();
    public readonly MiddlePunch MiddlePunch = new();
    public readonly HardPunch HardPunch = new();
    public readonly SpecialPunch SpecialPunch = new();
    public readonly LowKick LowKick = new();
    public readonly MiddleKick MiddleKick = new();
    public readonly HardKick HardKick = new();
    public readonly SpecialKick SpecialKick = new();
    public readonly Hurt Hurt = new();
    public readonly Block Block = new();
    public readonly Stun Stun = new();
}
