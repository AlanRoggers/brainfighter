using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Exit()
    {
        Debug.Log("Quiting idle");
    }

    public override void ReceiveState(StateType state)
    {
        _stateMachine.ChangeState(state);
    }

    public override void Start()
    {
        Debug.Log("Idle start");
    }
}