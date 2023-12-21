using System.Collections;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public Coroutine check_damage;
    public GameObject Reference;
    private bool turn; // VirtualState
    private AnimationStates currentState;
    private Components components;
    private Coroutine await_damage_anim;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        currentState = AnimationStates.Iddle;
        check_damage = null;
        await_damage_anim = null;
    }
    void Update()
    {
        KeepLooking();
        switch (currentState)
        {
            case AnimationStates.Iddle:
                IddleTransitions();
                break;
            case AnimationStates.StartWalking:
            case AnimationStates.Walking:
                WalkfTransitions();
                break;
            case AnimationStates.StartGoingBackwards:
            case AnimationStates.GoingBackwards:
                WalkbTransitions();
                break;
            case AnimationStates.Jump:
                JumpTransitions();
                break;
            case AnimationStates.Fall:
                FallTransitions();
                break;
            case AnimationStates.Kickl:
                KicklTransitions();
                break;
            case AnimationStates.Kickm:
                KickmTransitions();
                break;
            case AnimationStates.Kickh:
                KickhTransitions();
                break;

        }
    }
    private void ChangeAnimation(AnimationStates animation)
    {
        currentState = animation;
        components.anim.Play(animation.ToString());
    }
    private void KeepLooking()
    {
        if (!components.isKicking)
        {
            if (transform.position.x - Reference.transform.position.x <= 0 && transform.localScale.x < 0)
            {
                turn = true;
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                components.kicks.ChangeForceOrientation();
            }
            else if (transform.position.x - Reference.transform.position.x > 0 && transform.localScale.x > 0)
            {
                turn = true;
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                components.kicks.ChangeForceOrientation();
            }
        }
    }

    #region Transitions
    private void FallTransitions()
    {
        if (components.onGround)
        {
            bool activateWalkf = (components.phys.velocity.x > 0 && transform.localScale.x > 0) ||
                                    (components.phys.velocity.x < 0 && transform.localScale.x < 0);
            bool activateWalkb = (components.phys.velocity.x < 0 && transform.localScale.x > 0) ||
                                    (components.phys.velocity.x > 0 && transform.localScale.x < 0);
            bool activateIddle = components.phys.velocity.x == 0;

            if (activateIddle)
                ChangeAnimation(AnimationStates.Iddle);
            else if (activateWalkf)
            {
                ChangeAnimation(AnimationStates.StartWalking);
                StartCoroutine(AWAIT_ANIMATION(AnimationStates.Walking));
            }
            else if (activateWalkb)
            {
                ChangeAnimation(AnimationStates.StartGoingBackwards);
                StartCoroutine(AWAIT_ANIMATION(AnimationStates.GoingBackwards));
            }
        }
    }
    private void IddleTransitions()
    {
        bool activateWalkF = components.isWalking && (
                                    (components.phys.velocity.x > 0 && transform.localScale.x > 0) ||
                                    (components.phys.velocity.x < 0 && transform.localScale.x < 0));
        bool activateWalkB = components.isWalking && (
                                (components.phys.velocity.x < 0 && transform.localScale.x > 0) ||
                                (components.phys.velocity.x > 0 && transform.localScale.x < 0));
        bool activateJump = components.isJumping;

        bool activateKickl = components.isKicking;

        if (activateWalkF)
        {
            ChangeAnimation(AnimationStates.StartWalking);
            StartCoroutine(AWAIT_ANIMATION(AnimationStates.Walking));
        }
        else if (activateWalkB)
        {
            ChangeAnimation(AnimationStates.StartGoingBackwards);
            StartCoroutine(AWAIT_ANIMATION(AnimationStates.GoingBackwards));
        }
        else if (activateJump)
        {
            ChangeAnimation(AnimationStates.Jump);
        }
        else if (activateKickl)
        {
            components.kicks.Combo[0] = true;
            ChangeAnimation(AnimationStates.Kickl);
            components.kicks.KickPhys(currentState);
            check_damage = StartCoroutine(components.kicks.CHECK_DAMAGE(currentState));
            await_damage_anim = StartCoroutine(AWAIT_DAMAGE_ANIMATION(0));
        }
    }
    private void JumpTransitions()
    {
        bool activateFall = components.phys.velocity.y < 0;

        if (activateFall)
            ChangeAnimation(AnimationStates.Fall);
    }
    private void KickhTransitions()
    {
        bool activateIddle = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

        if (activateIddle)
        {
            // print("Acabo HardKick");
            ChangeAnimation(AnimationStates.Iddle);
        }
    }
    private void KicklTransitions()
    {
        bool activateIddle = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        bool activateKickm = components.canCombo;

        if (activateIddle)
        {
            ChangeAnimation(AnimationStates.Iddle);
            // print("Acabo la animación");
        }
        else if (activateKickm)
        {
            if (check_damage != null)
                StopCoroutine(check_damage);
            if (await_damage_anim != null)
                StopCoroutine(await_damage_anim);
            components.canCombo = false;
            components.kicks.Combo[0] = false;
            components.kicks.Combo[1] = true;
            ChangeAnimation(AnimationStates.Kickm);
            components.kicks.KickPhys(currentState);
            check_damage = StartCoroutine(components.kicks.CHECK_DAMAGE(currentState));
            await_damage_anim = StartCoroutine(AWAIT_DAMAGE_ANIMATION(1));
        }
    }
    private void KickmTransitions()
    {
        bool activateIddle = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        bool activateKickh = components.canCombo;

        if (activateIddle)
        {
            // print("Acabo MiddleKick");
            ChangeAnimation(AnimationStates.Iddle);
        }
        else if (activateKickh)
        {
            if (check_damage != null)
                StopCoroutine(check_damage);
            if (await_damage_anim != null)
                StopCoroutine(await_damage_anim);
            components.canCombo = false;
            components.kicks.Combo[1] = false;
            components.kicks.Combo[2] = true;
            ChangeAnimation(AnimationStates.Kickh);
            components.kicks.KickPhys(currentState);
            check_damage = StartCoroutine(components.kicks.CHECK_DAMAGE(currentState));
            await_damage_anim = StartCoroutine(AWAIT_DAMAGE_ANIMATION(2));
        }
    }
    private void WalkfTransitions()
    {
        bool activateIddle = !components.isWalking;
        bool activateJump = components.isJumping;
        bool activateWalkb;

        if (turn)
        {
            turn = false;
            activateWalkb = true;
        }
        else
        {
            activateWalkb = components.phys.velocity.x < 0 && transform.localScale.x == 1 || components.phys.velocity.x > 0 && transform.localScale.x == -1;
        }

        if (activateIddle)
            ChangeAnimation(AnimationStates.Iddle);
        else if (activateWalkb)
        {
            ChangeAnimation(AnimationStates.StartGoingBackwards);
            StartCoroutine(AWAIT_ANIMATION(AnimationStates.GoingBackwards));
        }
        else if (activateJump)
        {
            ChangeAnimation(AnimationStates.Jump);
        }
    }
    private void WalkbTransitions()
    {
        bool activateIddle = !components.isWalking;
        bool activateJump = components.isJumping;
        bool activateWalkf;

        if (turn)
        {
            turn = false;
            activateWalkf = true;
        }
        else
        {
            activateWalkf = components.phys.velocity.x > 0 && transform.localScale.x == 1 || components.phys.velocity.x < 0 && transform.localScale.x == -1;
        }

        if (activateIddle)
            ChangeAnimation(AnimationStates.Iddle);
        else if (activateWalkf)
        {
            ChangeAnimation(AnimationStates.StartWalking);
            StartCoroutine(AWAIT_ANIMATION(AnimationStates.Walking));
        }
        else if (activateJump)
        {
            ChangeAnimation(AnimationStates.Jump);
        }
    }
    #endregion

    private IEnumerator AWAIT_ANIMATION(AnimationStates state)
    {
        yield return new WaitUntil(() =>
            components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        currentState = state;
        components.anim.Play(state.ToString());
    }
    private IEnumerator AWAIT_DAMAGE_ANIMATION(int index)
    {
        yield return null;
        yield return new WaitUntil(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
        // print("Salio de la animación del golpe");
        components.kicks.RestartCombo(index);
        await_damage_anim = null;
    }
}