using UnityEngine;

public class ClassMessenger : MonoBehaviour
{
    public bool canCombo;
    public bool isCrouching;
    public bool isJumping;
    public bool isKicking;
    public bool isWalking;
    public bool onGround;
    public bool[] attackChain = new bool[3];

    void Start()
    {
        isJumping = false;
    }
}
