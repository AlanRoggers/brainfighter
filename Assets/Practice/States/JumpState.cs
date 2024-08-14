using UnityEngine;

public class JumpState : AbstractState
{
    public JumpState() => FixedOneExec = true;

    public override bool CanTransitionTo(StateType state)
    {
        return state == StateType.Idle;
    }

    public override void Exit()
    {
        Debug.Log("Quiting jump");
    }

    public override void Start()
    {
        Debug.Log("Starting jump");
    }
}