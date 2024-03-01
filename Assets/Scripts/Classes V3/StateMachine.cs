using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Animator animator;
    public State CurrentState;
    public AnimationStates CurrentClip;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ChangeAnimation(State nextState)
    {
        CurrentState = nextState;
        animator.Play(nextState.StateName.ToString());
        CurrentClip = nextState.StateName;
    }
    public void ChangeAnimation(AnimationStates nextClip)
    {
        animator.Play(nextClip.ToString());
        CurrentClip = nextClip;
    }
    public float CurrentTime() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
}
