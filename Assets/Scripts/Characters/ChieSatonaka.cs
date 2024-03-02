using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    protected override void Update()
    {
        base.Update();
        if (components.Messenger.RequestedAttack != AnimationStates.Null)
        {
            StopWalk();
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                InterruptCoroutine();
            }
            switch (components.Messenger.RequestedAttack)
            {
                case AnimationStates.LowPunch:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.LowPunch]));
                    break;
                case AnimationStates.MiddlePunch:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.MiddlePunch]));
                    break;
                case AnimationStates.HardPunch:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.HardPunch]));
                    break;
                case AnimationStates.SpecialPunch:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.SpecialPunch]));
                    break;
                case AnimationStates.LowKick:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.LowKick]));
                    break;
                case AnimationStates.MiddleKick:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.MiddleKick]));
                    break;
                case AnimationStates.HardKick:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.HardKick]));
                    break;
                case AnimationStates.SpecialKick:
                    attackCoroutine = StartCoroutine(Attack(attacks[AnimationStates.SpecialKick]));
                    break;
            }
        }

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (components.Messenger.Walking == 0) { }
        else if (Mathf.Sign(components.Messenger.Walking) > 0 && !(components.Messenger.Jumping || components.Messenger.Falling))
        {
            if (currentCommand != actions[AnimationStates.Walk])
                components.Machine.ChangeAnimation(actions[AnimationStates.Walk].ActionStates[0]);
            actions[AnimationStates.Walk].Execute(components);
            currentCommand = actions[AnimationStates.Walk];
        }
        else if (!(components.Messenger.Jumping || components.Messenger.Falling))
        {
            if (currentCommand != actions[AnimationStates.GoingBackwards])
                components.Machine.ChangeAnimation(actions[AnimationStates.GoingBackwards].ActionStates[0]);
            actions[AnimationStates.GoingBackwards].Execute(components);
            currentCommand = actions[AnimationStates.GoingBackwards];
        }


        if (components.Messenger.Jumping)
        {
            if (currentCommand != actions[AnimationStates.Jump] && !components.Messenger.Falling)
            {
                components.Machine.ChangeAnimation(actions[AnimationStates.Jump].ActionStates[0]);
                actions[AnimationStates.Jump].Execute(components);
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
                    hitS: 0.2f,
                    cd: 0.1f,
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
                    hitS: 0.3f,
                    cd: 0.4f,
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
                    hitS: 0.5f,
                    cd: 0.8f,
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
                    hitS: 1f,
                    cd: 1f,
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
                    hitS: 0.35f,
                    cd: 0.3f,
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
                    hitS: 0.3f,
                    cd: 0.55f,
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
                    hitS: 0.5f,
                    cd: 0.85f,
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
                    hitS: 1f,
                    cd: 1f,
                    inertia: new Vector2(-0.5f,5),
                    force: new Vector2(0,10),
                    actionStates: new List<AnimationStates>(){AnimationStates.SpecialKick,AnimationStates.ChainSpecialKick}
                )
            },
        };
    }
}
