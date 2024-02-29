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
    private AnimationStates initState;
    private AnimationStates endState;
    private bool Initialized;
    private int timesDamageApplied;
    public delegate void DoDamage(int damage, Collider2D enemy);
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
    public IEnumerator ExecuteAttack(HandlerComp components, CircleCollider2D hitbox, DoDamage doDamage, Dictionary<AnimationStates, State> states)
    {
        try
        {
            components.Messenger.Attacking = true;
            components.Machine.ChangeAnimation(states[initState]);
            yield return new WaitForEndOfFrame();
            while (components.Machine.CurrentTime() < 1.0f)
            {
                if (timesDamageApplied > 0)
                {
                    Collider2D enemy = Hitted(components.ContactLayer, hitbox);
                    if (enemy)
                    {
                        doDamage(Damage, enemy);
                        timesDamageApplied--;
                    }
                }
            }
            components.Machine.ChangeAnimation(states[endState]);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => components.Machine.CurrentTime() > 1.0f);
            components.Messenger.Attacking = false;
            components.Messenger.InCooldown = true;
            yield return new WaitForSecondsRealtime(CoolDown);
            components.Messenger.InCooldown = false;
        }
        finally
        {
            if (components.Messenger.Hurt)
                components.Messenger.Attacking = false;

            components.Messenger.InCooldown = false;
        }
    }
    private Collider2D Hitted(LayerMask contactLayer, CircleCollider2D hitbox)
    {
        return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);
    }
}
