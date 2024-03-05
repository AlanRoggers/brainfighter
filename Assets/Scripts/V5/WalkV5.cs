using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkV5 : PlayerState
{
    Coroutine animationCor = null;
    public WalkV5()
    {
        clips = new List<AnimationStates>()
        {
            AnimationStates.StartWalking,
            AnimationStates.Walk,
        };
    }
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
            return character.States[State.IDDLE];
        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        animationCor = character.StartCoroutine(HandleMultipleAnimations(character));
    }
    public override void OnExit(CharacterV5 character)
    {
        character.StopCoroutine(animationCor);
    }
    public override void Update(CharacterV5 character)
    {
        Debug.Log("Walk");

    }


}
