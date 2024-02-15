using System.Collections;
using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    public string HitStunCausant;
    public float HitStunTimer;
    public bool[] PunchChain = new bool[4]; //Orden de los golpes
    public bool[] KickChain = new bool[4];
    public bool AttackRestricted; //Restriccion de ataque por tiempo de recuperación
    public bool ChainOportunity; //Oportunidad de encadenar un siguiente golpe y omitir la restricción de ataque
    public bool DamageApplied;
    public bool DashTimer;
    public bool DashBackTimer;
    public bool Dead;
    public bool IsAttacking; //Restricción de acciones motion y acciones damage
    public bool IsBlocking;
    public bool IsCrouching;
    public bool IsDashing;
    public bool IsDashingBack;
    public bool IsJumping;
    public bool IsOnGround;
    public bool IsRunning;
    public bool IsTakingDamage;
    public bool IsTurning;
    public bool IsWalking;
    public bool NeedTurn;
    public bool StartedWithFirst;
    public Collider2D DamageHitbox;
    public GameObject Enemy;
    public Collider2D EnemyCollider;
    public Coroutine cooldown_timmer = null;
    [SerializeField] private float coolDown;
    void Start()
    {
        StartValues();
    }
    public void StartValues()
    {
        HitStunCausant = "";
        HitStunTimer = 0f;
        for (int i = 0; i < PunchChain.Length; i++)
            PunchChain[i] = false;
        for (int i = 0; i < KickChain.Length; i++)
            KickChain[i] = false;
        AttackRestricted = false;
        ChainOportunity = false;
        DamageApplied = false;
        DashTimer = false;
        DashBackTimer = false;
        Dead = false;
        IsAttacking = false;
        IsBlocking = false;
        IsCrouching = false;
        IsDashing = false;
        IsDashingBack = false;
        IsJumping = false;
        IsOnGround = false;
        IsRunning = false;
        IsTakingDamage = false;
        IsTurning = false;
        IsWalking = false;
        NeedTurn = false;
        StartedWithFirst = false;
        AttackRestricted = false;
        cooldown_timmer = null;
    }
    public IEnumerator COOLDOWN_TIMER()
    {
        AttackRestricted = true;
        yield return new WaitForSeconds(coolDown);
        AttackRestricted = false;
    }
}
