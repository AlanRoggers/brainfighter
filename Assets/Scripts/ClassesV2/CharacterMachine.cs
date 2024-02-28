using System;
using System.Collections;
using UnityEngine;

public class CharacterMachine : AnimationMachine
{
    private void Awake()
    {
        InitParameters();
    }
    public void WalkAnimation(int direction)
    {
        bool initAnimation = CurrentState != AnimationStates.StartWalking && CurrentState != AnimationStates.StartGoingBackwards;
        bool movementAnimation = CurrentState == AnimationStates.Walk || CurrentState == AnimationStates.GoingBackwards;
        // Debug.Log($"iniciar animación {initAnimation}. animacion de movimiento {movementAnimation}");
        if (initAnimation && !movementAnimation)
        {
            // Debug.Log($"[Walk] Inicio de la animación {CurrentState}");
            if (MathF.Sign(direction) == MathF.Sign(transform.localScale.x))
                Transition(AnimationStates.StartWalking, isFinalState: false);
            else
                Transition(AnimationStates.StartGoingBackwards, isFinalState: false);
        }
        else if (!movementAnimation)
        {
            // Debug.Log("[Walk] Loop de la animación");
            if (GetCurrentAnimationTime() > 1.0)
            {
                if (CurrentState == AnimationStates.StartWalking)
                    Transition(AnimationStates.Walk, isFinalState: false);
                else if (CurrentState == AnimationStates.StartGoingBackwards)
                    Transition(AnimationStates.GoingBackwards, isFinalState: false);
            }
        }
    }
    public void JumpAnimation()
    {
        StartCoroutine(JUMP_ANIMATION());

    }
    private IEnumerator JUMP_ANIMATION()
    {
        Transition(AnimationStates.StartJumping);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GetCurrentAnimationTime() > 1.0f);
        Transition(AnimationStates.Jump);
        yield return new WaitUntil(() => GetComponent<Rigidbody2D>().velocity.y <= 0);
        Transition(AnimationStates.StartFalling);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GetCurrentAnimationTime() > 1.0f);
        Transition(AnimationStates.Fall);
    }
}
