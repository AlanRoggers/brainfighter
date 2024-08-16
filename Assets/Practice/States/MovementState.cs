using UnityEngine;

public class MovementState : BaseState
{
    public MovementState(StateMachine stateMachineComp) : base(stateMachineComp) => FixedOneExec = false;

    public override void Exit()
    {
        Debug.Log("Quiting mvement");
    }

    public override void FixedUpdate()
    {
        Debug.Log("Update of Movement");
    }

    public override void ReceiveState(StateType state)
    {
        switch (state)
        {

        }
    }

    public override void Start()
    {
        FixedOneExec = false;
        Debug.Log("Movement started");
    }
}