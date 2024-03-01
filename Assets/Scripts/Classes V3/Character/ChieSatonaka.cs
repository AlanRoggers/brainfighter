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

        if (Input.GetKeyDown(KeyCode.U))
        {
            StopWalk();
            StartCoroutine(Attack(attacks[AnimationStates.LowPunch]));
        }

    }
    protected override void InitAnimations()
    {
        animations = new Dictionary<AnimationStates, State>
        {
            { AnimationStates.LowPunch, new ChieLPST() },
            { AnimationStates.Walk, new WalkSt() },
            { AnimationStates.Iddle, new IddleSt() },
            { AnimationStates.GoingBackwards, new BackwardSt() }
        };
    }

    protected override void InitAttacks()
    {
        attacks = new Dictionary<AnimationStates, Attack>
        {
            {AnimationStates.LowPunch, new ChieLP()}
        };
    }
}
