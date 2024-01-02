using System.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    private void AnyAttackLogic(int attack)
    {
        if (attack == 0)
            components.msng.startedWithFirst = true;

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
    public void AnyAttackAnimationHandler(bool endAttack, bool chainedAttack = false, int attack = -1)
    {
        if (endAttack)
        {
            components.msng.clear_attack = null;
            components.msng.chain_oportunity = null;
            if (!chainedAttack)
            {
                components.msng.IsAttacking = false;
                components.msng.startedWithFirst = false;
                components.msng.cooldown_timmer = StartCoroutine(components.msng.COOLDOWN_TIMER());
            }
        }
        else
        {
            components.msng.clear_attack = StartCoroutine(Clear_Attack(attack));
            components.msng.chain_oportunity = StartCoroutine(CHAIN_OPORTUNITY());
        }
    }
    #region Punchs
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
    public void SpecialPunch()
    {
        bool canSpecial = components.msng.startedWithFirst && components.msng.PunchChain[2] &&
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