using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
public class PPOAgent : Agent
{
    public delegate void EpisodeBegin(GameObject gameObject);
    public static event EpisodeBegin OnBegin;
    public AgentAcademy academy;
    private Character character;
    private readonly float maxDistance = 27.8f;
    private readonly float minDistance = 1.26f;
    protected override void Awake()
    {
        base.Awake();
        character = GetComponent<Character>();
    }
    public override void OnEpisodeBegin()
    {
        Debug.Log("Reseteo");
        character.Reset();
        OnBegin.Invoke(gameObject);
        // if (character.gameObject.layer == 6)
        //     academy.Spawn();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Debug.Log($"Academia {academy}");
        float currentDistanceX = Mathf.Abs(academy.agent1.transform.localPosition.x - academy.agent2.transform.localPosition.x);
        float normalizedCurrentDistanceX = Mathf.Clamp(
            (currentDistanceX - minDistance) / (maxDistance - minDistance),
            0.0f,
            1.0f
        );
        float currentState = 0;
        switch (character.CurrentState)
        {
            case Iddle:
                currentState = 1;
                break;
            case WalkV5:
                currentState = 2;
                break;
            case Back:
                currentState = 3;
                break;
            case Jump:
                currentState = 4;
                break;
            case Fall:
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
            case Block:
                currentState = 15;
                break;
            case Stun:
                currentState = 16;
                break;
        }
        sensor.AddObservation(normalizedCurrentDistanceX);
        sensor.AddObservation(academy.agent1.Health / 100f);
        sensor.AddObservation(academy.agent1.Health / 100f);
        sensor.AddObservation(character.OnColdoown);
        sensor.AddObservation(character.Resistance / 50f);
        sensor.AddObservation(currentState / 16);
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
        if (gameObject.layer == 7)
        {
            BehaviourActions(actions);
        }

        if (gameObject.layer == 6)
            MotionActions(actions);
    }
    private void BehaviourActions(ActionSegment<int> actions)
    {
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
    }
    private void MotionActions(ActionSegment<int> actions)
    {
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
    }
}
