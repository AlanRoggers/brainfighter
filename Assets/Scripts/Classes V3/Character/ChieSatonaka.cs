using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    protected override void InitAnimations()
    {
        animations = new Dictionary<AnimationStates, State>();
        // animations.Add(AnimationStates.LowPunch, new ChieLP(this, ))
    }

    protected override void InitAttacks()
    {
        throw new System.NotImplementedException();
    }
}
