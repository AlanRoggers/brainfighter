using System.Collections;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] private AnimationStates currentState;
    private Coroutine await_another_damage = null;
    private Components components;
    public GameObject Reference;
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
        switch (currentState)
        {
            case AnimationStates.Iddle:
                Iddle();
                break;
            case AnimationStates.StartJumping:
                StartJumping();
                break;
            case AnimationStates.Jump:
                Jump();
                break;
            case AnimationStates.StartFalling:
                StartFalling();
                break;
            case AnimationStates.Fall:
                Fall();
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
                TurnCrouch();
                break;
            case AnimationStates.TurnOnAir:
                TurnAir();
                break;
            case AnimationStates.StartRunning:
                StartRunning();
                break;
            case AnimationStates.Run:
                Run();
                break;
            case AnimationStates.Dash:
                Dash();
                break;
            case AnimationStates.DashBack:
                DashBack();
                break;
            case AnimationStates.StartCrouching:
                StartCrouching();
                break;
            case AnimationStates.Crouch:
                Crouch();
                break;
            case AnimationStates.LowPunch:
                LowPunch();
                break;
            case AnimationStates.ChainLowPunch:
                ChainLowPunch();
                break;
            case AnimationStates.MiddlePunch:
                MiddlePunch();
                break;
            case AnimationStates.ChainMiddlePunch:
                ChainMiddlePunch();
                break;
            case AnimationStates.HardPunch:
                HardPunch();
                break;
            case AnimationStates.ChainHardPunch:
                ChainHardPunch();
                break;
            case AnimationStates.SpecialPunch:
                SpecialPunch();
                break;
            case AnimationStates.ChainSpecialPunch:
                ChainSpecialPunch();
                break;
            case AnimationStates.LowKick:
                LowKick();
                break;
            case AnimationStates.ChainLowKick:
                ChainLowKick();
                break;
            case AnimationStates.MiddleKick:
                MiddleKick();
                break;
            case AnimationStates.ChainMiddleKick:
                ChainMiddleKick();
                break;
            case AnimationStates.HardKick:
                HardKick();
                break;
            case AnimationStates.ChainHardKick:
                ChainHardKick();
                break;
            case AnimationStates.SpecialKick:
                SpecialKick();
                break;
            case AnimationStates.ChainSpecialKick:
                ChainSpecialKick();
                break;
            case AnimationStates.KickWhileCrouch:
                break;
            case AnimationStates.AirKick:
                break;
            case AnimationStates.DashAttack:
                break;
            case AnimationStates.Damage:
                Damage();
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
                Land();
                break;
        }
    }
    private void ChangeAnimation(AnimationStates animation)
    {
        currentState = animation;
        components.anim.Play(animation.ToString());
    }

    #region Transitions
    private void Iddle()
    {
        bool walk = components.msng.IsWalking &&
                    (transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                    transform.localScale.x < 0 && components.phys.velocity.x < 0);

        bool goingBack = components.msng.IsWalking && !walk;

        if (components.msng.IsTakingDamage) ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0]) ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1]) ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2]) ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0]) ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1]) ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2]) ChangeAnimation(AnimationStates.HardKick);
        else if (components.msng.IsDashing) ChangeAnimation(AnimationStates.Dash);
        else if (components.msng.IsDashingBack) ChangeAnimation(AnimationStates.DashBack);
        else if (components.msng.IsRunning) ChangeAnimation(AnimationStates.StartRunning);
        else if (components.msng.IsCrouching) ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsJumping) ChangeAnimation(AnimationStates.StartJumping);
        else if (components.msng.NeedTurn) ChangeAnimation(AnimationStates.Turn);
        else if (walk) ChangeAnimation(AnimationStates.StartWalking);
        else if (goingBack) ChangeAnimation(AnimationStates.StartGoingBackwards);
    }

    #region Kicks
    private void HardKick()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainHardKick);
        }
    }
    private void ChainHardKick()
    {
        KickTransitions(2, AnimationStates.SpecialKick);
    }
    private void LowKick()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainLowKick);
        }
    }
    private void ChainLowKick()
    {
        KickTransitions(0, AnimationStates.MiddleKick);
    }
    private void MiddleKick()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainMiddleKick);
        }
    }
    private void ChainMiddleKick()
    {
        KickTransitions(1, AnimationStates.HardKick);
    }
    private void SpecialKick()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainSpecialKick);
        }
    }
    private void ChainSpecialKick()
    {
        KickTransitions(3, AnimationStates.Iddle);
    }
    private void KickTransitions(int attack, AnimationStates nextAttack)
    {
        bool endAttack = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        bool chainedAttack = components.msng.KickChain.Length - 1 >= attack + 1 && components.msng.KickChain[attack + 1];

        if (!endAttack && !components.msng.ChainOportunity)
            components.msng.ChainOportunity = true;

        if (chainedAttack)
        {
            components.msng.ChainOportunity = false;
            components.msng.DamageApplied = false;
            components.msng.KickChain[attack] = false;
            ChangeAnimation(nextAttack);
            return;
        }

        if (endAttack)
        {
            components.msng.ChainOportunity = false;
            components.msng.KickChain[attack] = false;
            components.msng.DamageApplied = false;
            components.msng.IsAttacking = false;
            components.msng.StartedWithFirst = false;
            components.msng.cooldown_timmer = StartCoroutine(components.msng.COOLDOWN_TIMER());
            ChangeAnimation(AnimationStates.Iddle);
        }
    }
    #endregion

    #region Motion
    private void Crouch()
    {
        if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.TurnWhileCrouch);
        else if (!components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.Iddle);
    }
    // Esto fue lo que no eh terminado de implementar por querer implementar primero la programación de lógica y fisicas
    private void Dash()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.IsDashing = false;
            components.phys.velocity = Vector2.zero;
            ChangeAnimation(AnimationStates.Iddle);
        }
    }
    private void DashBack()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.IsDashingBack = false;
            components.phys.velocity = Vector2.zero;
            ChangeAnimation(AnimationStates.Iddle);
        }
    }
    private void Fall()
    {
        if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.TurnOnAir);
        else if (components.msng.IsOnGround)
            ChangeAnimation(AnimationStates.Land);
    }
    private void GoingBackwards()
    {
        bool iddle = components.phys.velocity.x == 0;

        if (iddle)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsDashingBack)
            ChangeAnimation(AnimationStates.DashBack);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);
    }
    private void Jump()
    {
        if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.TurnOnAir);
        else if (components.phys.velocity.y <= 0)
            ChangeAnimation(AnimationStates.StartFalling);
    }
    private void Land()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ChangeAnimation(AnimationStates.Iddle);
    }
    private void Run()
    {
        bool noRunning = !components.msng.IsRunning && components.msng.IsWalking;
        bool walking = noRunning &&
                        (
                            transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                            transform.localScale.x < 0 && components.phys.velocity.x < 0
                        );

        if (!components.msng.IsRunning)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (noRunning && walking)
            ChangeAnimation(AnimationStates.Walk);
        else if (noRunning && !walking)
            ChangeAnimation(AnimationStates.StartGoingBackwards);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);
    }
    private void StartCrouching()
    {
        if (!components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.Iddle);
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ChangeAnimation(AnimationStates.Crouch);
    }
    private void StartFalling()
    {
        if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.TurnOnAir);
        else if (components.msng.IsOnGround)
            ChangeAnimation(AnimationStates.Land);
        else if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ChangeAnimation(AnimationStates.Fall);
    }
    private void StartGoingBackwards()
    {
        bool goingBack = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        if (goingBack)
            ChangeAnimation(AnimationStates.GoingBackwards);
        else if (!components.msng.IsWalking)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsDashingBack)
            ChangeAnimation(AnimationStates.DashBack);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);
    }
    private void StartJumping()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ChangeAnimation(AnimationStates.Jump);
    }
    private void StartRunning()
    {
        bool noRunning = !components.msng.IsRunning && components.msng.IsWalking;
        bool walking = noRunning &&
                        (
                            transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                            transform.localScale.x < 0 && components.phys.velocity.x < 0
                        );

        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ChangeAnimation(AnimationStates.Run);
        else if (components.phys.velocity.x == 0)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (noRunning && walking)
            ChangeAnimation(AnimationStates.Walk);
        else if (noRunning && !walking)
            ChangeAnimation(AnimationStates.StartGoingBackwards);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);
    }
    private void StartWalking()
    {
        bool walk = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;

        if (walk)
            ChangeAnimation(AnimationStates.Walk);
        else if (!components.msng.IsWalking)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsDashing)
            ChangeAnimation(AnimationStates.Dash);
        else if (components.msng.IsRunning)
            ChangeAnimation(AnimationStates.StartRunning);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);

    }
    private void Turn()
    {
        bool animationFinished = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

        AnyTurnLogic(animationFinished);

        if (animationFinished)
        {
            bool iddle = components.phys.velocity.x == 0;
            bool walk = components.msng.IsWalking &&
                        (transform.localScale.x > 0 && components.phys.velocity.x > 0 ||
                        transform.localScale.x < 0 && components.phys.velocity.x < 0);
            bool goingBack = components.msng.IsWalking && !walk;

            if (iddle)
                ChangeAnimation(AnimationStates.Iddle);
            else if (walk)
                ChangeAnimation(AnimationStates.StartWalking);
            else if (goingBack)
                ChangeAnimation(AnimationStates.StartGoingBackwards);
            else if (components.msng.IsJumping)
                ChangeAnimation(AnimationStates.StartJumping);
            else if (components.msng.IsCrouching)
                ChangeAnimation(AnimationStates.StartCrouching);
            else if (components.msng.IsRunning)
                ChangeAnimation(AnimationStates.StartRunning);
            else if (components.msng.IsDashing)
                ChangeAnimation(AnimationStates.Dash);
            else if (components.msng.IsDashingBack)
                ChangeAnimation(AnimationStates.DashBack);
            else if (components.msng.IsTakingDamage)
                ChangeAnimation(AnimationStates.Damage);
            else if (components.msng.PunchChain[0])
                ChangeAnimation(AnimationStates.LowPunch);
            else if (components.msng.PunchChain[1])
                ChangeAnimation(AnimationStates.MiddlePunch);
            else if (components.msng.PunchChain[2])
                ChangeAnimation(AnimationStates.HardPunch);
            else if (components.msng.KickChain[0])
                ChangeAnimation(AnimationStates.LowKick);
            else if (components.msng.KickChain[1])
                ChangeAnimation(AnimationStates.MiddleKick);
            else if (components.msng.KickChain[2])
                ChangeAnimation(AnimationStates.HardKick);
        }
    }
    private void TurnAir()
    {
        bool animationFinished = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

        AnyTurnLogic(animationFinished);

        if (animationFinished)
        {
            if (!components.msng.IsOnGround)
                ChangeAnimation(AnimationStates.StartFalling);
            else
                ChangeAnimation(AnimationStates.Land);
        }
    }
    private void TurnCrouch()
    {
        bool animationFinished = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

        AnyTurnLogic(animationFinished);

        if (animationFinished)
            if (components.msng.IsCrouching)
                ChangeAnimation(AnimationStates.Crouch);
            else
                ChangeAnimation(AnimationStates.Iddle);


    }
    private void AnyTurnLogic(bool animationFinished)
    {
        if (components.msng.NeedTurn)
        {
            components.msng.NeedTurn = false;
            components.msng.IsTurning = true;
        }

        if (animationFinished)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            components.msng.IsTurning = false;
        }
    }
    private void Walk()
    {
        bool iddle = components.phys.velocity.x == 0;

        if (iddle)
            ChangeAnimation(AnimationStates.Iddle);
        else if (components.msng.IsJumping)
            ChangeAnimation(AnimationStates.StartJumping);
        else if (components.msng.NeedTurn)
            ChangeAnimation(AnimationStates.Turn);
        else if (components.msng.IsCrouching)
            ChangeAnimation(AnimationStates.StartCrouching);
        else if (components.msng.IsDashing)
            ChangeAnimation(AnimationStates.Dash);
        else if (components.msng.IsRunning)
            ChangeAnimation(AnimationStates.StartRunning);
        else if (components.msng.IsTakingDamage)
            ChangeAnimation(AnimationStates.Damage);
        else if (components.msng.PunchChain[0])
            ChangeAnimation(AnimationStates.LowPunch);
        else if (components.msng.PunchChain[1])
            ChangeAnimation(AnimationStates.MiddlePunch);
        else if (components.msng.PunchChain[2])
            ChangeAnimation(AnimationStates.HardPunch);
        else if (components.msng.KickChain[0])
            ChangeAnimation(AnimationStates.LowKick);
        else if (components.msng.KickChain[1])
            ChangeAnimation(AnimationStates.MiddleKick);
        else if (components.msng.KickChain[2])
            ChangeAnimation(AnimationStates.HardKick);
    }
    #endregion

    #region Punches
    private void LowPunch()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainLowPunch);
        }
    }
    private void ChainLowPunch()
    {
        PunchTransitions(0, AnimationStates.MiddlePunch);
    }
    private void MiddlePunch()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainMiddlePunch);
        }
    }
    private void ChainMiddlePunch()
    {
        PunchTransitions(1, AnimationStates.HardPunch);
    }
    private void HardPunch()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainHardPunch);
        }
    }
    private void ChainHardPunch()
    {
        PunchTransitions(2, AnimationStates.SpecialPunch);
    }
    private void SpecialPunch()
    {
        if (components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            components.msng.DamageHitbox.enabled = false;
            ChangeAnimation(AnimationStates.ChainSpecialPunch);
        }
    }
    private void ChainSpecialPunch()
    {
        PunchTransitions(3, AnimationStates.Iddle);
    }
    private void PunchTransitions(int attack, AnimationStates nextAttack)
    {
        bool endAttack = components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        bool chainedAttack = components.msng.PunchChain.Length - 1 >= attack + 1 && components.msng.PunchChain[attack + 1];

        if (!endAttack && !components.msng.ChainOportunity)
            components.msng.ChainOportunity = true;

        if (chainedAttack)
        {
            components.msng.ChainOportunity = false;
            components.msng.DamageApplied = false;
            components.msng.PunchChain[attack] = false;
            ChangeAnimation(nextAttack);
            return;
        }

        if (endAttack)
        {
            components.msng.ChainOportunity = false;
            components.msng.PunchChain[attack] = false;
            components.msng.DamageApplied = false;
            components.msng.IsAttacking = false;
            components.msng.StartedWithFirst = false;
            components.msng.cooldown_timmer = StartCoroutine(components.msng.COOLDOWN_TIMER());
            ChangeAnimation(AnimationStates.Iddle);
        }
    }
    #endregion

    #region Emotes
    private void Damage()
    {
        await_another_damage ??= StartCoroutine(AWAIT_ANOTHER_DAMAGE(components.msng.HitStunCausant));
    }
    private IEnumerator AWAIT_ANOTHER_DAMAGE(string attackCausant)
    {
        // print($"Corrutina empezada por el ataque {attackCausant}");
        yield return new WaitForSeconds(components.msng.HitStunTimer);
        if (attackCausant == components.msng.HitStunCausant)
        {
            components.msng.HitStunCausant = "";
            components.msng.IsTakingDamage = false;
            await_another_damage = null;
            ChangeAnimation(AnimationStates.Iddle);
            yield break;
        }
        StopCoroutine(await_another_damage);
        await_another_damage = StartCoroutine(AWAIT_ANOTHER_DAMAGE(components.msng.HitStunCausant));
    }

    #endregion

    #endregion
}