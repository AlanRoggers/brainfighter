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
    public void Freeze(HandlerComp comp)
    {
        comp.Physics.velocity = Vector2.zero;
        comp.Physics.gravityScale = 0;
        animator.speed = 0;

    }
    public void UnFreeze(HandlerComp comp, Vector2 velocity)
    {
        animator.speed = 1;
        comp.Physics.velocity = velocity;
        comp.Physics.gravityScale = 4;
    }
}
