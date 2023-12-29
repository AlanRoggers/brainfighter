using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    public bool[] PunchChain = new bool[3]; //Orden de los golpes
    public bool[] KickChain = new bool[3];
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
    void Start()
    {
        IsJumping = false;
        AttackRestricted = false;
    }
}
