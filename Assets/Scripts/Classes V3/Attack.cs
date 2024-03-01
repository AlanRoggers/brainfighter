using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public bool HitFreeze { get; private set; }
    public int Damage { get; private set; }
    public float HitStun { get; private set; }
    public float CoolDown { get; private set; }
    public Vector2 Inertia { get; private set; }
    public Vector2 Force { get; private set; }
    public AnimationStates initState;
    public AnimationStates endState;
    private bool Initialized;
    public int timesDamageApplied;
    public void InitValues(Vector2 inertia, Vector2 force, bool hitFreeze, int damage, int timesDamageApplied, float hitStun, float coolDown, AnimationStates initState, AnimationStates endState)
    {
        if (!Initialized)
        {
            Initialized = true;
            Inertia = inertia;
            Force = force;
            HitFreeze = hitFreeze;
            Damage = damage;
            HitStun = hitStun;
            CoolDown = coolDown;
            this.timesDamageApplied = timesDamageApplied;
            this.initState = initState;
            this.endState = endState;
        }
        else Debug.Log("[Attaque] Los valores ya se inicializaron, imposible cambiarlos");
    }
}
