using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public bool HitFreeze;
    public int Damage;
    public float HitStun;
    public float CoolDown;
    public Vector2 Force;
    public Vector2 Inertia;
    public Attack(int damage, Vector2 force, Vector2 inertia, float hitStun, float coolDown, bool hitFreeze)
    {
        HitFreeze = hitFreeze;
        Damage = damage;
        HitStun = hitStun;
        CoolDown = coolDown;
        Force = force;
        Inertia = inertia;
    }
}
