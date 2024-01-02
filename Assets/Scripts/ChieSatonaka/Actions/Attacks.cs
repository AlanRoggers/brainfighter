using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    [SerializeField]
    private float coolDown;
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    private void AnyAttackLogic(int attack)
    {
        components.msng.PunchChain[attack] = true;
        StartCoroutine(Clear_Attack(attack)); // Esto puede ser que no vaya aquí
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
    public IEnumerator COOLDOWN_TIMER()
    {
        yield return new WaitForSeconds(coolDown);
        components.msng.AttackRestricted = false;
    }
    public IEnumerator CHAIN_OPORTUNITY()
    {
        yield return new WaitUntil(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.45f);
        components.msng.ChainOportunity = true;
        yield return new WaitWhile(() => components.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.1f);
        components.msng.ChainOportunity = false;
    }
    private bool Attacking()
    {
        foreach (bool attackSpace in components.msng.PunchChain)
        {
            if (attackSpace) return true;
        }
        return false;
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
        }
    }
    public void HardPunch()
    {
        bool canAttack = components.msng.IsOnGround && !components.msng.IsCrouching &&
                            !components.msng.IsDashing && !components.msng.IsDashingBack &&
                            !components.msng.AttackRestricted && !components.msng.IsAttacking;

        bool chainOportunity = components.msng.PunchChain[1] && components.msng.ChainOportunity;
        AnyAttackLogic(2);
        // Fisicas del tercer golpe
    }
    #endregion
}