using System.Collections;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField]
    private bool canDash;
    private Coroutine dash_chance;
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
    }

    private void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.F))
            components.attacks.LowPunch();
    }
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
            else components.msng.isRunning = false;
        }
        else components.msng.isRunning = false;
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
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!canDash && !components.msng.isDashing)
                {
                    canDash = true;
                    dash_chance = StartCoroutine(DASH_CAHNCE());
                }
                else
                {
                    print("Dash Positivo");
                    components.motion.Dash(false);
                    StopCoroutine(dash_chance);
                    canDash = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!canDash && !components.msng.isDashing)
                {
                    canDash = true;
                    dash_chance = StartCoroutine(DASH_CAHNCE());
                }
                else
                {
                    print("Dash Negativo");
                    components.motion.Dash(true);
                    StopCoroutine(dash_chance);
                    canDash = false;
                }
            }
        }
    }
    private IEnumerator DASH_CAHNCE()
    {
        yield return new WaitForSeconds(0.25f);
        canDash = false;
    }
}
