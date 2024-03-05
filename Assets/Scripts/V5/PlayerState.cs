using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected List<AnimationStates> clips;
    public abstract PlayerState InputHandler(CharacterV5 character);
    public abstract void Update(CharacterV5 character);
    public abstract void OnEntry(CharacterV5 character);
    public abstract void OnExit(CharacterV5 character);
    public virtual IEnumerator HandleMultipleAnimations(CharacterV5 character)
    {
        character.animator.Play(clips[0].ToString());
        for (int i = 1; i < clips.Count; i++)
        {
            yield return new WaitWhile(() =>
                character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f
            );
            character.animator.Play(clips[i].ToString());
        }
    }
}
