using System.Collections;
using UnityEngine;

public class Stun : PlayerState
{
    public delegate void AgentStuned(bool whichAgent);
    public event AgentStuned OnStun;
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
        // OnStun.Invoke(character.gameObject.layer == 6);
        character.Animator.Play(AnimationState.Incapacite.ToString());
        rescueCor = character.StartCoroutine(RescueTime());
    }
    public override void OnExit(Character character)
    {
        endStun = false;
        if (rescueCor != null)
        {
            character.StopCoroutine(rescueCor);
            rescueCor = null;
        }

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
        rescueCor = null;
    }
}
