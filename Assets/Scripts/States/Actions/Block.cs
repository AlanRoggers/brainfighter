using System.Collections;
using UnityEngine;

public class Block : PlayerState
{
    public delegate void AgentBlock(int entryDamage, bool whichAgent);
    public event AgentBlock OnBlock;
    private bool stopBlock;
    private Coroutine blockCor;
    public override PlayerState InputAIHandler(Character character)
    {
        if (character.Resistance <= 0)
            return character.States.Stun;
        else if (character.EntryAttack)
            return character.States.Block;

        if (stopBlock)
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

    public override void Update(Character character)
    {
    }

    private IEnumerator BlockLogic(Character character)
    {

        stopBlock = false;

        if (character.transform.localScale.x < 0)
            character.Physics.velocity = new Vector2(7, 0);
        else
            character.Physics.velocity = new Vector2(-7, 0);

        for (int i = 0; i < 130; i++)
        {
            yield return null;
        }

        character.Animator.speed = 0;
        // Debug.Log("Bloqueando");
        yield return new WaitForSeconds(0.4f);
        // Debug.Log("Desbloqueando");
        character.Animator.speed = 1;

        for (int i = 1; i <= 80; i++)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                break;
            yield return null;

        }
        stopBlock = true;
        character.AttackReceived = null;
        blockCor = null;
    }
}