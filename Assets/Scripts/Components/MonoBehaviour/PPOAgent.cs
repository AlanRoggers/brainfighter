using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
public class PPOAgent : Agent
{
    public State RequestedAction { get; private set; }
    public delegate void EpisodeBegin(GameObject gameObject);
    public static event EpisodeBegin OnBegin;
    private Character character;
    private GameManager mngr;
    private readonly float maxDistance = 27.8f;
    private readonly float minDistance = 1.26f;
    protected override void Awake()
    {
        base.Awake();
        character = GetComponent<Character>();
        mngr = GetComponentInParent<GameManager>();
    }
    public override void OnEpisodeBegin()
    {
        // Debug.Log("Reseteo");
        Reset();
        OnBegin.Invoke(gameObject);
        // if (character.gameObject.layer == 6)
        //     academy.Spawn();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Debug.Log($"Academia {academy}");
        float normalizedCurrentDistanceX = Mathf.Clamp(
            (mngr.playersDistance - minDistance) / (maxDistance - minDistance),
            0.0f,
            1.0f
        );
        // if (gameObject.layer == 6)
        // {
        //     Debug.Log($"CurrentState: {agentState}");
        //     Debug.Log($"CurrentStateEnemy: {enemyState}");
        // }
        sensor.AddObservation(normalizedCurrentDistanceX);
        sensor.AddObservation(character.Health / 100f);
        sensor.AddObservation((gameObject.layer == 6 ? mngr.Player2.Health : mngr.Player1.Health) / 100f);
        sensor.AddObservation(character.OnColdoown);
        sensor.AddObservation(character.Resistance / 50f);
        sensor.AddOneHotObservation(StateObservation(character.CurrentState), 16);
        sensor.AddOneHotObservation(StateObservation
        (
            gameObject.layer == 6 ? mngr.Player2.CurrentState : mngr.Player1.CurrentState), 16
        );
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        switch (actions.DiscreteActions[0])
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
            case 3:
                RequestedAction = State.IDDLE;
                break;
            case 4:
                RequestedAction = State.JUMP;
                break;
            case 5:
                RequestedAction = State.LOW_PUNCH;
                break;
            case 6:
                RequestedAction = State.MIDDLE_PUNCH;
                break;
            case 7:
                RequestedAction = State.HARD_PUNCH;
                break;
            case 8:
                RequestedAction = State.LOW_KICK;
                break;
            case 9:
                RequestedAction = State.MIDDLE_KICK;
                break;
            case 10:
                RequestedAction = State.HARD_KICK;
                break;
            case 11:
                RequestedAction = State.SPECIAL_KICK;
                break;
            case 12:
                RequestedAction = State.SPECIAL_PUNCH;
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
        character.HealthSet = 100;
        character.ResistanceSet = 50;
        character.Friction.friction = 1; // Tal vez no
        character.FutureStateSet = character.States.Iddle;
    }
}
