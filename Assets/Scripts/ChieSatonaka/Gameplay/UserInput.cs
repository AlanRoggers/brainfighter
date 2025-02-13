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
        // AttackTest();
        CrouchInput();
        RunInput();
        DashInput();
        DashBackInput();
        LowPunchInput();
        MiddlePunchInput();
        HardPunchInput();
        SpecialPunchInput();
        LowKickInput();
        MiddleKickInput();
        HardKickInput();
        SpecialKickInput();
        CrouchKickInput();
        BlockInput();
        // KeyChecker();
    }

    #region DamageInputs
    private void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.O) && !Input.GetKey(KeyCode.LeftAlt))
            components.attacks.SpecialPunch();
    }
    private void HardPunchInput()
    {
        if (gameObject.layer == 6)
        {
            if (Input.GetKeyDown(KeyCode.L) && !Input.GetKey(KeyCode.LeftAlt))
            {
                components.attacks.HardPunch();
            }
        }
        else if (gameObject.layer == 7)
        {
            if (Input.GetKeyDown(KeyCode.P) && !Input.GetKey(KeyCode.LeftAlt))
            {
                components.attacks.HardPunch();
            }
        }
    }
    private void LowPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.J) && !Input.GetKey(KeyCode.LeftAlt))
            components.attacks.LowPunch();
    }
    private void MiddlePunchInput()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Input.GetKey(KeyCode.LeftAlt))
            components.attacks.MiddlePunch();
    }
    private void SpecialPunchInput()
    {
        if (Input.GetKeyDown(KeyCode.K) && !Input.GetKey(KeyCode.LeftAlt))
            components.attacks.SpecialPunch();
    }
    private void HardKickInput()
    {
        if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftAlt))
            components.attacks.HardKick();
    }
    private void LowKickInput()
    {
        if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.LeftAlt))
            components.attacks.LowKick();
    }
    private void MiddleKickInput()
    {
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftAlt))
            components.attacks.MiddleKick();
    }
    private void SpecialKickInput()
    {
        if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftAlt))
            components.attacks.SpecialKick();
    }
    private void CrouchKickInput()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.J))
        {
            print("Hola");
            components.attacks.CrouchKick();
        }
    }
    private void KeyChecker()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
            print("Presionada la tecla alt");
    }
    #endregion

    #region MotionInputs
    private void BlockInput()
    {
        if (Input.GetKey(KeyCode.F))
            components.motion.Block();
        else if (Input.GetKeyUp(KeyCode.F))
            components.motion.StopBlock();
    }
    private void CrouchInput()
    {
        if (Input.GetKey(KeyCode.S))
            components.motion.Crouch();
        if (Input.GetKeyUp(KeyCode.S))
            components.motion.StandUp();
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
                components.motion.Dash();
        }
    }
    private void DashBackInput()
    {
        if (transform.localScale.x > 0)
        {
            if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D))
                components.motion.DashBack();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A))
                components.motion.DashBack(true);
        }
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
        else
        {
            components.msng.IsRunning = false;
        }

    }
    private void WalkInput()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        int direction = !right && left ? -1 : right && !left ? 1 : 0;
        components.motion.Walk(direction);
    }
    #endregion

}
