using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChieLowPunchSt : State
{
    public ChieLowPunchSt()
    {
        StateName = AnimationStates.LowPunch;
    }
    public override void Transitions(StateMachine animator, Messenger msng, Dictionary<AnimationStates, State> states)
    {
        return;
    }
}
