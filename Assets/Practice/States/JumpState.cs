using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(StateMachine stateMachineComp) : base(stateMachineComp) => FixedOneExec = true;

    public override void Exit()
    {
        Debug.Log("Quiting jump");
    }

    public override void ReceiveState(StateType state)
    {
        switch (state)
        {
            case StateType.Jump:
            case StateType.Idle:
                _stateMachine.ChangeState(state);
                break;
            default:
                Debug.Log($"[Jump] No considered state {state}");
                break;
        }
    }

    public override void Start()
    {
        Debug.Log("Starting jump");
    }
}