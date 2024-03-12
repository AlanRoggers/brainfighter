using Unity.MLAgents;
using UnityEngine;

public class AgentAcademy : MonoBehaviour
{
    [SerializeField] private int maxSteps;
    public Character agent1;
    public Character agent2;
    [SerializeField] private float numSum;
    private readonly float maxNegativeX = -15f;
    private readonly float maxPositiveX = 10f;
    private int lastAG1Health;
    private int lastAG2Health;
    private int stepCounter = 0;
    private Agent agent1Brain;
    private Agent agent2Brain;
    private void Awake()
    {
        agent1Brain = agent1.GetComponent<Agent>();
        agent2Brain = agent2.GetComponent<Agent>();
    }
    private void Start()
    {
        // agent1.States.Hurt.OnHurt += Hurt;
        // agent1.States.Block.OnBlock += Block;
        // agent1.States.Stun.OnStun += Stuned;
        agent1.States.Dead.OnDead += Dead;
        // agent2.States.Hurt.OnHurt += Hurt;
        // agent2.States.Block.OnBlock += Block;
        // agent2.States.Stun.OnStun += Stuned;
        agent2.States.Dead.OnDead += Dead;
        PPOAgent.OnBegin += Spawn;
    }
    void FixedUpdate()
    {
        if (stepCounter < maxSteps)
        {
            stepCounter++;
            // if (stepCounter % 10 == 0)
            // {
            //     if (stepCounter > 10)
            //     {
            //         if (lastAG1Health == agent1.Health && lastAG2Health == agent2.Health)
            //         {
            //             // Debug.Log("Entro");
            //             agent1Brain.AddReward(-numSum);
            //             agent2Brain.AddReward(-numSum);
            //             numSum += 0.01f;
            //         }
            //         else
            //         {
            //             lastAG1Health = agent1.Health;
            //             lastAG2Health = agent2.Health;
            //             numSum = 0.1f;
            //         }
            //     }
            // }
        }
        else
        {
            // Debug.Log("Episodio Interrumpido");
            if (agent1.Health > agent2.Health)
            {
                agent1Brain.AddReward(1);
                agent2Brain.AddReward(-1);
            }
            else if (agent1.Health < agent2.Health)
            {
                agent1Brain.AddReward(-1);
                agent2Brain.AddReward(1);
            }
            agent1Brain.EndEpisode();
            agent2Brain.EndEpisode();
            stepCounter = 0;
            numSum = 1;
            // Debug.Log($"Recompensa del Agente 1: {agent1Brain.GetCumulativeReward()}");
            // Debug.Log($"Recompensa del Agente 2: {agent2Brain.GetCumulativeReward()}");
            // agent1Brain.EpisodeInterrupted();
            // agent2Brain.EpisodeInterrupted();
        }

    }
    public void Spawn(GameObject agent)
    {
        // Debug.Log("Spawn");
        float agentX = Random.Range(maxNegativeX, maxPositiveX);
        agent.transform.localPosition = new Vector2(agentX, -8.51f);
    }
    private void Hurt(int entryDamage, bool whichAgent)
    {
        if (whichAgent)
        {
            // Debug.Log("[Hurt] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.1f * entryDamage);
            agent2Brain.AddReward(0.1f * entryDamage);
        }
        else
        {
            // Debug.Log("[Hurt] +Agente1 -Agente2");
            agent1Brain.AddReward(0.1f * entryDamage);
            agent2Brain.AddReward(-0.1f * entryDamage);
        }
    }
    private void Block(int entryDamage, bool whichAgent)
    {
        if (whichAgent)
        {
            // Debug.Log("[Block] +Agente1 -Agente2");
            agent1Brain.AddReward(0.2f);
            agent2Brain.AddReward(-0.2f);
        }
        else
        {
            // Debug.Log("[Block] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.2f);
            agent2Brain.AddReward(+0.2f);
        }
    }
    private void Stuned(bool whichAgent)
    {
        if (whichAgent)
        {
            // Debug.Log("[Stuned] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.6f);
            agent2Brain.AddReward(+0.6f);
        }
        else
        {
            // Debug.Log("[Stuned] +Agente1 -Agente2");
            agent1Brain.AddReward(+0.6f);
            agent2Brain.AddReward(-0.6f);
        }
    }
    private void Dead(bool whichAgent)
    {
        Debug.Log($"Win {(whichAgent ? "P2" : "P1")}");
        if (whichAgent)
        {
            // Debug.Log("[Win] +Agente1 -Agente2");
            agent1Brain.AddReward(-1);
            agent2Brain.AddReward(1f);
            stepCounter = 0;
            numSum = 1;
            agent1Brain.EndEpisode();
            agent2Brain.EndEpisode();
        }
        else
        {
            // Debug.Log("[Win] -Agente1 +Agente2");
            agent1Brain.AddReward(1f);
            agent2Brain.AddReward(-1f);
            stepCounter = 0;
            numSum = 1;
            agent1Brain.EndEpisode();
            agent2Brain.EndEpisode();
        }
        // Debug.Log($"Recompensa del Agente 1: {agent1Brain.GetCumulativeReward()}");
        // Debug.Log($"Recompensa del Agente 2: {agent2Brain.GetCumulativeReward()}");
    }
}