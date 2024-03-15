using UnityEngine;

public class Iddle : PlayerState
{
    public override PlayerState InputAIHandler(Character character, PPOAgent agent)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (agent.RequestedAction == State.JUMP)
            return character.States.Jump;

        if (agent.RequestedAction == State.WALK)
        {
            if (character.transform.localScale.x > 0)
                return character.States.Walk;
            else
                return character.States.Back;
        }

        if (agent.RequestedAction == State.BACK)
        {
            if (character.transform.localScale.x > 0)
                return character.States.Back;
            else
                return character.States.Walk;
        }

        if (!character.OnColdoown)
        {
            switch (agent.RequestedAction)
            {
                case State.LOW_PUNCH:
                    return character.States.LowPunch;
                case State.MIDDLE_PUNCH:
                    return character.States.MiddlePunch;
                case State.HARD_PUNCH:
                    return character.States.HardPunch;
                case State.LOW_KICK:
                    return character.States.LowKick;
                case State.MIDDLE_KICK:
                    return character.States.MiddleKick;
                case State.HARD_KICK:
                    return character.States.HardKick;
            }
        }

        return null;
    }

    public override PlayerState InputHandler(Character character)
    {
        if (character.EntryAttack)
            return character.States.Hurt;

        if (character.gameObject.layer == 7)
        {
            if (Input.GetKeyDown(KeyCode.M))
                return character.States.LowPunch;
        }

        if (character.AcceptInput)
        {

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
        }
        return null;
    }
    public override void OnEntry(Character character)
    {
        // Debug.Log("Iddle");
        character.Animator.Play(AnimationState.Iddle.ToString());
    }
    public override void Update(Character character)
    {
        // if (character.Animator.speed == 0)
        //     character.Animator.speed = 1;
    }
}
