using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            StopWalk();
        else if (Input.GetKey(KeyCode.A))
            Walk(-1);
        else if (Input.GetKey(KeyCode.D))
            Walk(1);
        else
            StopWalk();

    }
    protected override void InitAnimations()
    {
        animations = new Dictionary<AnimationStates, State>
        {
            { AnimationStates.LowPunch, new ChieLowPunchSt() },
            { AnimationStates.StartWalking, new StartWalkSt() },
            { AnimationStates.Walk, new WalkSt() },
            { AnimationStates.Iddle, new IddleSt() },
            { AnimationStates.StartGoingBackwards, new StartBackwardSt() },
            { AnimationStates.GoingBackwards, new BackwardSt() }
        };
    }

    protected override void InitAttacks()
    {
        attacks = new List<Attack>
        {
            new ChieLP()
        };
    }
}
