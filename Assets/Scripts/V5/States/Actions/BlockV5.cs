using System.Collections;
using UnityEngine;

public class BlockV5 : PlayerState
{
    private bool stopBlock;
    public AttackV5 AttackReceived;
    private Coroutine blockCor;
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (stopBlock)
            return character.States.Iddle;

        return null;
    }

    public override void OnEntry(CharacterV5 character)
    {
        character.Physics.velocity = Vector2.zero;
        character.ReduceResistance(AttackReceived.Damage);

        if (!Input.GetKey(KeyCode.S))
            character.Animator.Play(AnimationState.Block.ToString());
        else
            character.Animator.Play(AnimationState.BlockWhileCrouch.ToString());

        if (blockCor != null)
            character.StopCoroutine(blockCor);

        blockCor = character.StartCoroutine(BlockLogic(character));
    }

    public override void OnExit(CharacterV5 character)
    {
        if (blockCor != null)
            character.StopCoroutine(blockCor);
        character.Animator.speed = 1;
    }

    public override void Update(CharacterV5 character)
    {
    }

    private IEnumerator BlockLogic(CharacterV5 character)
    {

        stopBlock = false;
        Debug.Log($"Aplicando fuerzas {AttackReceived.Force}");


        if (character.transform.localScale.x < 0)
            character.Physics.velocity = new Vector2(7, 0);
        else
            character.Physics.velocity = new Vector2(-7, 0);

        for (int i = 0; i < 130; i++)
        {
            yield return null;
        }

        character.Animator.speed = 0;
        Debug.Log("Bloqueando");
        yield return new WaitForSeconds(0.4f);
        Debug.Log("Desbloqueando");
        character.Animator.speed = 1;

        for (int i = 1; i <= 80; i++)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                break;
            yield return null;

        }
        stopBlock = true;
    }
}