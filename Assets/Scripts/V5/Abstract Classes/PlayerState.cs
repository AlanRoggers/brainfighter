using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Coroutine animationCor = null;
    protected List<AnimationState> clips;
    public abstract PlayerState InputHandler(CharacterV5 character);
    public abstract void Update(CharacterV5 character);
    public virtual void OnEntry(CharacterV5 character)
    {
        animationCor = character.StartCoroutine(HandleMultipleAnimations(character));
    }
    public virtual void OnExit(CharacterV5 character)
    {
        character.StopCoroutine(animationCor);
    }
    public virtual IEnumerator HandleMultipleAnimations(CharacterV5 character)
    {
        character.Animator.Play(clips[0].ToString());
        for (int i = 1; i < clips.Count; i++)
        {
            yield return new WaitWhile(() =>
                character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f
            );
            character.Animator.Play(clips[i].ToString());
        }
    }
}
