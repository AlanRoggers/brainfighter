using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Animator animator;
    public AnimationStates CurrentClip;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ChangeAnimation(AnimationStates nextClip)
    {
        animator.Play(nextClip.ToString());
        CurrentClip = nextClip;
    }
    public float CurrentTime() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
}
