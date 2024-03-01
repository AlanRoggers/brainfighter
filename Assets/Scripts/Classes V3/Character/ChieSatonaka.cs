using System.Collections.Generic;
using UnityEngine;

public class ChieSatonaka : Character
{
    public Vector2 forceHelper;
    public Vector2 inertiaHelper;
    private void Update()
    {
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
                    attackCoroutine = StartCoroutine(Attack(attacksv4[AnimationStates.LowPunch]));
                    break;
                case AnimationStates.MiddlePunch:
                    attackCoroutine = StartCoroutine(Attack(attacksv4[AnimationStates.MiddlePunch]));
                    break;
                case AnimationStates.HardPunch:
                    attackCoroutine = StartCoroutine(Attack(attacksv4[AnimationStates.HardPunch]));
                    break;
                case AnimationStates.SpecialPunch:
                    attackCoroutine = StartCoroutine(Attack(attacksv4[AnimationStates.SpecialPunch]));
                    break;
            }
        }

    }
    private void FixedUpdate()
    {
        if (components.Messenger.Walking == 0)
            StopWalk();
        else if (components.Messenger.Walking > 0)
        {
            if (currentCommand != actions[AnimationStates.Walk])
                components.Machine.ChangeAnimation(actions[AnimationStates.Walk].ActionStates[0]);

            actions[AnimationStates.Walk].Execute(components);
            currentCommand = actions[AnimationStates.Walk];
        }
        else
        {
            if (currentCommand != actions[AnimationStates.GoingBackwards])
                components.Machine.ChangeAnimation(actions[AnimationStates.GoingBackwards].ActionStates[0]);
            actions[AnimationStates.GoingBackwards].Execute(components);
            currentCommand = actions[AnimationStates.GoingBackwards];
        }
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();

        bool iddle = !components.Messenger.Attacking && !components.Messenger.Hurt && components.Messenger.Walking == 0;

        if (iddle)
        {
            components.Machine.ChangeAnimation(AnimationStates.Iddle);
            currentCommand = null;
        }

    }
    protected override void InitActions()
    {
        actions = new Dictionary<AnimationStates, ActionV4>
        {
            { AnimationStates.Walk, new Walk(10f, 500f, new List<AnimationStates>(){AnimationStates.StartWalking, AnimationStates.Walk}) },
            { AnimationStates.GoingBackwards, new Back(10f, 500f, new List<AnimationStates>(){AnimationStates.StartGoingBackwards, AnimationStates.GoingBackwards}) },
        };
    }
    protected override void InitAttacks()
    {
        attacksv4 = new Dictionary<AnimationStates, AttackV4>
        {
            {
                AnimationStates.LowPunch,
                new AttackV4
                (
                    hitF: false,
                    dmg: 3,
                    timeDmg: 1,
                    hitS: 0.2f,
                    cd: 0.1f,
                    inertia: Vector2.zero,
                    force: new Vector2(1.5f, 0),
                    actionStates: new List<AnimationStates>(){AnimationStates.LowPunch,AnimationStates.ChainLowPunch}
                )
            },
            {
                AnimationStates.MiddlePunch,
                new AttackV4
                (
                    hitF: true,
                    dmg: 5,
                    timeDmg: 1,
                    hitS: 0.3f,
                    cd: 0.4f,
                    inertia: Vector2.zero,
                    force: new Vector2(0,12),
                    actionStates: new List<AnimationStates>(){AnimationStates.MiddlePunch,AnimationStates.ChainMiddlePunch}
                )
            },
            {
                AnimationStates.HardPunch,
                new AttackV4
                (
                    hitF: true,
                    dmg: 7,
                    timeDmg: 1,
                    hitS: 0.5f,
                    cd: 0.8f,
                    inertia: new Vector2(160,270),
                    force: new Vector2(10,0),
                    actionStates: new List<AnimationStates>(){AnimationStates.HardPunch,AnimationStates.ChainHardPunch}
                )
            },
            {
                AnimationStates.SpecialPunch,
                new AttackV4
                (
                    hitF: true,
                    dmg: 5,
                    timeDmg: 1,
                    hitS: 0.3f,
                    cd: 0.4f,
                    inertia: Vector2.zero,
                    force: new Vector2(0,12),
                    actionStates: new List<AnimationStates>(){AnimationStates.SpecialPunch,AnimationStates.ChainSpecialPunch}
                )
            },
        };
    }

}
