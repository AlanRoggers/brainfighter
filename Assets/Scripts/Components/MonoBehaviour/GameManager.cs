using System.Collections;
using Unity.Sentis.Layers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float[] lastDistance = new float[] { -10000f, -10000f };
    public bool TrainStage;
    public float PlayersDistance { get; private set; }
    public Character Player1 { get; private set; }
    public Character Player2 { get; private set; }
    private readonly int maxSteps = 1000;
    private int steps = 0;
    private void Start()
    {
        Character[] chars = GetComponentsInChildren<Character>();
        Player1 = chars[0];
        Player2 = chars[1];

        PlayersDistance = UpdatePlayerDistance();

        if (TrainStage)
        {
            Player2.Agent.OnBegin += SpawnAgents;

            Player1.States.Hurt.OnHurt += AgentHurted;
            Player2.States.Hurt.OnHurt += AgentHurted;

            Player1.States.Block.OnBlock += AgentBlockedAttack;
            Player2.States.Block.OnBlock += AgentBlockedAttack;

            Player1.States.Stun.OnStun += AgentStuned;
            Player2.States.Stun.OnStun += AgentStuned;

            Player1.States.Back.Backing += Movement;
            Player1.States.Walk.Walking += Movement;
            Player2.States.Back.Backing += Movement;
            Player2.States.Walk.Walking += Movement;

        }

    }
    private void Update()
    {
        IgnoreCollisions();
        PlayersDistance = UpdatePlayerDistance();
    }
    private void FixedUpdate()
    {
        if (TrainStage)
        {
            steps++;

            Player1.Agent.AddReward(-0.0005f);
            Player2.Agent.AddReward(-0.0005f);

            AgentWin();

            if (steps == maxSteps)
            {
                // Player1.Agent.SetReward(0);
                // Player2.Agent.SetReward(0);
                Player1.Agent.EpisodeInterrupted();
                Player2.Agent.EpisodeInterrupted();
            }
        }


    }
    private float UpdatePlayerDistance() => Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x);
    private void IgnoreCollisions()
    {
        if (Player1.CurrentState == Player1.States.Jump || Player1.CurrentState == Player1.States.Fall ||
            Player2.CurrentState == Player2.States.Jump || Player2.CurrentState == Player2.States.Fall)
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body);
        else
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body, false);
    }

    #region TrainingAI
    private void AgentHurted(PPOAgent agent)
    {
        if (agent.gameObject.layer == 6)
            Player2.Agent.AddReward(1);
        else
            Player1.Agent.AddReward(1);

        // agent.AddReward(-1);
    }
    private void AgentBlockedAttack(PPOAgent agent)
    {
        if (agent.gameObject.layer == 6)
            Player2.Agent.AddReward(-1);
        else
            Player1.Agent.AddReward(-1);

        // agent.AddReward(1);
    }
    private void AgentStuned(PPOAgent agent)
    {
        if (agent.gameObject.layer == 6)
            Player2.Agent.AddReward(10);
        else
            Player1.Agent.AddReward(10);

        // agent.AddReward(-10);
    }
    private void Movement(Character character)
    {
        if (character.gameObject.layer == 6)
        {
            float distance = UpdatePlayerDistance();
            if (lastDistance[0] > distance)
                Player1.Agent.AddReward(0.1f);
            else
                Player1.Agent.AddReward(-0.1f);
            lastDistance[0] = distance;
        }
        else
        {
            float distance = UpdatePlayerDistance();
            if (lastDistance[1] > distance)
                Player2.Agent.AddReward(0.1f);
            else
                Player2.Agent.AddReward(-0.1f);
            lastDistance[1] = distance;
        }
    }
    private void AgentWin()
    {
        if (Player1.Health <= 0)
        {
            Player1.Agent.EpisodeInterrupted();
            Player2.Agent.EndEpisode();
            return;
        }

        if (Player2.Health <= 0)
        {
            Debug.Log("IA Gano");
            Player1.Agent.EndEpisode();
            Player2.Agent.EpisodeInterrupted();
        }
    }
    private void SpawnAgents()
    {
        steps = 0;
        float distance = 12f;
        float Player1X = Random.Range(-14f, 12f);
        float Player2X;

        if (Player1X - distance > -13f && Player1X + distance < 11)
            Player2X = Player1X + distance * (Random.Range(0, 1) == 0 ? 1 : -1);
        else if (Player1X - distance > -13f)
            Player2X = Player1X - distance;
        else
            Player2X = Player1X + distance;


        Player1.transform.localPosition = new Vector2(Player1X, Player1.Spawn.y);
        Player2.transform.localPosition = new Vector2(Player2X, Player2.Spawn.y);

        if (!Player1.IsAI)
            Player1.ResetParams();

        if (!Player2.IsAI)
            Player1.ResetParams();
    }

    #endregion
}
