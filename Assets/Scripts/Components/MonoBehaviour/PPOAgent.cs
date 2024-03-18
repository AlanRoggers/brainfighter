using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
public class PPOAgent : Agent
{
    public delegate void Begin();
    public event Begin OnBegin;
    public State RequestedAction { get; private set; }
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
        Reset();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        float normalizedCurrentDistanceX = Mathf.Clamp(
            (mngr.PlayersDistance - minDistance) / (maxDistance - minDistance),
            0.0f,
            1.0f
        );
        sensor.AddObservation(normalizedCurrentDistanceX);
        sensor.AddObservation(character.Health / 100f);
        sensor.AddObservation((gameObject.layer == 6 ? mngr.Player2.Health : mngr.Player1.Health) / 100f);
        sensor.AddObservation(character.OnColdoown);
        sensor.AddObservation(character.Resistance / 50f);
        sensor.AddObservation(character.OverlapDetector.EnemyOverlapping(
                character.Body,
                character.gameObject.layer == 6 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1")
            )
        );
        sensor.AddObservation(character.HitsChained / 4f);
        sensor.AddObservation(Mathf.Sign(character.transform.localScale.x));
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
                if (character.transform.localScale.x > 0)
                    RequestedAction = State.WALK;
                else
                    RequestedAction = State.BACK;
                break;
            case 2:
                if (character.transform.localScale.x > 0)
                    RequestedAction = State.BACK;
                else
                    RequestedAction = State.WALK;
                break;
            case 3:
                RequestedAction = State.JUMP;
                break;
            case 4:
                RequestedAction = State.LOW_PUNCH;
                break;
            case 5:
                RequestedAction = State.MIDDLE_PUNCH;
                break;
            case 6:
                RequestedAction = State.HARD_PUNCH;
                break;
            case 7:
                RequestedAction = State.LOW_KICK;
                break;
            case 8:
                RequestedAction = State.MIDDLE_KICK;
                break;
            case 9:
                RequestedAction = State.HARD_KICK;
                break;
            case 10:
                RequestedAction = State.SPECIAL_KICK;
                break;
            case 11:
                RequestedAction = State.SPECIAL_PUNCH;
                break;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (character.AcceptInput)
        {

            ActionSegment<int> actions = actionsOut.DiscreteActions;

            if (Input.GetKey(KeyCode.Space))
            {
                actions[0] = 3;
                return;
            }

            if (Input.GetKey(KeyCode.U))
            {
                actions[0] = 4;
                return;
            }

            if (Input.GetKey(KeyCode.I))
            {
                actions[0] = 5;
                return;
            }

            if (Input.GetKey(KeyCode.O))
            {
                actions[0] = 6;
                return;
            }

            if (Input.GetKey(KeyCode.P))
            {
                actions[0] = 11;
                return;
            }

            if (Input.GetKey(KeyCode.J))
            {
                actions[0] = 7;
                return;
            }

            if (Input.GetKey(KeyCode.K))
            {
                actions[0] = 8;
                return;
            }

            if (Input.GetKey(KeyCode.L))
            {
                actions[0] = 9;
                return;
            }

            if (Input.GetKey(KeyCode.Semicolon))
            {
                actions[0] = 10;
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
        character.HealthSet = 50;
        // character.HealthSet = Random.Range(10, 100);
        character.ResistanceSet = 50;
        character.Friction.friction = 1; // Tal vez no
        // character.transform.localPosition = character.Spawn;
        character.Animator.speed = 1;
        character.FutureStateSet = character.States.Iddle;
        character.Reset = true;
        OnBegin?.Invoke();
    }
}
