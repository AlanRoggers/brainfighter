using System.Collections;
using UnityEngine;

public class Block : PlayerState
{
    public delegate void AgentBlock(int entryDamage, bool whichAgent);
    public event AgentBlock OnBlock;
    private bool stopBlock;
    private Coroutine blockCor;
    public override PlayerState InputAIHandler(Character character, PPOAgent agent) => SharedActions(character);
    public override PlayerState InputHandler(Character character) => SharedActions(character);
    public override void OnEntry(Character character)
    {
        character.EntryAttack = false;
        character.Physics.velocity = Vector2.zero;
        character.ReduceResistance(character.AttackReceived.Damage);

        if (character.Resistance <= 0)
            return;

        // OnBlock.Invoke(character.AttackReceived.Damage, character.gameObject.layer == 6);

        character.Animator.Play(AnimationState.Block.ToString());

        blockCor = character.StartCoroutine(BlockLogic(character));
    }
    public override void OnExit(Character character)
    {
        character.Animator.speed = 1;
        if (blockCor != null)
        {
            character.StopCoroutine(blockCor);
            blockCor = null;
        }
        stopBlock = false;
    }
    public override void Update(Character character) { }
    private IEnumerator BlockLogic(Character character)
    {

        stopBlock = false;

        if (character.transform.localScale.x < 0)
            character.Physics.velocity = new Vector2(1, 0);
        else
            character.Physics.velocity = new Vector2(-1, 0);

        yield return new WaitUntil(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);

        character.Animator.speed = 0;
        yield return new WaitForSeconds(1f);
        character.Animator.speed = 1;

        stopBlock = true;
        character.AttackReceived = null;
        blockCor = null;
    }
    private PlayerState SharedActions(Character character)
    {
        if (character.Resistance <= 0)
            return character.States.Stun;
        else if (character.EntryAttack)
            return character.States.Block;

        if (stopBlock)
            return character.States.Iddle;

        return null;
    }
}