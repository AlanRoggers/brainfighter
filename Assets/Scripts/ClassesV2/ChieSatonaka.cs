using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    public CircleCollider2D hitbox;
    protected override void Awake()
    {
        base.Awake();
        AssignAttacks();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            StopWalk();
        else if (Input.GetKey(KeyCode.A))
            Walk(-1, 12);
        else if (Input.GetKey(KeyCode.D))
            Walk(1, 12);

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            StopWalk();

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (attacks[0].CanExecuteAttack())
            {
                StartCoroutine(attacks[0].ExecuteAttack(animationMachine, msng, phys, gameObject.layer == 6 ? 7 : 6));
            }
        }
        if (Input.GetKey(KeyCode.Space) && msng.InGround)
            Jump(22.5f, 10);
    }
    protected override void AssignAttacks()
    {
        attacks = new List<Attack>
        {
            new(3, new Vector2(1.5f, 0), Vector2.zero, 0.3f, 0.4f, false, AnimationStates.LowPunch, AnimationStates.ChainLowPunch, hitbox),
            new(5, new Vector2(0, 12), Vector2.zero, 0.4f, 0.5f, true, AnimationStates.MiddlePunch, AnimationStates.ChainMiddlePunch, hitbox),
            new(7, new Vector2(10, 0), new Vector2(160, 270), 0.6f, 0.7f, true, AnimationStates.HardPunch, AnimationStates.ChainHardPunch, hitbox),
            new(10, new Vector2(1.5f, 15), new Vector2(200, 700), 0.8f, 0.9f, true, AnimationStates.SpecialPunch, AnimationStates.ChainSpecialPunch, hitbox),
            new(3, new Vector2(3.5f, 0), new Vector2(0, 250), 0.3f, 0.4f, true, AnimationStates.LowKick, AnimationStates.ChainLowKick, hitbox),
            new(5, new Vector2(0, 10), new Vector2(150, 0), 0.4f, 0.5f, true, AnimationStates.MiddleKick, AnimationStates.ChainMiddleKick, hitbox),
            new(7, new Vector2(5, 0), new Vector2(0, 500), 0.6f, 0.7f, true, AnimationStates.HardKick, AnimationStates.ChainHardKick, hitbox),
            new(10, new Vector2(0, 10), new Vector2(250, 0), 0.8f, 0.9f, true, AnimationStates.SpecialKick, AnimationStates.ChainSpecialKick, hitbox)
        };
    }
}
