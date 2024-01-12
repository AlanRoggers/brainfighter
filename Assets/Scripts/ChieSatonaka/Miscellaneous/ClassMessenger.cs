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
    public bool IsAttacking; //Restricción de acciones motion y acciones damage
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
    public Collider2D enemy;
    public Coroutine clear_attack = null;
    public Coroutine chain_oportunity = null;
    public Coroutine cooldown_timmer = null;
    [SerializeField]
    private float coolDown;
    void Start()
    {
        IsJumping = false;
        AttackRestricted = false;
    }
    public IEnumerator COOLDOWN_TIMER()
    {
        AttackRestricted = true;
        yield return new WaitForSeconds(coolDown);
        AttackRestricted = false;
    }
}
