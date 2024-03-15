using System.Collections;
using UnityEngine;

public abstract class Attack : PlayerState
{
    private float timeAttack;
    public bool HitFreeze { get; protected set; }
    public int Damage { get; protected set; }
    public float HitFreezeTimer { get; protected set; }
    public float HitStun { get; protected set; }
    public Vector2 Force { get; protected set; }
    protected int timesDamageApplied;
    protected float coolDown;
    protected AnimationState currentClip;
    protected Vector2 inertia;
    public override void OnEntry(Character character)
    {
        timeAttack = Time.time;
        if (character.CoolDownCor != null)
            character.StopCoroutine(character.CoolDownCor);

        animationCor = character.StartCoroutine(AttackLogic(character));
        character.HitsChained++;
    }
    protected virtual IEnumerator AttackLogic(Character character)
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
                Collider2D enemy = character.OverlapDetector.AttackHit(character.gameObject.layer == 6 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1"), character.Hitbox);
                if (enemy && character.Hitbox.enabled)
                {
                    // Esto se buguea si los dos se pegan al mismo tiempo
                    enemy.GetComponent<Character>().SetAttack(this);

                    character.IncrementResistance(Damage);

                    if (HitFreeze)
                    {
                        Vector2 current = character.Physics.velocity;
                        character.Physics.velocity = Vector2.zero;
                        character.Physics.gravityScale = 0;
                        character.Animator.speed = 0;
                        yield return new WaitForSeconds(HitFreezeTimer);
                        character.Animator.speed = 1;
                        character.Physics.velocity = current;
                        character.Physics.gravityScale = 4;
                    }
                    times--;
                    if (times > 0)
                        yield return new WaitForSeconds(0.1f); //Aqui iba 0.2f
                }

            }
            yield return null;
        }

        character.Animator.Play(clips[1].ToString());
        currentClip = clips[1];
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        character.CoolDownSet = character.StartCoroutine(character.CoolDown(coolDown));
        character.HitsChained = 0;
        animationCor = null;
    }
    public override void OnExit(Character character)
    {
        base.OnExit(character);
        Debug.Log(Time.time - timeAttack);
        character.RequestedBehaviourAction = State.IDDLE;
        character.Physics.gravityScale = 4;
        character.Animator.speed = 1;
        if (animationCor != null)
            character.StopCoroutine(animationCor);
    }
}
