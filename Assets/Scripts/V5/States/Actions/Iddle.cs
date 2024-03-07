using UnityEngine;

public class Iddle : PlayerState
{
    public override PlayerState InputHandler(CharacterV5 character)
    {
        if (!character.IsAI)
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

                // if (Input.GetKeyDown(KeyCode.P))
                //     return character.States.SpecialPunch;

                // if (Input.GetKeyDown(KeyCode.Semicolon))
                //     return character.States.SpecialKick;
            }
        }
        if (character.gameObject.layer == 7 && Input.GetKeyDown(KeyCode.LeftArrow))
            return character.States.LowKick;

        return null;
    }
    public override void OnEntry(CharacterV5 character)
    {
        // Debug.Log("Iddle");
        character.Animator.Play(AnimationState.Iddle.ToString());
        character.LastVelocity = 0;
    }
    public override void OnExit(CharacterV5 character)
    {
        // Debug.Log("Saliendo de Iddle");
    }
    public override void Update(CharacterV5 character)
    {
        // if (character.Animator.speed == 0)
        //     character.Animator.speed = 1;
    }
}
