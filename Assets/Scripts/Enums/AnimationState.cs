public enum AnimationState
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
    Block,
    BlockWhileCrouch,
    #endregion
    #region Attacks
    LowPunch, ChainLowPunch,
    MiddlePunch, ChainMiddlePunch,
    HardPunch, ChainHardPunch,
    SpecialPunch, ChainSpecialPunch,
    SomerSaultKick,
    LowKick, ChainLowKick,
    MiddleKick, ChainMiddleKick,
    HardKick, ChainHardKick,
    SpecialKick, ChainSpecialKick,
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
    Incapacite,
    #endregion
    Null,
}