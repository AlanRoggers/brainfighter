using System.Collections;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    [SerializeField] private float hitstopTimer;
    public float Timer;
    public AnimationClip[] attackClips = new AnimationClip[8];
    public Vector2[] AttackForces = new Vector2[8];
    public Vector2[] AttacksInertia = new Vector2[8];
    private Rigidbody2D enemyPhys = null;

    private float currentDamageAttack;
    private AnimationClip currentClipAttack;
    private Components components;
    private Vector2 currentAttackForce;

    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Update()
    {
        if (components.msng.IsAttacking && !components.msng.DamageApplied)
            AnyAttackDamage();
    }
    void FixedUpdate()
    {
        // Fisicas aplicadas al enemigo
        if (enemyPhys != null)
        {
            enemyPhys.velocity = Vector2.zero;
            enemyPhys.AddForce(currentAttackForce, ForceMode2D.Impulse);
            enemyPhys = null;
        }
    }
    private void AnyAttackLogic(int attack, Vector2 inertia, bool isKick = false)
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
        components.msng.DamageHitbox.enabled = true;

        // Movimiento del personaje
        components.phys.AddForce(inertia);
    }
    private void AnyAttackDamage()
    {
        if (components.msng.enemy != null)
        {
            print($"Any Attack Behaviour; Current Clip: {currentClipAttack.name} Fuerza que se aplicara: {currentAttackForce}");
            components.msng.DamageApplied = true;

            Components enemyComponents = components.msng.enemy.GetComponentInParent<Components>();

            float damage = currentDamageAttack; //Esta parte aún no se implementa

            enemyComponents.msng.HitStunCausant = currentClipAttack.name;
            enemyComponents.msng.HitStunTimer = currentClipAttack.length + Timer;
            enemyComponents.msng.IsTakingDamage = true;
            StartCoroutine(HITSTOP());
            enemyPhys = enemyComponents.phys;
        }
    }

    private IEnumerator HITSTOP()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitstopTimer);
        Time.timeScale = 1;
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
            AnyAttackLogic(2, AttacksInertia[6], isKick: true);
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
            AnyAttackLogic(0, AttacksInertia[4], isKick: true);
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
            AnyAttackLogic(1, AttacksInertia[5], isKick: true);
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
            AnyAttackLogic(3, AttacksInertia[7], isKick: true);
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
            AnyAttackLogic(2, AttacksInertia[2]);

            // Parametros para la interacción entre el personaje y el enemigo cuando se causa daño
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
            AnyAttackLogic(0, AttacksInertia[0]);
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
            AnyAttackLogic(1, AttacksInertia[1]);
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
            AnyAttackLogic(3, AttacksInertia[3]);
            currentAttackForce = AttackForces[3];
            currentDamageAttack = 10f;
            currentClipAttack = attackClips[3];
        }
    }
    #endregion
}