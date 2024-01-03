using System.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    public void AnyAttackAnimationHandler(bool endAttack, bool chainedAttack = false, bool isKick = false, int attack = -1)
    {
        if (endAttack)
        {
            components.msng.clear_attack = null;
            components.msng.chain_oportunity = null;
            if (!chainedAttack)
            {
                components.msng.IsAttacking = false;
                components.msng.StartedWithFirst = false;
                components.msng.cooldown_timmer = StartCoroutine(components.msng.COOLDOWN_TIMER());
            }
        }
        else
        {
            components.msng.clear_attack = StartCoroutine(Clear_Attack(attack, isKick: isKick));
            components.msng.chain_oportunity = StartCoroutine(CHAIN_OPORTUNITY());
        }
    }
    private void AnyAttackLogic(int attack, bool isKick = false)
    {
        if (attack == 0)
            components.msng.StartedWithFirst = true;

        if (isKick)
            components.msng.KickChain[attack] = true;
        else
            components.msng.PunchChain[attack] = true;

        components.msng.IsAttacking = true;
        components.phys.velocity = new Vector2(0, 0);
        components.msng.IsWalking = false;
        components.msng.IsRunning = false;
    }
    private IEnumerator Clear_Attack(int attack, bool isKick = false)
    {
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        if (isKick)
            components.msng.KickChain[attack] = false;
        else
            components.msng.PunchChain[attack] = false;
    }
    private IEnumerator CHAIN_OPORTUNITY()
    {
        yield return new WaitUntil(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.35f);
        print("Chain Time" + components.anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
        components.msng.ChainOportunity = true;
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);
        print("No Chain Time" + components.anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
        components.msng.ChainOportunity = false;
    }

    #region Kicks
    public void HardKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        bool chainOportunity = components.msng.KickChain[1] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(2, isKick: true);
            // Fisicas de la patada
            //Daño y contacto de la patada
        }
    }
    public void LowKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        if (canAttack)
        {
            AnyAttackLogic(0, isKick: true);
            // Fisicas de la patada
            //Daño y contacto de la patada
        }
    }
    public void MiddleKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        bool chainOportunity = components.msng.KickChain[0] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(1, isKick: true);
            // Fisicas de la patada
            //Daño y contacto de la patada
        }
    }
    public void SpecialKick()
    {
        bool canSpecial = components.msng.StartedWithFirst && components.msng.KickChain[2] &&
                            components.msng.ChainOportunity;

        if (canSpecial)
        {
            AnyAttackLogic(3, isKick: true);
            // Fisicas
            // Daño
        }
    }
    #endregion

    #region Punchs
    public void HardPunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        bool chainOportunity = components.msng.PunchChain[1] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(2);
            // Fisicas del tercer golpe
            // Daño y contacto del primer golpe
        }
    }
    public void LowPunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        if (canAttack)
        {
            AnyAttackLogic(0);
            // Fisicas del primer golpe
            // Daño y contacto del primer golpe
        }
    }
    public void MiddlePunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        bool chainOportunity = components.msng.PunchChain[0] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(1);
            // Fisicas del segundo golpe
            // Daño y contacto del primer golpe
        }
    }
    public void SpecialPunch()
    {
        bool canSpecial = components.msng.StartedWithFirst && components.msng.PunchChain[2] &&
                            components.msng.ChainOportunity;

        if (canSpecial)
        {
            AnyAttackLogic(3);
            // Fisicas del golpe especial
            // Daño y contacto del primer golpe
        }
    }
    #endregion
}