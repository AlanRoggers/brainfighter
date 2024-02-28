using System.Collections;
using UnityEngine;

public class Attack
{
    public float CoolDown { get; private set; }
    public int Damage { get; private set; }
    public AnimationStates FinalState { get; private set; }
    public Vector2 Force { get; private set; }
    private readonly CircleCollider2D hitbox;
    public bool HitFreeze { get; private set; }
    public float HitStun { get; private set; }
    public Vector2 Inertia { get; private set; }
    public AnimationStates State { get; private set; }
    private readonly int timesDamagedApplied;
    public Attack(int damage, Vector2 force, Vector2 inertia, float hitStun, float coolDown, bool hitFreeze, AnimationStates state, AnimationStates finalState, CircleCollider2D hitbox)
    {
        HitFreeze = hitFreeze;
        Damage = damage;
        HitStun = hitStun;
        CoolDown = coolDown;
        Force = force;
        Inertia = inertia;
        State = state;
        FinalState = finalState;
        this.hitbox = hitbox;
    }
    public virtual bool CanExecuteAttack()
    {
        return true;
    }
    public virtual IEnumerator ExecuteAttack(AnimationMachine machine, Messenger msng, Rigidbody2D phys, LayerMask contactMask)
    {
        try
        {
            msng.Attacking = true;
            machine.Transition(State, isFinalState: false);
            yield return new WaitForEndOfFrame();
            phys.AddForce(Inertia);
            int damageApplied = timesDamagedApplied;
            while (machine.GetCurrentAnimationTime() > 1.0f)
            {
                Collider2D enemy = HitboxCollision(contactMask);
                if (enemy != null && damageApplied > 0)
                {
                    Rigidbody2D enemyPhys = enemy.GetComponent<Rigidbody2D>();
                    enemyPhys.velocity = Vector2.zero;
                    enemyPhys.AddForce(Force);
                    enemy.GetComponent<PlayerHealth>().ReduceHealth(Damage);
                    damageApplied--;
                }
                yield return null;
            }
            machine.Transition(FinalState);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => machine.GetCurrentAnimationTime() > 1.0f);
            msng.AttackInCooldown = true;
            yield return new WaitForSecondsRealtime(CoolDown);
            ExitState(msng);
        }
        finally
        {
            InterruptState(msng, machine.CurrentState == FinalState);
        }
    }
    protected virtual void ExitState(Messenger msng)
    {
        msng.Attacking = false;
        msng.AttackInCooldown = false;
    }
    private Collider2D HitboxCollision(LayerMask enemyLayer)
    {
        return Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, enemyLayer);
    }
    protected virtual void InterruptState(Messenger msng, bool byCombo)
    {
        if (!byCombo)
            ExitState(msng);
    }
    protected bool NullRestrictions(AnimationStates nullState, AnimationStates currentState) { return nullState == currentState; }
}