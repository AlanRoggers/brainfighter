using UnityEngine;
public class ChieLP : Attack
{
    public ChieLP()
    {
        Inertia = Vector2.zero;
        Force = new Vector2(1.5f, 0);
        HitFreeze = false;
        Damage = 3;
        TimesDamageApplied = 1;
        HitStun = 0.2f;
        CoolDown = 0.1f;
        initState = AnimationStates.LowPunch;
        endState = AnimationStates.ChainLowPunch;
    }
}
