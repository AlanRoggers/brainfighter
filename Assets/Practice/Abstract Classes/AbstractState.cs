public abstract class AbstractState
{
    public bool FixedOneExec = false;
    public abstract void Start();
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public abstract void Exit();
    public abstract bool CanTransitionTo(StateType state);
}