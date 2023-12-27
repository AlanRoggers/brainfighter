using UnityEngine;

public class GameplayInput : MonoBehaviour
{
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Update()
    {
        WalkInput();
        JumpInput();
        AttackTest();
        Crouch();
    }
    private void WalkInput()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        int direction = !right && left ? -1 : right && !left ? 1 : 0;
        components.motion.Walk(direction);
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            components.motion.Jump();
    }
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.S))
            components.motion.Crouch();
        if (Input.GetKeyUp(KeyCode.S))
            components.motion.StandUp();
    }
    private void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.F))
            components.damage.LowPunch();
    }
}
