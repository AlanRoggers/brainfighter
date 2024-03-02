using System.Collections.Generic;

public abstract class Action : Command
{
    public Action(List<AnimationStates> actionStates) : base(actionStates) { }
    public abstract void Execute(HandlerComp components);
}
