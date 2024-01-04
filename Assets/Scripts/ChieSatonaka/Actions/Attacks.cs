using System.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public float Timer;
    public Vector2[] AttackForces = new Vector2[8];
    public AnimationClip[] attackClips = new AnimationClip[8];
    private bool damageApplied;
    private Components components;
    [SerializeField]
    private Vector2 currentAttackForce;
    [SerializeField]
    private float currentDamageAttack;
    [SerializeField]
    private AnimationClip currentClipAttack;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Update()
    {
        if (components.msng.IsAttacking && !damageApplied)
            AnyAttackBehaviour(currentAttackForce, currentDamageAttack, currentClipAttack.length, currentClipAttack.name);
    }
    public void AnyAttackAnimationHandler(bool endAttack, bool chainedAttack = false, bool isKick = false, int attack = -1)
    {
        if (endAttack)
        {
            components.msng.clear_attack = null;
            components.msng.chain_oportunity = null;
            damageApplied = false;
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
    private void AnyAttackBehaviour(Vector2 inertia, float damage, float timer, string attackStunCausant)
    {
        if (components.msng.enemy != null)
        {
            print($"Any Attack Behaviour; Current Clip: {currentClipAttack.name}");
            damageApplied = true;

            Components enemyComponents = components.msng.enemy.GetComponentInParent<Components>();

            enemyComponents.msng.HitStunCausant = attackStunCausant;
            enemyComponents.msng.HitStunTimer = timer + Timer;
            enemyComponents.msng.IsTakingDamage = true;

            enemyComponents.phys.AddForce(inertia);
        }
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
        // print("Chain Time" + components.anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
        components.msng.ChainOportunity = true;
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);
        // print("No Chain Time" + components.anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
        components.msng.ChainOportunity = false;
    }

    #region Kicks
    public void HardKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        bool chainOportunity = components.msng.KickChain[1] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(2, isKick: true);
            currentAttackForce = AttackForces[6];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[6];
        }
    }
    public void LowKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        if (canAttack)
        {
            AnyAttackLogic(0, isKick: true);
            currentAttackForce = AttackForces[4];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[4];
        }
    }
    public void MiddleKick()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        bool chainOportunity = components.msng.KickChain[0] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(1, isKick: true);
            currentAttackForce = AttackForces[5];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[5];
        }
    }
    public void SpecialKick()
    {
        bool canSpecial = components.msng.StartedWithFirst && components.msng.KickChain[2] &&
                            components.msng.ChainOportunity;

        if (canSpecial)
        {
            AnyAttackLogic(3, isKick: true);
            currentAttackForce = AttackForces[7];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[7];
        }
    }
    #endregion

    #region Punchs
    public void HardPunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        bool chainOportunity = components.msng.PunchChain[1] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(2);
            currentAttackForce = AttackForces[2];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[2];
        }
    }
    public void LowPunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        if (canAttack)
        {
            AnyAttackLogic(0);
            currentAttackForce = AttackForces[0];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[0];
        }
    }
    public void MiddlePunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking &&
                            !components.msng.IsTakingDamage;

        bool chainOportunity = components.msng.PunchChain[0] && components.msng.ChainOportunity;

        if (canAttack || chainOportunity)
        {
            AnyAttackLogic(1);
            currentAttackForce = AttackForces[1];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[1];
        }
    }
    public void SpecialPunch()
    {
        bool canSpecial = components.msng.StartedWithFirst && components.msng.PunchChain[2] &&
                            components.msng.ChainOportunity;

        if (canSpecial)
        {
            AnyAttackLogic(3);
            currentAttackForce = AttackForces[3];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[3];
        }
    }
    #endregion
}