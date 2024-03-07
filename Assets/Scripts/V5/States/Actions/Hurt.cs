using System.Collections;
using UnityEngine;

public class Hurt : PlayerState
{
    public AttackV5 AttackReceived;
    private bool canExitState;
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (canExitState)
            return character.States.Iddle;
        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        character.Friction.friction = 0;
        character.Animator.Play(AnimationState.Damage.ToString());
        canExitState = false;
        if (character.HurtCor != null)
        {
            character.StopCoroutine(character.HurtCor);
            character.Physics.velocity = Vector2.zero;
        }
        character.HurtCor = character.StartCoroutine(HurtLogic(character));

    }
    public override void OnExit(CharacterV5 character)
    {
        character.Friction.friction = 1;
        character.Animator.speed = 1;
    }
    public override void Update(CharacterV5 character)
    {
        // Debug.Log("Herido");
    }
    private IEnumerator HurtLogic(CharacterV5 character)
    {
        if (character.transform.localScale.x < 0)
            character.Physics.AddForce(AttackReceived.Force, ForceMode2D.Impulse);
        else
            character.Physics.AddForce(AttackReceived.Force * new Vector2(-1, 1), ForceMode2D.Impulse);
        yield return new WaitForEndOfFrame();

        if (AttackReceived.HitFreeze)
        {
            Vector2 current = character.Physics.velocity;
            character.Physics.velocity = Vector2.zero;
            character.Physics.gravityScale = 0;
            character.Animator.speed = 0;
            yield return new WaitForSeconds(AttackReceived.HitFreezeTimer);
            character.Animator.speed = 1;
            character.Physics.velocity = current;
            character.Physics.gravityScale = 4;
        }

        yield return new WaitUntil(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
        int framesWaiting = (int)AttackReceived.HitStun;
        while (framesWaiting > 0)
        {
            framesWaiting--;
            yield return null;
        }
        canExitState = true;
    }
}
