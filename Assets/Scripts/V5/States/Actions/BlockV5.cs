using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockV5 : PlayerState
{
    private bool stopBlock;
    private Coroutine blockCor;
    public bool AttackFreeze;
    public Vector2 Force;
    public float AttackStun;
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (stopBlock)
        {
            if (Input.GetKey(KeyCode.S))
                return character.States.Crouch;
            else
                return character.States.Iddle;
        }
        return null;
    }

    public override void OnEntry(CharacterV5 character)
    {
        stopBlock = false;
        if (!Input.GetKey(KeyCode.S))
            character.Animator.Play(AnimationStates.Block.ToString());
        else
            character.Animator.Play(AnimationStates.BlockWhileCrouch.ToString());

        if (blockCor != null)
            character.StopCoroutine(blockCor);

        blockCor = character.StartCoroutine(BlockLogic(character));
    }

    public override void OnExit(CharacterV5 character)
    {
        Debug.Log("Saliendo de bloquear");
    }

    public override void Update(CharacterV5 character)
    {

    }

    private IEnumerator BlockLogic(CharacterV5 character)
    {
        stopBlock = false;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        character.Animator.speed = 0;
        Debug.Log("Bloqueando");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Desbloqueando");
        character.Animator.speed = 1;
        yield return null;
        stopBlock = true;
    }
}