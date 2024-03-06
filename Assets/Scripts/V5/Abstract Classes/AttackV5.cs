using System.Collections;
using UnityEngine;

public abstract class AttackV5 : PlayerState
{
    //Falta force y damage
    protected AnimationStates currentClip;
    protected Vector2 inertia;
    protected Vector2 force;
    protected int damage;
    protected int timesDamageApplied;
    protected bool hitFreeze;
    protected float coolDown;
    protected float hitFreezeTimer;
    protected float hitStun;
    public override void OnEntry(CharacterV5 character)
    {
        if (character.CoolDownCor != null)
            character.StopCoroutine(character.CoolDownCor);
        animationCor = character.StartCoroutine(Attack(character));
        character.HitsChained++;
    }
    protected virtual IEnumerator Attack(CharacterV5 character)
    {
        character.Animator.Play(clips[0].ToString());
        currentClip = clips[0];

        yield return new WaitForEndOfFrame();

        if (character.transform.localScale.x > 0)
            character.Physics.AddForce(inertia, ForceMode2D.Impulse);
        else
            character.Physics.AddForce(inertia * new Vector2(-1, 1), ForceMode2D.Impulse);

        int times = timesDamageApplied;

        while (character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            if (times > 0)
            {
                Collider2D enemy = character.OverlapDetector.AttackHit(character.Layer == 64 ? 128 : 64, character.Hitbox);
                if (enemy && character.Hitbox.enabled)
                {
                    enemy.GetComponent<CharacterV5>().EntryAttack(damage, force, hitStun, hitFreeze);

                    if (hitFreeze)
                    {
                        Vector2 current = character.Physics.velocity;
                        character.Physics.velocity = Vector2.zero;
                        character.Physics.gravityScale = 0;
                        character.Animator.speed = 0;
                        yield return new WaitForSeconds(hitFreezeTimer);
                        character.Animator.speed = 1;
                        character.Physics.velocity = current;
                        character.Physics.gravityScale = 4;
                    }
                    times--;
                    // GainResistance(attack.ResistanceGained);
                    yield return new WaitForSeconds(0.2f);
                }

            }
            yield return null;
        }

        character.Animator.Play(clips[1].ToString());
        currentClip = clips[1];
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        character.CoolDownCor = character.StartCoroutine(character.CoolDown(coolDown));
        character.HitsChained = 0;
    }
}
