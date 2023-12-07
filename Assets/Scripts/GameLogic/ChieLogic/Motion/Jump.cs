using UnityEngine;

public class Jump : MonoBehaviour
{
    private int jumpForce;
    private float lastJumpTime;
    private float timeFixJump;
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        lastJumpTime = 0f;
        timeFixJump = 0.5f;
        jumpForce = 800;
        components.isJumping = false;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            JumpLogic();
    }
    public void JumpLogic()
    {
        bool canJump = components.onGround && Time.time - lastJumpTime >= timeFixJump &&
                        !components.isKicking;
        if (canJump)
        {
            components.phys.AddForce(new Vector2(0, jumpForce));
            lastJumpTime = Time.time;
            components.isJumping = true;
            components.coll.CanCheckGround = false;
        }
    }
}