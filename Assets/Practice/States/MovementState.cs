using UnityEngine;

public class MovementState : AbstractState
{
    public MovementState() => FixedOneExec = false;

    public override bool CanTransitionTo(StateType state)
    {
        return state switch
        {
            StateType.Idle or StateType.Jump => true,
            _ => false,
        };
    }

    public override void Exit()
    {
        Debug.Log("Quiting mvement");
    }

    public override void FixedUpdate()
    {
        Debug.Log("Update of Movement");
    }

    public override void Start()
    {
        FixedOneExec = false;
        Debug.Log("Movement started");
    }
}