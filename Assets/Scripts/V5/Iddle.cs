using UnityEngine;

public class Iddle : PlayerState
{
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKey(KeyCode.D))
            return character.States[State.WALK];
        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        character.animator.Play(AnimationStates.Iddle.ToString());
    }
    public override void OnExit(CharacterV5 character)
    {
        Debug.Log("Saliendo de Iddle");
    }
    public override void Update(CharacterV5 character)
    {
        Debug.Log("Iddle");
    }
}
