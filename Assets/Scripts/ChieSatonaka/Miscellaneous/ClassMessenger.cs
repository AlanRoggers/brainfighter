using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    public bool[] AttackChain = new bool[3];
    public bool dashTimer;
    public bool dashBackTimer;
    public bool IsAttacking;
    public bool IsCrouching;
    public bool IsDashing;
    public bool IsDashingBack;
    public bool IsJumping;
    public bool IsKicking;
    public bool IsOnGround;
    public bool IsRunning;
    public bool IsWalking;
    void Start()
    {
        IsJumping = false;
    }
}
