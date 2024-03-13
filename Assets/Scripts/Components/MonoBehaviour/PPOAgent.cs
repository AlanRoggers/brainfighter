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
        // Debug.Log("Reseteo");
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
        // if (gameObject.layer == 6)
        // {
        //     Debug.Log($"CurrentState: {agentState}");
        //     Debug.Log($"CurrentStateEnemy: {enemyState}");
        // }
        sensor.AddObservation(normalizedCurrentDistanceX);
        sensor.AddObservation(academy.agent1.Health / 100f);
        sensor.AddObservation(academy.agent2.Health / 100f);
        sensor.AddObservation(character.OnColdoown);
        sensor.AddObservation(character.Resistance / 50f);
        sensor.AddObservation(StateObservation(character.CurrentState));
        sensor.AddObservation(StateObservation(gameObject.layer == 6 ? academy.agent2.CurrentState : academy.agent1.CurrentState));
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
    private int StateObservation(PlayerState state)
    {
        return state switch
        {
            Iddle => 1,
            Walk => 2,
            Back => 3,
            Jump => 4,
            Fall => 5,
            LowPunch => 6,
            MiddlePunch => 7,
            HardPunch => 8,
            SpecialPunch => 9,
            LowKick => 10,
            MiddleKick => 11,
            HardKick => 12,
            SpecialKick => 13,
            Hurt => 14,
            Block => 15,
            Stun => 16,
            _ => 0,
        };
    }
}
