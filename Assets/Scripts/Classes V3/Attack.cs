using UnityEngine;

public abstract class Attack
{
    public bool HitFreeze { get; protected set; }
    public int Damage { get; protected set; }
    public float HitStun { get; protected set; }
    public float CoolDown { get; protected set; }
    public Vector2 Inertia { get; protected set; }
    public Vector2 Force { get; protected set; }
    public AnimationStates initState;
    public AnimationStates endState;
    protected bool Initialized;
    public int TimesDamageApplied;
}
