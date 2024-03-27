using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly int maxHealth = 25;
    private float[] lastDistance = new float[] { -10000f, -10000f };
    public bool TrainStage;
    public float PlayersDistance { get; private set; }
    public Character Player1 { get; private set; }
    public Character Player2 { get; private set; }
    private readonly int maxSteps = 6000;
    private int steps = 0;
    public bool Hurt1;
    public bool Hurt2;
    public bool Block1;
    public bool Block2;
    private void Start()
    {
        Character[] chars = GetComponentsInChildren<Character>();
        Player1 = chars[0];
        Player2 = chars[1];

        PlayersDistance = UpdatePlayerDistance();

        if (TrainStage)
        {
            Player2.Agent.OnBegin += SpawnAgents;

            Player2.States.Walk.Walking += AgentReduceDistanceW;
            Player2.States.Back.Backing += AgentReduceDistanceB;
            Player1.States.Hurt.OnHurt += AgentHurted;
            Player2.States.Hurt.OnHurt += AgentHurted;

            Player1.States.Walk.Walking += AgentReduceDistanceW;
            Player1.States.Back.Backing += AgentReduceDistanceB;
            // Player1.States.Block.OnBlock += AgentBlockedAttack;
            // Player2.States.Block.OnBlock += AgentBlockedAttack;

            // Player1.States.Stun.OnStun += AgentStuned;
            // Player2.States.Stun.OnStun += AgentStuned;

            // Player1.States.Back.Backing += Movement;
            // Player1.States.Walk.Walking += Movement;
            // Player2.States.Back.Backing += Movement;
            // Player2.States.Walk.Walking += Movement;


        }

    }
    private void Update()
    {
        Hurt1 = Player1.CurrentState is Hurt;
        Hurt2 = Player2.CurrentState is Hurt;
        Block1 = Player1.CurrentState is Block;
        Block2 = Player2.CurrentState is Block;
        IgnoreCollisions();
        PlayersDistance = UpdatePlayerDistance();
        // Debug.Log(PlayersDistance);
    }
    private void FixedUpdate()
    {
        if (TrainStage)
        {
            AgentWin();

            steps++;

            if (steps == maxSteps)
                InterruptedEpisodes();
        }


    }
    private float UpdatePlayerDistance() => MathF.Round(Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x), 2, MidpointRounding.AwayFromZero);
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
        {
            Player1.Agent.AddReward(-0.075f);
            Player2.Agent.AddReward(0.075f);
        }
        else
        {
            Player1.Agent.AddReward(0.075f);
            Player2.Agent.AddReward(-0.075f);
        }

        // agent.AddReward(-1);
    }
    private void AgentReduceDistanceW(PPOAgent agent)
    {
        if (agent.transform.localScale.x > 0)
        {
            if (PlayersDistance > 1.5f)
                agent.AddReward(0.1f);
        }
        else agent.AddReward(-0.1f);
    }
    private void AgentReduceDistanceB(PPOAgent agent)
    {
        if (agent.transform.localScale.x < 0)
        {
            if (PlayersDistance > 1.5f)
                agent.AddReward(0.1f);
        }
        else agent.AddReward(-0.1f);
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
    private void AgentWin()
    {
        if (Player1.Health <= 0)
        {
            Player1.Agent.AddReward(-1 * (1 - Player1.Agent.GetCumulativeReward()));
            Player2.Agent.AddReward(1 - Player2.Agent.GetCumulativeReward());
            // Debug.Log($"Agente 1: {Player1.Agent.GetCumulativeReward()}");
            // Debug.Log($"Agente 2: {Player2.Agent.GetCumulativeReward()}");
            EndEpisodes();
            return;
        }

        if (Player2.Health <= 0)
        {
            Player1.Agent.AddReward(1 - Player1.Agent.GetCumulativeReward());
            Player2.Agent.AddReward(-1 * (1 - Player2.Agent.GetCumulativeReward()));
            // Debug.Log($"Agente 1: {Player1.Agent.GetCumulativeReward()}");
            // Debug.Log($"Agente 2: {Player2.Agent.GetCumulativeReward()}");
            EndEpisodes();
        }
    }
    private void SpawnAgents()
    {
        steps = 0;
        float distance = 5f;
        float Player1X = UnityEngine.Random.Range(-14f, 12f);
        float Player2X;

        if (Player1X - distance > -13f && Player1X + distance < 11)
            Player2X = Player1X + distance * (UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1);
        else if (Player1X - distance > -13f)
            Player2X = Player1X - distance;
        else
            Player2X = Player1X + distance;


        Player1.transform.localPosition = new Vector2(Player1X, Player1.Spawn.y);
        Player2.transform.localPosition = new Vector2(Player2X, Player2.Spawn.y);
    }
    private void EndEpisodes()
    {
        Player1.Agent.EndEpisode();
        Player2.Agent.EndEpisode();
    }
    private void InterruptedEpisodes()
    {
        // if (Player1.Health > Player2.Health)
        // {
        //     Player1.Agent.AddReward((100f - Player2.Health) / 100f);
        //     Player2.Agent.AddReward(-Player1.Health / 100f);
        // }
        // else if (Player1.Health < Player2.Health)
        // {
        //     Player2.Agent.AddReward((100f - Player1.Health) / 100f);
        //     Player1.Agent.AddReward(-Player2.Health / 100f);
        // }
        Player1.Agent.EpisodeInterrupted();
        Player2.Agent.EpisodeInterrupted();
    }
    #endregion
}
