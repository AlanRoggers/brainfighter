public enum AnimationStates
{
    #region Motion
    Iddle,
    StartJumping,
    Jump,
    StartFalling,
    Fall,
    StartWalking,
    Walk,
    StartGoingBackwards,
    GoingBackwards,
    Turn,
    TurnWhileCrouch,
    TurnOnAir,
    StartCrouching,
    Crouch,
    StartRunning,
    Run,
    Dash,
    DashBack,
    #endregion
    #region Attacks
    LowPunch, ChainLowPunch,
    MiddlePunch, ChainMiddlePunch,
    HardPunch, ChainHardPunch,
    SpecialPunch, ChainSpecialPunch,
    SomerSaultKick,
    LowKick,
    MiddleKick,
    HardKick,
    SpecialKick,
    KickWhileCrouch,
    AirKick,
    DashAttack,
    #endregion
    #region Emotes
    Damage,
    DamageWhileCrouch,
    InFloor,
    Recover,
    Dead,
    Land,
    #endregion
}