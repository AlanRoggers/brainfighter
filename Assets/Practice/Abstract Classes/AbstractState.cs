public abstract class BaseState
{
    protected StateMachine _stateMachine;
    public BaseState(StateMachine stateMachineComp)
    {
        _stateMachine = stateMachineComp;
        _stateMachine.ReceivedState += ReceiveState;
    }
    public bool FixedOneExec = false;
    public abstract void Start();
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public abstract void Exit();
    public abstract void ReceiveState(StateType state);
}