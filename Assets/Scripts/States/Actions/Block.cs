using System.Collections;
using UnityEngine;

public class Block : PlayerState
{
    private float timeBlock = 0.19f;
    public delegate void Blocked(PPOAgent agent);
    public event Blocked OnBlock;
    private bool stopBlock;
    private Coroutine blockCor;
    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        if (character.Resistance <= 0)
            return character.States.Stun;

        else if (character.EntryAttack)
            return character.States.Block;

        if (agent.RequestedAction != State.BLOCK)
            return character.States.Iddle;

        return null;
    }
    public override PlayerState InputHandler(Character character)
    {
        if (character.Resistance <= 0)
            return character.States.Stun;

        else if (character.EntryAttack)
            return character.States.Block;

        if (stopBlock)
            return character.States.Iddle;

        return null;
    }
    public override void OnEntry(Character character)
    {
        stopBlock = false;
        character.Physics.velocity = Vector2.zero;

        if (character.Resistance <= 0)
            return;

        OnBlock?.Invoke(character.Agent);
        character.Animator.Play(AnimationState.Block.ToString(), 0, 0);

        if (!character.IsAI)
        {
            character.EntryAttack = false;
            character.ReduceResistance(character.AttackReceived.Damage);
            blockCor = character.StartCoroutine(BlockLogic(character));
        }
    }
    public override void OnExit(Character character)
    {
        character.Animator.speed = 1;
        if (blockCor != null)
        {
            character.StopCoroutine(blockCor);
            blockCor = null;
        }
        if (!character.EntryAttack)
            character.AttackReceived = null;

    }
    public override void Update(Character character)
    {
        if (character.IsAI && character.Resistance > 0 && character.EntryAttack)
        {
            character.EntryAttack = false;
            character.ReduceResistance(character.AttackReceived.Damage);
            blockCor = character.StartCoroutine(BlockLogic(character));
        }
    }
    private IEnumerator BlockLogic(Character character)
    {
        if (character.transform.localScale.x < 0)
            character.Physics.velocity = new Vector2(1, 0);
        else
            character.Physics.velocity = new Vector2(-1, 0);

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

        float awaitTime = timeBlock - character.AttackReceived.HitStun;
        // Debug.Log(awaitTime);
        if (Mathf.Sign(awaitTime) < 0)
        {
            // Debug.Log("Si es mayor");
            yield return new WaitForSeconds(Mathf.Abs(awaitTime / 1.5f));
        }

        stopBlock = true;
        if (!character.EntryAttack)
            character.AttackReceived = null;
        blockCor = null;
    }
}