using System;
using System.Collections;
using UnityEngine;

public class Hurt : PlayerState
{
    private float timeHurt = 0.57f;
    private Coroutine hurtCor;
    private bool canExitState;
    public delegate void AgentHurt(int entryDamage, bool whichAgent);
    public event AgentHurt OnHurt;
    public override PlayerState InputAIHandler(Character character, PPOAgent agent) => SharedActions(character);
    public override PlayerState InputHandler(Character character) => SharedActions(character);
    public override void OnEntry(Character character)
    {
        // timeHurt = Time.time;
        if (character.AttackReceived != null)
        {
            character.EntryAttack = false;
            character.ReduceHealth(character.AttackReceived.Damage);

            if (character.Health <= 0)
                return;

            // OnHurt.Invoke(character.AttackReceived.Damage, character.gameObject.layer == 6);


            character.Friction.friction = 0;
            character.Animator.Play(AnimationState.Damage.ToString());
            if (hurtCor != null)
            {
                character.StopCoroutine(hurtCor);
                character.Physics.velocity = Vector2.zero;
            }
            hurtCor = character.StartCoroutine(HurtLogic(character));
        }
        else
            canExitState = true;

    }
    public override void OnExit(Character character)
    {
        // Debug.Log(Time.time - timeHurt);
        character.Friction.friction = 1;
        character.Animator.speed = 1;
        if (hurtCor != null)
        {
            character.StopCoroutine(hurtCor);
            hurtCor = null;
        }
        canExitState = false;
    }
    public override void Update(Character character)
    {
        // Debug.Log("Herido");
    }
    private IEnumerator HurtLogic(Character character)
    {
        if (character.transform.localScale.x < 0)
            character.Physics.AddForce(character.AttackReceived.Force, ForceMode2D.Impulse);
        else
            character.Physics.AddForce(character.AttackReceived.Force * new Vector2(-1, 1), ForceMode2D.Impulse); //Aca da bugs
        yield return new WaitForEndOfFrame();

        if (character.AttackReceived.HitFreeze)
        {
            Vector2 current = character.Physics.velocity;
            character.Physics.velocity = Vector2.zero;
            character.Physics.gravityScale = 0;
            character.Animator.speed = 0;
            yield return new WaitForSeconds(character.AttackReceived.HitFreezeTimer);
            character.Animator.speed = 1;
            character.Physics.velocity = current;
            character.Physics.gravityScale = 4;
        }

        yield return new WaitUntil(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
        float awaitTime = timeHurt - character.AttackReceived.HitStun;
        if (Math.Sign(awaitTime) < 0)
            yield return new WaitForSeconds(Mathf.Abs(awaitTime));
        character.AttackReceived = null;
        canExitState = true;
        hurtCor = null;
    }
    private PlayerState SharedActions(Character character)
    {
        if (character.Health <= 0)
            return character.States.Dead;

        if (character.EntryAttack)
            return character.States.Hurt;

        if (canExitState)
        {
            if (character.OverlapDetector.GroundDetection(character.Body, LayerMask.GetMask("Ground")))
                return character.States.Iddle;
            else
                return character.States.Fall;
        }
        return null;
    }
}
