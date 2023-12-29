using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class StateController : MonoBehaviour
{
    public bool TurnHandler;
    public GameObject Reference;
    [SerializeField]
    private AnimationStates currentState;
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        currentState = AnimationStates.Iddle;
    }
    void Update()
    {
        if (currentState == AnimationStates.Iddle || currentState == AnimationStates.Jump ||
            currentState == AnimationStates.Fall || currentState == AnimationStates.Walk ||
            currentState == AnimationStates.Crouch || currentState == AnimationStates.Run ||
            currentState == AnimationStates.GoingBackwards)
            TurnHandler = transform.localScale.x != KeepLooking();

        switch (currentState)
        {
            case AnimationStates.Iddle:
                Iddle();
                break;
            case AnimationStates.StartJumping:
                break;
            case AnimationStates.Jump:
                break;
            case AnimationStates.StartFalling:
                break;
            case AnimationStates.Fall:
                break;
            case AnimationStates.StartWalking:
                StartWalking();
                break;
            case AnimationStates.Walk:
                Walk();
                break;
            case AnimationStates.StartGoingBackwards:
                StartGoingBackwards();
                break;
            case AnimationStates.GoingBackwards:
                GoingBackwards();
                break;
            case AnimationStates.Turn:
                Turn();
                break;
            case AnimationStates.TurnWhileCrouch:
                break;
            case AnimationStates.TurnOnAir:
                break;
            case AnimationStates.StartRunning:
                break;
            case AnimationStates.Run:
                break;
            case AnimationStates.Dash:
                break;
            case AnimationStates.DashBack:
                break;
            case AnimationStates.LowPunch:
                break;
            case AnimationStates.MiddlePunch:
                break;
            case AnimationStates.HardPunch:
                break;
            case AnimationStates.SpecialPunch:
                break;
            case AnimationStates.LowKick:
                break;
            case AnimationStates.MiddleKick:
                break;
            case AnimationStates.HardKick:
                break;
            case AnimationStates.SpecialKick:
                break;
            case AnimationStates.KickWhileCrouch:
                break;
            case AnimationStates.AirKick:
                break;
            case AnimationStates.DashAttack:
                break;
            case AnimationStates.Damage:
                break;
            case AnimationStates.DamageWhileCrouch:
                break;
            case AnimationStates.InFloor:
                break;
            case AnimationStates.Recover:
                break;
            case AnimationStates.Dead:
                break;
            case AnimationStates.Land:
                break;
        }
    }
    private void ChangeAnimation(AnimationStates animation)
    {
        currentState = animation;
        components.anim.Play(animation.ToString());
    }
    private int KeepLooking()
    {
        if (!components.msng.IsKicking)
        {
            if (transform.position.x - Reference.transform.position.x <= 0 && transform.localScale.x < 0)
                return 1;
            else if (transform.position.x - Reference.transform.position.x > 0 && transform.localScale.x > 0)
                return -1;
            else return (int)transform.localScale.x;
        }
        else return (int)transform.localScale.x;
    }
    private IEnumerator AWAIT_ANIMATION(AnimationStates state)
    {
        yield return new WaitUntil(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        currentState = state;
        components.anim.Play(state.ToString());
    }

    #region Transitions
    private void Iddle()
    {
        bool walk = components.msng.IsWalking &&
                    (transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                    transform.localScale.x < 0 && components.phys.velocity.x < 0);

        bool goingBack = components.msng.IsWalking && !walk;

        if (components.msng.IsCrouching) ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsJumping) ChangeAnimation(AnimationStates.StartJumping);
        else if (TurnHandler)
        {
            ChangeAnimation(AnimationStates.Turn);
        }
        else if (walk) ChangeAnimation(AnimationStates.StartWalking);
        else if (goingBack) ChangeAnimation(AnimationStates.StartGoingBackwards);
    }
    private void StartJumping()
    {

    }
    private void Jump()
    {

    }
    private void StartFalling()
    {

    }
    private void Fall()
    {

    }
    private void StartWalking()
    {
        bool walk = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        if (walk) ChangeAnimation(AnimationStates.Walk);
    }
    private void Walk()
    {
        bool iddle = components.phys.velocity.x == 0;

        if (components.msng.IsJumping) ChangeAnimation(AnimationStates.StartJumping);
        else if (TurnHandler) ChangeAnimation(AnimationStates.Turn);
        else if (iddle) ChangeAnimation(AnimationStates.Iddle);
    }
    private void StartGoingBackwards()
    {
        bool goingBack = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        if (goingBack) ChangeAnimation(AnimationStates.GoingBackwards);
    }
    private void GoingBackwards()
    {
        bool iddle = components.phys.velocity.x == 0;

        if (TurnHandler) ChangeAnimation(AnimationStates.Turn);
        else if (iddle) ChangeAnimation(AnimationStates.Iddle);
    }
    private void Turn()
    {
        bool animationFinished = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

        if (animationFinished)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            bool iddle = components.phys.velocity.x == 0;
            bool walk = components.msng.IsWalking &&
                        (transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                        transform.localScale.x < 0 && components.phys.velocity.x < 0);
            bool goingBack = components.msng.IsWalking && !walk;

            if (iddle) ChangeAnimation(AnimationStates.Iddle);
            else if (walk) ChangeAnimation(AnimationStates.StartWalking);
            else if (goingBack) ChangeAnimation(AnimationStates.StartGoingBackwards);
        }
    }
    private void Land()
    {

    }
    private void Dash()
    {
        StartCoroutine(components.motion.NO_DASHING());
    }
    #endregion
}