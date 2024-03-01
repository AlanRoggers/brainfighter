using System.Collections.Generic;
using UnityEngine;

public abstract class ActionV4 : Command
{
    public ActionV4(List<AnimationStates> actionStates) : base(actionStates) { }
    public abstract void Execute(HandlerComp components);
}
