using UnityEngine;

public class WalkV5 : PlayerState
{
    public override void InputHandler(CharacterV5 character)
    {
        Debug.Log("Input Walk");
    }

    public override void Update(CharacterV5 character)
    {
        if (character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Walk")
        {
            character.animator.Play(AnimationStates.Walk.ToString());
            Debug.Log("Walk");
        }
    }
}
