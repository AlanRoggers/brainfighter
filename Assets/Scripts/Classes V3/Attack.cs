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
    private bool Initialized;
    private int timesDamageApplied;
    public delegate void DoDamage(int damage, Collider2D enemy);
    public void InitValues(Vector2 inertia, Vector2 force, bool hitFreeze, int damage, int timesDamageApplied, float hitStun, float coolDown)
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
        }
        else Debug.Log("[Attaque] Los valores ya se inicializaron, imposible cambiarlos");
    }
    public IEnumerator ExecuteAttack(StateMachine machine, LayerMask contactLayer, CircleCollider2D hitbox, DoDamage doDamage, Messenger msng,
        AnimationStates initState, AnimationStates endState, AnimationStates transition, Dictionary<AnimationStates, State> states)
    {
        try
        {
            msng.Attacking = true;
            machine.ChangeAnimation(states[initState]);
            yield return new WaitForEndOfFrame();
            while (machine.CurrentTime() < 1.0f)
            {
                if (timesDamageApplied > 0)
                {
                    Collider2D enemy = Hitted(contactLayer, hitbox);
                    if (enemy)
                    {
                        doDamage(Damage, enemy);
                        timesDamageApplied--;
                    }
                }
            }
            machine.ChangeAnimation(states[endState]);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => machine.CurrentTime() > 1.0f);
            msng.Attacking = false;
            msng.InCooldown = true;
            machine.ChangeAnimation(states[transition]);
            yield return new WaitForSecondsRealtime(CoolDown);
            msng.InCooldown = false;
        }
        finally
        {
            if (msng.Hurt)
                msng.Attacking = false;

            msng.InCooldown = false;
        }
    }
    private Collider2D Hitted(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);
    }
}
