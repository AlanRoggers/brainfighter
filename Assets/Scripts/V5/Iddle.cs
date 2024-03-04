using UnityEngine;

public class Iddle : PlayerState
{
    public Iddle()
    {
        state = State.IDDLE;
    }
    public override void InputHandler(CharacterV5 character)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            character.currentState = CharacterV5.walk;
        }

    }

    public override void Update(CharacterV5 character)
    {
        Debug.Log("Input Iddle");
    }
}
