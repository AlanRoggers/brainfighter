using System.Collections;
using UnityEngine;

public class Stun : PlayerState
{
    private bool endStun;
    private Coroutine rescueCor;

    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (endStun)
            return character.States.Iddle;

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

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

        character.IncrementResistance(20);
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
