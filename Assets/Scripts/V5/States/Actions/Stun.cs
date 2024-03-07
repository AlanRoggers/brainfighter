using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : PlayerState
{
    private bool endStun;
    private Coroutine rescueCor;
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (endStun)
            return character.States.Iddle;
        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        endStun = false;
        character.Animator.Play(AnimationState.Incapacite.ToString());
        rescueCor = character.StartCoroutine(RescueTime());
    }
    public override void OnExit(CharacterV5 character)
    {
        if (rescueCor != null)
            character.StopCoroutine(rescueCor);
    }
    public override void Update(CharacterV5 character)
    {
        Debug.Log("Tiempo de recuperación");
    }
    private IEnumerator RescueTime()
    {
        yield return new WaitForSeconds(2f);
        endStun = true;
    }
}
