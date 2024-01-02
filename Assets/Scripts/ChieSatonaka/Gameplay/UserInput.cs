using UnityEngine;

public class UserInput : MonoBehaviour
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
        CrouchInput();
        RunInput();
        DashInput();
        DashBackInput();
        LowPunchInput();
        MiddlePunchInput();
        HardPunchInput();
    }

    #region MotionInputs
    private void CrouchInput()
    {
        if (Input.GetKey(KeyCode.S))
            components.motion.Crouch();
        if (Input.GetKeyUp(KeyCode.S))
            components.motion.StandUp();
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            components.motion.Jump();
    }
    private void RunInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.D) && transform.localScale.x > 0)
                components.motion.Run();
            else if (Input.GetKey(KeyCode.A) && transform.localScale.x < 0)
                components.motion.Run();
            else components.msng.IsRunning = false;
        }
        else components.msng.IsRunning = false;
    }
    private void WalkInput()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        int direction = !right && left ? -1 : right && !left ? 1 : 0;
        components.motion.Walk(direction);
    }
    private void DashInput()
    {
        if (transform.localScale.x > 0)
        {
            if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A))
                components.motion.Dash(false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D))
                components.motion.Dash(true);
        }
    }
    private void DashBackInput()
    {
        if (transform.localScale.x > 0)
        {
            if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D))
                components.motion.DashBack(false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A))
                components.motion.Dash(true);
        }
    }
    #endregion

    #region DamageInputs
    private void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.O))
            components.attacks.SpecialPunch();
    }

    private void LowPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
            components.attacks.LowPunch();
    }

    private void MiddlePunchInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
            components.attacks.MiddlePunch();
    }

    private void HardPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            components.attacks.HardPunch();
        }
    }
    #endregion
}
