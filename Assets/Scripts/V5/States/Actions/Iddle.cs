using UnityEngine;

public class Iddle : PlayerState
{
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            return character.States.Walk;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            return character.States.Back;

        if (Input.GetKeyDown(KeyCode.Space))
            return character.States.Jump;

        if (Input.GetKeyDown(KeyCode.U))
            return character.States.LowPunch;

        if (Input.GetKeyDown(KeyCode.I))
            return character.States.MiddlePunch;

        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        character.Animator.Play(AnimationStates.Iddle.ToString());
        character.States.Walk.lastVelocity = character.States.Back.lastVelocity = 0;
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
