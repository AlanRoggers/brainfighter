using UnityEngine;

public class Iddle : PlayerState
{
    public override PlayerState InputAIHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            return character.States.MiddleKick;

        if (!character.OnColdoown)
        {
            switch (character.RequestedBehaviourAction)
            {
                case State.LOW_PUNCH:
                    if (!character.OnColdoown)
                        return character.States.LowPunch;
                    break;
                case State.MIDDLE_PUNCH:
                    if (!character.OnColdoown)
                        return character.States.MiddlePunch;
                    break;
                case State.HARD_PUNCH:
                    if (!character.OnColdoown)
                        return character.States.HardPunch;
                    break;
                case State.LOW_KICK:
                    if (!character.OnColdoown)
                        return character.States.LowKick;
                    break;
                case State.MIDDLE_KICK:
                    if (!character.OnColdoown)
                        return character.States.MiddleKick;
                    break;
                case State.HARD_KICK:
                    if (!character.OnColdoown)
                        return character.States.HardKick;
                    break;
            }
        }

        if (character.RequestedBehaviourAction == State.JUMP)
            return character.States.Jump;

        switch (character.RequestedMotionAction)
        {
            case State.WALK:
                if (character.transform.localScale.x > 0)
                    return character.States.Walk;
                else
                    return character.States.Back;
            case State.BACK:
                if (character.transform.localScale.x > 0)
                    return character.States.Back;
                else
                    return character.States.Walk;
        }

        return null;
    }
    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (character.transform.localScale.x > 0)
                return character.States.Walk;
            else
                return character.States.Back;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (character.transform.localScale.x > 0)
                return character.States.Back;
            else
                return character.States.Walk;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            return character.States.Jump;

        if (!character.OnColdoown)
        {
            if (Input.GetKeyDown(KeyCode.U))
                return character.States.LowPunch;

            if (Input.GetKeyDown(KeyCode.I))
                return character.States.MiddlePunch;

            if (Input.GetKeyDown(KeyCode.O))
                return character.States.HardPunch;

            if (Input.GetKeyDown(KeyCode.J))
                return character.States.LowKick;

            if (Input.GetKeyDown(KeyCode.K))
                return character.States.MiddleKick;

            if (Input.GetKeyDown(KeyCode.L))
                return character.States.HardKick;

        }


        return null;
    }
    public override void OnEntry(Character character)
    {
        // Debug.Log("Iddle");
        character.Animator.Play(AnimationState.Iddle.ToString());
        character.LastVelocity = 0;
    }
    public override void OnExit(Character character)
    {
        // Debug.Log("Saliendo de Iddle");
    }
    public override void Update(Character character)
    {
        // if (character.Animator.speed == 0)
        //     character.Animator.speed = 1;
    }
}
