using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;
public class PPOAgent : Agent
{
    public static event ResetEnvironment OnReset;
    public delegate void ResetEnvironment();
    public AgentAcademy academy;
    private CharacterV5 character;
    private readonly float maxDistance = 32.10f;
    private readonly float minDistance = 1f;
    private Dictionary<string, KeyCode> onePlayer = new Dictionary<string, KeyCode>();
    // private Dictionary<string, KeyCode> twoPlayer = new Dictionary<string, KeyCode>();
    protected override void Awake()
    {
        base.Awake();
        character = GetComponent<CharacterV5>();
    }
    public override void OnEpisodeBegin()
    {
        OnReset.Invoke();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log($"Academia {academy}");
        float currentDistanceX = Mathf.Abs(academy.agent1.transform.localPosition.x - academy.agent2.transform.localPosition.x);
        float normalizedCurrentDistanceX = Mathf.Clamp(
            (currentDistanceX - minDistance) / (maxDistance - minDistance),
            0.0f,
            1.0f
        );
        float currentState = 0;
        switch (character.currentState)
        {
            case Iddle:
                currentState = 1;
                break;
            case WalkV5:
                currentState = 2;
                break;
            case BackV5:
                currentState = 3;
                break;
            case JumpV5:
                currentState = 4;
                break;
            case FallV5:
                currentState = 5;
                break;
            case LowPunch:
                currentState = 6;
                break;
            case MiddlePunch:
                currentState = 7;
                break;
            case HardPunch:
                currentState = 8;
                break;
            case SpecialPunch:
                currentState = 9;
                break;
            case LowKick:
                currentState = 10;
                break;
            case MiddleKick:
                currentState = 11;
                break;
            case HardKick:
                currentState = 12;
                break;
            case SpecialKick:
                currentState = 13;
                break;
            case Hurt:
                currentState = 14;
                break;
            case BlockV5:
                currentState = 15;
                break;
            case Stun:
                currentState = 16;
                break;
        }
        // Distancia normalizada entre los personajes
        sensor.AddObservation(normalizedCurrentDistanceX); //Distancia entre el enemigo (Es un vector de dos dimensiones) 1+
        // Vida el enemigo normalizada
        sensor.AddObservation(academy.agent1.Health / 100f); // Vida del enemigo 1 +
        // Vida normalizada
        sensor.AddObservation(academy.agent1.Health / 100f); // Vida propia 1 +
        // Esto se va reemplazar mandandole el estado completo en el que esta
        // // Me estan haciendo daño
        // sensor.AddObservation(components.msng.IsTakingDamage); // Esta variable controla si puede o no atacar 1 +
        // // Tiempo de recuperación
        // sensor.AddObservation(components.msng.AttackRestricted);
        // // Estoy atacando no puedo atacar más
        // sensor.AddObservation(components.msng.IsAttacking);
        // // Estoy en el aire
        // sensor.AddObservation(components.msng.IsOnGround); //Tocando el piso 1 +
        // // Enemigo esta bloquendo
        // sensor.AddObservation(components.msng.Enemy.GetComponent<Components>().msng.IsBlocking);
        sensor.AddObservation(currentState / 16);

        // sensor.AddObservation(components.msng.Enemy.GetComponent<Components>().msng.Dead); //Enemigo muerto 1 +
        // sensor.AddObservation(components.msng.Dead); //Me mori 1 = 8
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int motionAction = actions.DiscreteActions[0];
        int behaviourAction = actions.DiscreteActions[1];
        switch (motionAction)
        {
            case 0:
                character.RequestedMotionAction = State.IDDLE;
                break;
            case 1:
                character.RequestedMotionAction = State.WALK;
                break;
            case 2:
                character.RequestedMotionAction = State.BACK;
                break;
        }
        switch (behaviourAction)
        {
            case 0:
                character.RequestedBehaviourAction = State.IDDLE;
                break;
            case 1:
                character.RequestedBehaviourAction = State.JUMP;
                break;
            case 2:
                character.RequestedBehaviourAction = State.LOW_PUNCH;
                break;
            case 3:
                character.RequestedBehaviourAction = State.MIDDLE_PUNCH;
                break;
            case 4:
                character.RequestedBehaviourAction = State.HARD_PUNCH;
                break;
            case 5:
                character.RequestedBehaviourAction = State.LOW_KICK;
                break;
            case 6:
                character.RequestedBehaviourAction = State.MIDDLE_KICK;
                break;
            case 7:
                character.RequestedBehaviourAction = State.HARD_KICK;
                break;
            case 8:
                character.RequestedBehaviourAction = State.SPECIAL_KICK;
                break;
            case 9:
                character.RequestedBehaviourAction = State.SPECIAL_PUNCH;
                break;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        if (gameObject.layer == 6)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                actions[0] = 0;

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

            actions[1] = 0;

            // else if (Input.GetKey(onePlayer["LowPunch"]))
            //     actions[1] = 2;
        }
    }
}
