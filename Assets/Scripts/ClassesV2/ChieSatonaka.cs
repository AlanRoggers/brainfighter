using UnityEngine;

public class ChieSatonaka : Character
{
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) { }
        else if (Input.GetKey(KeyCode.A))
            Walk(-1, 12);
        else if (Input.GetKey(KeyCode.D))
            Walk(1, 12);

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            StopWalk();
    }
    protected override void AssignAttacks()
    {
        attacks.Add
        (
            new Attack(3, new Vector2(1.5f, 0), Vector2.zero, 0.3f, 0.4f, false, AnimationStates.LowPunch)
        );
        attacks.Add
        (
            new Attack(5, new Vector2(0, 12), Vector2.zero, 0.4f, 0.5f, true, AnimationStates.MiddlePunch)
        );
        attacks.Add
        (
            new Attack(7, new Vector2(10, 0), new Vector2(160, 270), 0.6f, 0.7f, true, AnimationStates.HardPunch)
        );
        attacks.Add
        (
            new Attack(10, new Vector2(1.5f, 15), new Vector2(200, 700), 0.8f, 0.9f, true, AnimationStates.SpecialPunch)
        );
        attacks.Add
        (
            new Attack(3, new Vector2(3.5f, 0), new Vector2(0, 250), 0.3f, 0.4f, true, AnimationStates.LowKick)
        );
        attacks.Add
        (
            new Attack(5, new Vector2(0, 10), new Vector2(150, 0), 0.4f, 0.5f, true, AnimationStates.MiddleKick)
        );
        attacks.Add
        (
            new Attack(7, new Vector2(5, 0), new Vector2(0, 500), 0.6f, 0.7f, true, AnimationStates.HardKick)
        );
        attacks.Add
        (
            new Attack(10, new Vector2(0, 10), new Vector2(250, 0), 0.8f, 0.9f, true, AnimationStates.SpecialKick)
        );
    }
}
