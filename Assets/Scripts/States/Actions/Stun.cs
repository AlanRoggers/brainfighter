using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : PlayerState
{
    private bool endStun;
    private Coroutine rescueCor;

    public override PlayerState InputAIHandler(Character character)
    {
        throw new System.NotImplementedException();
    }

    public override PlayerState InputHandler(Character character)
    {
        if (endStun)
            return character.States.Iddle;
        return null;
    }
    public override void OnEntry(Character character)
    {
        endStun = false;
        character.Animator.Play(AnimationState.Incapacite.ToString());
        rescueCor = character.StartCoroutine(RescueTime());
    }
    public override void OnExit(Character character)
    {
        if (rescueCor != null)
            character.StopCoroutine(rescueCor);
    }
    public override void Update(Character character)
    {
        Debug.Log("Tiempo de recuperación");
    }
    private IEnumerator RescueTime()
    {
        yield return new WaitForSeconds(2f);
        endStun = true;
    }
}
