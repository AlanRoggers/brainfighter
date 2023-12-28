using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    public bool[] attackChain = new bool[3];
    public bool canCombo;
    public bool isCrouching;
    public bool isDashing;
    public bool isJumping;
    public bool isKicking;
    public bool isRunning;
    public bool isWalking;
    public bool onGround;

    void Start()
    {
        isJumping = false;
    }
}
