using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;
public class PPOAgent : Agent
{
    public delegate void Begin();
    public event Begin OnBegin;
    public State RequestedAction { get; private set; }
    private Character character;
    private GameManager mngr;
    private readonly float maxDistance = 27.29f;
    private readonly float minDistance = 1.25f;
    protected override void Awake()
    {
        base.Awake();
        character = GetComponent<Character>();
        mngr = GetComponentInParent<GameManager>();
    }
    public override void OnEpisodeBegin()
    {
        Reset();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log(Mathf.Abs((mngr.PlayersDistance - minDistance) / (maxDistance - minDistance)));
        sensor.AddObservation(Mathf.Abs((mngr.PlayersDistance - minDistance) / (maxDistance - minDistance)));
        sensor.AddObservation(gameObject.layer == 6 ? mngr.Player2.CurrentState is Block : mngr.Player1.CurrentState is Block);
        sensor.AddObservation(gameObject.layer == 6 ? mngr.Hurt2 : mngr.Hurt1); //Herir al enemigo
        sensor.AddObservation(gameObject.layer == 6 ? mngr.Block2 : mngr.Block1); // Enemigo bloquea
        sensor.AddObservation(transform.localScale.x > 0); //Dirección del enemigo
        sensor.AddObservation(mngr.PlayersDistance <= 1.7f); //Distancia adecuada
        sensor.AddObservation(character.Health > 0); //Vida
        sensor.AddObservation(gameObject.layer == 6 ? mngr.Player2.Health > 0 : mngr.Player1.Health > 0); //Vida del enemigo
        sensor.AddObservation(character.Resistance > 0); //Resistencia
        sensor.AddObservation(character.HitsChained / 4f); //Ataques encadenados
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int branch1 = actions.DiscreteActions[0];
        int branch2 = actions.DiscreteActions[1];
        if (branch2 == 0)
        {
            switch (branch1)
            {
                case 0:
                    RequestedAction = State.IDDLE;
                    break;
                case 1:
                    RequestedAction = State.WALK;
                    break;
                case 2:
                    RequestedAction = State.BACK;
                    break;
            }
        }
        else
        {
            switch (branch2)
            {
                case 0:
                    RequestedAction = State.IDDLE;
                    break;
                case 1:
                    RequestedAction = State.JUMP;
                    break;
                case 2:
                    RequestedAction = State.LOW_PUNCH;
                    break;
                case 3:
                    RequestedAction = State.MIDDLE_PUNCH;
                    break;
                case 4:
                    RequestedAction = State.HARD_PUNCH;
                    break;
                case 5:
                    RequestedAction = State.LOW_KICK;
                    break;
                case 6:
                    RequestedAction = State.MIDDLE_KICK;
                    break;
                case 7:
                    RequestedAction = State.HARD_KICK;
                    break;
                case 8:
                    RequestedAction = State.SPECIAL_KICK;
                    break;
                case 9:
                    RequestedAction = State.SPECIAL_PUNCH;
                    break;
                case 10:
                    RequestedAction = State.BLOCK;
                    break;
            }
        }
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        if (character.OnColdoown)
        {
            actionMask.SetActionEnabled(1, 2, false);
            actionMask.SetActionEnabled(1, 3, false);
            actionMask.SetActionEnabled(1, 4, false);
            actionMask.SetActionEnabled(1, 5, false);
            actionMask.SetActionEnabled(1, 6, false);
            actionMask.SetActionEnabled(1, 7, false);
            actionMask.SetActionEnabled(1, 8, false);
            actionMask.SetActionEnabled(1, 9, false);
        }

        switch (character.CurrentState)
        {
            case Jump:
            case Fall:
            case Block:
            case Stun:
            case SpecialKick:
            case SpecialPunch:
                actionMask.SetActionEnabled(0, 1, false);
                actionMask.SetActionEnabled(0, 2, false);
                actionMask.SetActionEnabled(1, 1, false);
                actionMask.SetActionEnabled(1, 2, false);
                actionMask.SetActionEnabled(1, 3, false);
                actionMask.SetActionEnabled(1, 4, false);
                actionMask.SetActionEnabled(1, 5, false);
                actionMask.SetActionEnabled(1, 6, false);
                actionMask.SetActionEnabled(1, 7, false);
                actionMask.SetActionEnabled(1, 8, false);
                actionMask.SetActionEnabled(1, 9, false);
                actionMask.SetActionEnabled(1, 10, false);
                // Debug.Log("Accion permitida: Iddle");
                break;
            case LowKick:
            case MiddleKick:
            case HardKick:
                // Debug.Log("Accion permitida: Patadas");
                actionMask.SetActionEnabled(0, 1, false);
                actionMask.SetActionEnabled(0, 2, false);
                actionMask.SetActionEnabled(1, 1, false);
                actionMask.SetActionEnabled(1, 2, false);
                actionMask.SetActionEnabled(1, 3, false);
                actionMask.SetActionEnabled(1, 4, false);
                actionMask.SetActionEnabled(1, 9, false);
                actionMask.SetActionEnabled(1, 10, false);
                break;
            case LowPunch:
            case MiddlePunch:
            case HardPunch:
                // Debug.Log("Accion permitida: Golpes");
                actionMask.SetActionEnabled(0, 1, false);
                actionMask.SetActionEnabled(0, 2, false);
                actionMask.SetActionEnabled(1, 5, false);
                actionMask.SetActionEnabled(1, 6, false);
                actionMask.SetActionEnabled(1, 7, false);
                actionMask.SetActionEnabled(1, 8, false);
                actionMask.SetActionEnabled(1, 10, false);
                break;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (character.AcceptInput)
        {

            ActionSegment<int> actions = actionsOut.DiscreteActions;

            if (Input.GetKey(KeyCode.B))
            {
                actions[1] = 10;
                return;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                actions[1] = 1;
                return;
            }

            if (Input.GetKey(KeyCode.U))
            {
                actions[1] = 2;
                return;
            }

            if (Input.GetKey(KeyCode.I))
            {
                actions[1] = 3;
                return;
            }

            if (Input.GetKey(KeyCode.O))
            {
                actions[1] = 4;
                return;
            }

            if (Input.GetKey(KeyCode.P))
            {
                actions[1] = 9;
                return;
            }

            if (Input.GetKey(KeyCode.J))
            {
                actions[1] = 5;
                return;
            }

            if (Input.GetKey(KeyCode.K))
            {
                actions[1] = 6;
                return;
            }

            if (Input.GetKey(KeyCode.L))
            {
                actions[1] = 7;
                return;
            }

            if (Input.GetKey(KeyCode.Semicolon))
            {
                actions[1] = 8;
                return;
            }

            if (Input.GetKey(KeyCode.D))
            {
                actions[0] = 1;
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {

                actions[0] = 2;
                return;
            }

            actions[0] = 0;
            actions[1] = 0;
        }
    }
    private int StateObservation(PlayerState state)
    {
        return state switch
        {
            Iddle => 0,
            Walk => 1,
            Back => 2,
            Jump => 3,
            Fall => 4,
            LowPunch => 5,
            MiddlePunch => 6,
            HardPunch => 7,
            SpecialPunch => 8,
            LowKick => 9,
            MiddleKick => 10,
            HardKick => 11,
            SpecialKick => 12,
            Hurt => 13,
            Block => 14,
            Stun => 15,
            _ => 16,
        };
    }
    public void Reset()
    {
        // Debug.Log($"{(character.gameObject.layer == 6 ? "A1 Reset" : "A2 Reset")}");
        character.CurrentState.OnExit(character);
        character.AttackReceived = null;
        character.EntryAttack = false;
        character.OnColdoown = false;
        character.HitsChained = 0;
        character.Physics.velocity = Vector2.zero;
        if (character.CoolDownCor != null)
        {
            StopCoroutine(character.CoolDownCor);
            character.CoolDownSet = null;
        }
        character.Physics.gravityScale = 4;
        character.HealthSet = 25;
        // character.HealthSet = Random.Range(10, 100);
        character.ResistanceSet = 15;
        character.Friction.friction = 1; // Tal vez no
        // character.transform.localPosition = character.Spawn;
        character.Animator.speed = 1;
        character.FutureStateSet = character.States.Iddle;
        character.Reset = true;
        OnBegin?.Invoke();
    }
}
