using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Animator animator;
    public State CurrentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ChangeAnimation(State nextState)
    {
        animator.Play(nextState.StateName.ToString());
    }
    public float CurrentTime() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
}
