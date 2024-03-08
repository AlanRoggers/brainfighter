using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Coroutine animationCor = null;
    protected List<AnimationState> clips;
    public abstract PlayerState InputAIHandler(Character character);
    public abstract PlayerState InputHandler(Character character);
    public abstract void Update(Character character);
    public virtual void OnEntry(Character character)
    {
        animationCor = character.StartCoroutine(HandleMultipleAnimations(character));
    }
    public virtual void OnExit(Character character)
    {
        character.StopCoroutine(animationCor);
    }
    public virtual IEnumerator HandleMultipleAnimations(Character character)
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
