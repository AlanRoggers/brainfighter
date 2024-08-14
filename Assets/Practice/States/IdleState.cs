using UnityEngine;

public class IdleState : AbstractState
{
    public override bool CanTransitionTo(StateType state)
    {
        return true;
    }

    public override void Exit()
    {
        Debug.Log("Quiting idle");
    }

    public override void Start()
    {
        Debug.Log("Idle start");
    }
}