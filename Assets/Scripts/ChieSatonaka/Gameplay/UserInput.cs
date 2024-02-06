using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField] private int player;
    private Dictionary<string, KeyCode> onePlayer = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> twoPlayer = new Dictionary<string, KeyCode>();
    private Components components;
    void Start()
    {
        InitDictionarites();
    }
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

    void InitDictionarites()
    {
        onePlayer.Add("Block", KeyCode.E);
        onePlayer.Add("Crouch", KeyCode.S);
        onePlayer.Add("Right", KeyCode.D);
        onePlayer.Add("Left", KeyCode.A);
        onePlayer.Add("Jump", KeyCode.W);
        onePlayer.Add("Run", KeyCode.LeftControl);
        onePlayer.Add("HardPunch", KeyCode.Y);
        onePlayer.Add("MiddlePunch", KeyCode.T);
        onePlayer.Add("LowPunch", KeyCode.R);
        onePlayer.Add("HardKick", KeyCode.H);
        onePlayer.Add("MiddleKick", KeyCode.G);
        onePlayer.Add("LowKick", KeyCode.F);
        onePlayer.Add("SpecialPunch", KeyCode.R);
        onePlayer.Add("SpecialKick", KeyCode.F);
        twoPlayer.Add("Block", KeyCode.P);
        twoPlayer.Add("Crouch", KeyCode.L);
        twoPlayer.Add("Right", KeyCode.Semicolon);
        twoPlayer.Add("Left", KeyCode.K);
        twoPlayer.Add("Jump", KeyCode.O);
        twoPlayer.Add("Run", KeyCode.AltGr);
        twoPlayer.Add("HardPunch", KeyCode.Keypad9);
        twoPlayer.Add("MiddlePunch", KeyCode.Keypad8);
        twoPlayer.Add("LowPunch", KeyCode.Keypad7);
        twoPlayer.Add("HardKick", KeyCode.Keypad6);
        twoPlayer.Add("MiddleKick", KeyCode.Keypad5);
        twoPlayer.Add("LowKick", KeyCode.Keypad4);
        twoPlayer.Add("SpecialPunch", KeyCode.Keypad7);
        twoPlayer.Add("SpecialKick", KeyCode.Keypad4);

    }

    #region DamageInputs
    private void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.O) && !Input.GetKey(KeyCode.LeftAlt))
            components.attacks.SpecialPunch();
    }
    private void HardPunchInput()
    {
        KeyCode input = player == 1 ? onePlayer["HardPunch"] : twoPlayer["HardPunch"];
        if (Input.GetKeyDown(input))
            components.attacks.HardPunch();
    }
    private void LowPunchInput()
    {
        KeyCode input = player == 1 ? onePlayer["LowPunch"] : twoPlayer["LowPunch"];
        if (Input.GetKeyDown(input))
            components.attacks.LowPunch();
    }
    private void MiddlePunchInput()
    {
        KeyCode input = player == 1 ? onePlayer["MiddlePunch"] : twoPlayer["MiddlePunch"];
        if (Input.GetKeyDown(input))
            components.attacks.MiddlePunch();
    }
    private void SpecialPunchInput()
    {
        KeyCode input = player == 1 ? onePlayer["SpecialPunch"] : twoPlayer["SpecialPunch"];
        if (Input.GetKeyDown(input))
            components.attacks.SpecialPunch();
    }
    private void HardKickInput()
    {
        KeyCode input = player == 1 ? onePlayer["HardKick"] : twoPlayer["HardKick"];
        if (Input.GetKeyDown(input))
            components.attacks.HardKick();
    }
    private void LowKickInput()
    {
        KeyCode input = player == 1 ? onePlayer["LowKick"] : twoPlayer["LowKick"];
        if (Input.GetKeyDown(input))
            components.attacks.LowKick();
    }
    private void MiddleKickInput()
    {
        KeyCode input = player == 1 ? onePlayer["MiddleKick"] : twoPlayer["MiddleKick"];
        if (Input.GetKeyDown(input))
            components.attacks.MiddleKick();
    }
    private void SpecialKickInput()
    {
        KeyCode input = player == 1 ? onePlayer["SpecialKick"] : twoPlayer["SpecialKick"];
        if (Input.GetKeyDown(input))
            components.attacks.SpecialKick();
    }
    private void CrouchKickInput()
    {
        KeyCode input1 = player == 1 ? onePlayer["LowKick"] : twoPlayer["LowKick"];
        KeyCode input2 = player == 1 ? onePlayer["Crouch"] : twoPlayer["Crouch"];
        if (Input.GetKey(input2) && Input.GetKeyDown(input1))
            components.attacks.CrouchKick();
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
        KeyCode input = player == 1 ? onePlayer["Block"] : twoPlayer["Block"];
        if (Input.GetKey(input))
            components.motion.Block();
        else if (Input.GetKeyUp(input))
            components.motion.StopBlock();
    }
    private void CrouchInput()
    {
        KeyCode input = player == 1 ? onePlayer["Crouch"] : twoPlayer["Crouch"];
        if (Input.GetKey(input))
            components.motion.Crouch();
        if (Input.GetKeyUp(input))
            components.motion.StandUp();
    }
    private void DashInput()
    {
        KeyCode input1 = player == 1 ? onePlayer["Left"] : twoPlayer["Left"];
        KeyCode input2 = player == 1 ? onePlayer["Right"] : twoPlayer["Right"];
        if (transform.localScale.x > 0)
        {
            if (Input.GetKeyDown(input2) && !Input.GetKey(input1))
                components.motion.Dash(false);
        }
        else
        {
            if (Input.GetKeyDown(input1) && !Input.GetKey(input2))
                components.motion.Dash();
        }
    }
    private void DashBackInput()
    {
        KeyCode input1 = player == 1 ? onePlayer["Left"] : twoPlayer["Left"];
        KeyCode input2 = player == 1 ? onePlayer["Right"] : twoPlayer["Right"];
        if (transform.localScale.x > 0)
        {
            if (Input.GetKeyDown(input1) && !Input.GetKey(input2))
                components.motion.DashBack();
        }
        else
        {
            if (Input.GetKeyDown(input2) && !Input.GetKey(input1))
                components.motion.DashBack(true);
        }
    }
    private void JumpInput()
    {
        KeyCode input = player == 1 ? onePlayer["Jump"] : twoPlayer["Jump"];
        if (Input.GetKeyDown(input))
            components.motion.Jump();
    }
    private void RunInput()
    {
        KeyCode input1 = player == 1 ? onePlayer["Run"] : twoPlayer["Run"];
        KeyCode input2 = player == 1 ? onePlayer["Left"] : twoPlayer["Left"];
        KeyCode input3 = player == 1 ? onePlayer["Right"] : twoPlayer["Right"];
        if (Input.GetKey(input1))
        {
            if (Input.GetKey(input3) && transform.localScale.x > 0)
                components.motion.Run();
            else if (Input.GetKey(input2) && transform.localScale.x < 0)
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
        KeyCode input1 = player == 1 ? onePlayer["Right"] : twoPlayer["Right"];
        KeyCode input2 = player == 1 ? onePlayer["Left"] : twoPlayer["Left"];
        bool right = Input.GetKey(input1);
        bool left = Input.GetKey(input2);
        int direction = !right && left ? -1 : right && !left ? 1 : 0;
        components.motion.Walk(direction);
    }
    #endregion
}
