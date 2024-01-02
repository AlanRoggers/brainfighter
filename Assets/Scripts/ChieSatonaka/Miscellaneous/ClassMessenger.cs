using System.Collections;
using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    [SerializeField]
    private float coolDown;
    public bool[] PunchChain = new bool[4]; //Orden de los golpes
    public bool[] KickChain = new bool[4];
    public bool AttackRestricted; //Restriccion de ataque por tiempo de recuperación
    public bool ChainOportunity; //Oportunidad de encadenar un siguiente golpe y omitir la restricción de ataque
    public bool dashTimer;
    public bool dashBackTimer;
    public bool IsAttacking; //Restricción de acciones motion y acciones damage
    public bool IsCrouching;
    public bool IsDashing;
    public bool IsDashingBack;
    public bool IsJumping;
    public bool IsOnGround;
    public bool IsRunning;
    public bool IsWalking;
    public Coroutine clear_attack = null;
    public Coroutine chain_oportunity = null;
    public Coroutine cooldown_timmer = null;
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
