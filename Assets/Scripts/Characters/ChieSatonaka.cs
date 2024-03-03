using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    protected override void Update()
    {
        base.Update();
        if (Components.Messenger.RequestedAttack != AnimationStates.Null)
        {
            StopWalk();
            if (attackC != null)
            {
                StopCoroutine(attackC);
                InterruptAttack();
            }
            switch (Components.Messenger.RequestedAttack)
            {
                case AnimationStates.LowPunch:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.LowPunch]));
                    break;
                case AnimationStates.MiddlePunch:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.MiddlePunch]));
                    break;
                case AnimationStates.HardPunch:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.HardPunch]));
                    break;
                case AnimationStates.SpecialPunch:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.SpecialPunch]));
                    break;
                case AnimationStates.LowKick:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.LowKick]));
                    break;
                case AnimationStates.MiddleKick:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.MiddleKick]));
                    break;
                case AnimationStates.HardKick:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.HardKick]));
                    break;
                case AnimationStates.SpecialKick:
                    attackC = StartCoroutine(Attack(attacks[AnimationStates.SpecialKick]));
                    break;
            }
        }

        if (Components.Messenger.Blocking)
        {
            if (currentCommand != actions[AnimationStates.Block])
                currentCommand = actions[AnimationStates.Block];
        }

        if (Components.Messenger.Crouching && !Components.Messenger.Hurt)
        {
            if (currentCommand != actions[AnimationStates.Crouch] && !Components.Messenger.Blocking)
            {
                Components.Machine.ChangeAnimation(actions[AnimationStates.Crouch].ActionStates[0]);
                actions[AnimationStates.Crouch].Execute(Components);
                currentCommand = actions[AnimationStates.Crouch];
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Components.Messenger.Walking == 0) { }
        else if (Mathf.Sign(Components.Messenger.Walking) > 0 && !(Components.Messenger.Jumping || Components.Messenger.Falling))
        {
            if (currentCommand != actions[AnimationStates.Walk])
            {
                Components.Machine.ChangeAnimation(actions[AnimationStates.Walk].ActionStates[0]);
                currentCommand = actions[AnimationStates.Walk];
            }
            actions[AnimationStates.Walk].Execute(Components);
        }
        else if (!(Components.Messenger.Jumping || Components.Messenger.Falling))
        {
            if (currentCommand != actions[AnimationStates.GoingBackwards])
            {
                Components.Machine.ChangeAnimation(actions[AnimationStates.GoingBackwards].ActionStates[0]);
                currentCommand = actions[AnimationStates.GoingBackwards];
            }
            actions[AnimationStates.GoingBackwards].Execute(Components);
        }


        if (Components.Messenger.Jumping)
        {
            if (currentCommand != actions[AnimationStates.Jump] && !Components.Messenger.Falling)
            {
                Components.Machine.ChangeAnimation(actions[AnimationStates.Jump].ActionStates[0]);
                actions[AnimationStates.Jump].Execute(Components);
                currentCommand = actions[AnimationStates.Jump];
            }
        }
    }

    protected override void InitActions()
    {
        actions = new Dictionary<AnimationStates, Action>
        {
            { AnimationStates.Walk, new Walk(10f, 500f, new List<AnimationStates>(){AnimationStates.StartWalking, AnimationStates.Walk}) },
            { AnimationStates.GoingBackwards, new Back(10f, 500f, new List<AnimationStates>(){AnimationStates.StartGoingBackwards, AnimationStates.GoingBackwards}) },
            { AnimationStates.Jump, new Jump(22.5f, new List<AnimationStates>(){AnimationStates.StartJumping, AnimationStates.Jump}) },
            { AnimationStates.Fall , new Fall(new List<AnimationStates>(){AnimationStates.StartFalling, AnimationStates.Fall}) },
            { AnimationStates.Block , new Block(new List<AnimationStates>(){AnimationStates.Block, AnimationStates.BlockWhileCrouch}) },
            { AnimationStates.Crouch , new Crouch(new List<AnimationStates>(){AnimationStates.StartCrouching,AnimationStates.Crouch}) },
        };
    }

    protected override void InitAttacks()
    {
        attacks = new Dictionary<AnimationStates, Attack>
        {
            {
                AnimationStates.LowPunch,
                new Attack
                (
                    hitF: false,
                    dmg: 3,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 45,
                    cd: 0.1f,
                    hitFT:0.25f,
                    inertia: new Vector2(-1.5f, 0),
                    force: new Vector2(1.5f, 0),
                    actionStates: new List<AnimationStates>(){AnimationStates.LowPunch,AnimationStates.ChainLowPunch}
                )
            },
            {
                AnimationStates.MiddlePunch,
                new Attack
                (
                    hitF: true,
                    dmg: 5,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 65,
                    cd: 0.2f,
                    hitFT:0.25f,
                    inertia: new Vector2(0.5f,0),
                    force: new Vector2(0,12),
                    actionStates: new List<AnimationStates>(){AnimationStates.MiddlePunch,AnimationStates.ChainMiddlePunch}
                )
            },
            {
                AnimationStates.HardPunch,
                new Attack
                (
                    hitF: true,
                    dmg: 7,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 71,
                    cd: 0.4f,
                    hitFT:0.25f,
                    inertia: new Vector2(1,4),
                    force: new Vector2(10,0),
                    actionStates: new List<AnimationStates>(){AnimationStates.HardPunch,AnimationStates.ChainHardPunch}
                )
            },
            {
                AnimationStates.SpecialPunch,
                new Attack
                (
                    hitF: true,
                    dmg: 10,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 70,
                    cd: 0.6f,
                    hitFT:0.25f,
                    inertia: new Vector2(6,13.3f),
                    force: new Vector2(1.5f,15),
                    actionStates: new List<AnimationStates>(){AnimationStates.SpecialPunch,AnimationStates.ChainSpecialPunch}
                )
            },
            {
                AnimationStates.LowKick,
                new Attack
                (
                    hitF: false,
                    dmg: 4,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 41,
                    cd: 0.15f,
                    hitFT:0.25f,
                    inertia: new Vector2(-0.5f,7.5f),
                    force: new Vector2(3.5f,0),
                    actionStates: new List<AnimationStates>(){AnimationStates.LowKick,AnimationStates.ChainLowKick}
                )
            },
            {
                AnimationStates.MiddleKick,
                new Attack
                (
                    hitF: true,
                    dmg: 6,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 70,
                    cd: 0.25f,
                    hitFT:0.25f,
                    inertia: new Vector2(0,8),
                    force: new Vector2(0,10),
                    actionStates: new List<AnimationStates>(){AnimationStates.MiddleKick,AnimationStates.ChainMiddleKick}
                )
            },
            {
                AnimationStates.HardKick,
                new Attack
                (
                    hitF: true,
                    dmg: 8,
                    timeDmg: 1,
                    resGained: 2,
                    hitS: 65,
                    cd: 0.45f,
                    hitFT:0.25f,
                    inertia: new Vector2(1.9f,10),
                    force: new Vector2(5,0),
                    actionStates: new List<AnimationStates>(){AnimationStates.HardKick,AnimationStates.ChainHardKick}
                )
            },
            {
                AnimationStates.SpecialKick,
                new Attack
                (
                    hitF: true,
                    dmg: 11,
                    timeDmg: 2,
                    resGained: 2,
                    hitS: 100,
                    cd: 0.8f,
                    hitFT:0.25f,
                    inertia: new Vector2(-0.5f,5),
                    force: new Vector2(0,10),
                    actionStates: new List<AnimationStates>(){AnimationStates.SpecialKick,AnimationStates.ChainSpecialKick}
                )
            },
        };
    }
}
