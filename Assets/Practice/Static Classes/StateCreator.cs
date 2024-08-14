public static class StateCreator
{
    /// <summary>
    /// Instantiates a new state
    /// </summary>
    /// <param name="stateType">Type of state that needs to instantiate</param>
    /// <returns>Required state</returns>
    public static AbstractState CreateState(StateType stateType)
    {
        return stateType switch
        {
            StateType.Idle => new IdleState(),
            StateType.Movement => new MovementState(),
            StateType.Jump => new JumpState(),
            _ => null,
        };
    }
}
