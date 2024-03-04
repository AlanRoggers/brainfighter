using UnityEngine;

public abstract class PlayerState
{
    public State state;
    public abstract void InputHandler(CharacterV5 character);
    public abstract void Update(CharacterV5 character);
}
