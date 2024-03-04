using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool ArtificialInteligence;
    public Messenger Messenger;
    public StateMachine Machine;
    void Update()
    {
        if (!ArtificialInteligence)
        {
            Punches();
            Kicks();
            Walk();
            Jump();
            Block();
            Crouch();
        }
    }
    private bool AttackGeneralRestrictions() => Messenger.Hurt || Messenger.Attacking || Messenger.InCooldown || Messenger.Blocking || Messenger.Incapacited || !Messenger.InGround || Messenger.Crouching;
    private bool ActionsGeneralRestrictions() => Messenger.Attacking || Messenger.Hurt || Messenger.Blocking || Messenger.Crouching || Messenger.Incapacited;
    private void Crouch()
    {
        if (!Messenger.Attacking && !Messenger.Hurt && !Messenger.Incapacited)
        {
            if (Input.GetKey(KeyCode.S))
                Messenger.Crouching = true;
            else
                Messenger.Crouching = false;
        }
    }
    private void Block()
    {
        if (!Messenger.Attacking && !Messenger.Hurt && !Messenger.Incapacited && !Messenger.InCooldown)
        {
            if (transform.localScale.x > 0)
            {
                if (Input.GetKey(KeyCode.A) && Messenger.DistanceForBlock)
                    Messenger.Blocking = true;
                else
                    Messenger.Blocking = false;
            }
            else if (transform.localScale.x < 0)
            {
                if (Input.GetKey(KeyCode.D) && Messenger.DistanceForBlock)
                    Messenger.Blocking = true;
                else
                    Messenger.Blocking = false;
            }
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !ActionsGeneralRestrictions() && Messenger.InGround)
            Messenger.Jumping = true;
    }
    private void Walk()
    {
        if (!ActionsGeneralRestrictions())
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                Messenger.Walking = 0;
            else if (Input.GetKey(KeyCode.D))
            {
                if (Mathf.Sign(transform.localScale.x) == 1)
                    Messenger.Walking = 1;
                else
                    Messenger.Walking = -1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (Mathf.Sign(transform.localScale.x) == 1)
                    Messenger.Walking = -1;
                else
                    Messenger.Walking = 1;
            }
            else
                Messenger.Walking = 0;
        }
        else
        {
            Messenger.Walking = 0;
        }
    }
    private void Punches()
    {
        if (Input.GetKeyDown(KeyCode.U))
            if (!AttackGeneralRestrictions())
                Messenger.RequestedAttack = AnimationStates.LowPunch;
            else
            {
                if (Messenger.Hurt)
                    Debug.Log("No ataca por culpa de Hurt");
                if (Messenger.Attacking)
                    Debug.Log("No ataca por culpa de Atackking");
                if (Messenger.InCooldown)
                    Debug.Log("No ataca por culpa de InCooldown");
                if (Messenger.Blocking)
                    Debug.Log("No ataca por culpa de Blocking");
                if (Messenger.Incapacited)
                    Debug.Log("No ataca por culpa de InCapacited");
                if (!Messenger.InGround)
                    Debug.Log("No ataca por culpa de InGround");
            }

        if (Input.GetKeyDown(KeyCode.I) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainLowPunch))
        {
            if (Machine.CurrentClip == AnimationStates.ChainLowPunch)
            {
                Messenger.ComboCount++;
            }

            Messenger.RequestedAttack = AnimationStates.MiddlePunch;
        }

        if (Input.GetKeyDown(KeyCode.O) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainMiddlePunch))
        {
            if (Machine.CurrentClip == AnimationStates.ChainMiddlePunch)
            {
                Messenger.ComboCount++;
            }

            Messenger.RequestedAttack = AnimationStates.HardPunch;
        }

        if (Input.GetKeyDown(KeyCode.P) && Machine.CurrentClip == AnimationStates.ChainHardPunch && Messenger.ComboCount >= 2)
        {
            Messenger.RequestedAttack = AnimationStates.SpecialPunch;
        }
    }
    private void Kicks()
    {
        if (Input.GetKeyDown(KeyCode.J) && !AttackGeneralRestrictions())
            Messenger.RequestedAttack = AnimationStates.LowKick;

        if (Input.GetKeyDown(KeyCode.K) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainLowKick))
        {
            if (Machine.CurrentClip == AnimationStates.ChainLowKick)
            {
                Messenger.ComboCount++;
            }

            Messenger.RequestedAttack = AnimationStates.MiddleKick;
        }

        if (Input.GetKeyDown(KeyCode.L) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainMiddleKick))
        {
            if (Machine.CurrentClip == AnimationStates.ChainMiddleKick)
            {
                Messenger.ComboCount++;
            }

            Messenger.RequestedAttack = AnimationStates.HardKick;
        }

        if (Input.GetKeyDown(KeyCode.Semicolon) && Machine.CurrentClip == AnimationStates.ChainHardKick && Messenger.ComboCount >= 2)
        {
            if (Machine.CurrentClip == AnimationStates.ChainHardKick)

                Messenger.RequestedAttack = AnimationStates.SpecialKick;
        }

        if (Input.GetKeyDown(KeyCode.J) && Messenger.Crouching && !Messenger.Blocking && !Messenger.Incapacited && !Messenger.Attacking && !Messenger.Hurt)
            Messenger.RequestedAttack = AnimationStates.KickWhileCrouch;
    }
}
