using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public abstract class Character : Agent
{
    public string Name;
    public int Health;
    protected List<Attack> attacks;
    protected Dictionary<AnimationStates, State> animations;
    protected StateMachine stateMachine;
    protected Messenger messenger;
    protected virtual void LateUpdate()
    {
        // Mantener siempre comprobandose las transiciones del estado actual
        stateMachine.CurrentState.Transitions(stateMachine, messenger, animations);
    }
    protected virtual void Walk()
    {
        Debug.Log("[Caminando]");
    }
    protected virtual void StopWalk()
    {
        Debug.Log("[No caminar]");
    }
    protected virtual void Jump()
    {
        Debug.Log("[Saltar]");
    }
    protected virtual void Dash()
    {
        Debug.Log("[Dash]");
    }
    protected virtual void Block()
    {
        Debug.Log("[Bloquear]");
    }
    protected virtual void Crouch()
    {
        Debug.Log("[Agacharse]");
    }
    protected abstract void InitAttacks();
    protected abstract void InitAnimations();
}
