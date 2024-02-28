using System.Collections;
using UnityEngine;

public class AnimationMachine : MonoBehaviour
{
    protected virtual void InitParameters()
    {
        animator = GetComponent<Animator>();
    }
    public AnimationStates CurrentState;
    private Coroutine await_end_animation;
    protected Animator animator;
    public void Transition(AnimationStates toState, bool isFinalState = true)
    {
        CurrentState = toState;
        animator.Play(toState.ToString());
        if (isFinalState)
            await_end_animation = StartCoroutine(AWAIT_END_ANIMATION());
    }
    private IEnumerator AWAIT_END_ANIMATION()
    {
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f
        );
        animator.Play(AnimationStates.Iddle.ToString());
        CurrentState = AnimationStates.Iddle;
    }
    public void InterruptAnimation() => StopCoroutine(await_end_animation);
    public float GetCurrentAnimationTime() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
}
