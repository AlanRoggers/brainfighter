using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Messenger Messenger;
    public StateMachine Machine;
    void Update()
    {
        Walk();


        if (Input.GetKeyDown(KeyCode.U) && !AttackGeneralRestrictions())
            Messenger.RequestedAttack = AnimationStates.LowPunch;

        if (Input.GetKeyDown(KeyCode.I) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainLowPunch))
        {
            if (Machine.CurrentClip == AnimationStates.ChainLowPunch)
                Debug.Log("[Combo]");

            Messenger.RequestedAttack = AnimationStates.MiddlePunch;
        }

        if (Input.GetKeyDown(KeyCode.O) && (!AttackGeneralRestrictions() || Machine.CurrentClip == AnimationStates.ChainMiddlePunch))
        {
            if (Machine.CurrentClip == AnimationStates.ChainMiddlePunch)
                Debug.Log("[Combo]");

            Messenger.RequestedAttack = AnimationStates.HardPunch;
        }

        if (Input.GetKeyDown(KeyCode.P) && Machine.CurrentClip == AnimationStates.ChainHardPunch)
        {
            if (Machine.CurrentClip == AnimationStates.ChainHardPunch)
                Debug.Log("[Combo]");

            Messenger.RequestedAttack = AnimationStates.SpecialPunch;
        }

    }
    private bool AttackGeneralRestrictions() => Messenger.Hurt || Messenger.Attacking || Messenger.InCooldown;

    private bool ActionsGeneralRestrictions() => Messenger.Attacking || Messenger.Hurt;
    private void Walk()
    {
        if (!ActionsGeneralRestrictions())
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                Messenger.Walking = 0;
            else if (Input.GetKey(KeyCode.D))
                Messenger.Walking = 1;
            else if (Input.GetKey(KeyCode.A))
                Messenger.Walking = -1;
            else
                Messenger.Walking = 0;
        }
        else
        {
            Messenger.Walking = 0;
        }
    }
}
