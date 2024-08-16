public static class StateCreator
{
    /// <summary>
    /// Instantiates a new state
    /// </summary>
    /// <param name="stateType">Type of state that needs to instantiate</param>
    /// <returns>Required state</returns>
    public static BaseState CreateState(StateType stateType, StateMachine stateMachine)
    {
        return stateType switch
        {
            StateType.Idle => new IdleState(stateMachine),
            StateType.Movement => new MovementState(stateMachine),
            StateType.Jump => new JumpState(stateMachine),
            _ => null,
        };
    }
}
