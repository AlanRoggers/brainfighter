using Unity.MLAgents;
using UnityEngine;

public class AgentAcademy : MonoBehaviour
{
    [SerializeField] private int maxSteps;
    public CharacterV5 agent1;
    public CharacterV5 agent2;
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
        CharacterV5.OnHurt += Hurt;
        CharacterV5.OnBlock += Block;
        CharacterV5.OnStun += Stuned;
        CharacterV5.OnWin += Win;
    }
    void FixedUpdate()
    {
        Debug.Log($"Steps {stepCounter}");
        if (stepCounter < maxSteps)
        {
            stepCounter++;
            if (stepCounter % 10 == 0)
            {
                if (stepCounter > 10)
                {
                    if (lastAG1Health == agent1.Health && lastAG2Health == agent2.Health)
                    {
                        // Debug.Log("Entro");
                        agent1Brain.AddReward(-numSum);
                        agent2Brain.AddReward(-numSum);
                        numSum += 0.1f;
                    }
                    else
                    {
                        lastAG1Health = agent1.Health;
                        lastAG2Health = agent2.Health;
                        numSum = 0.1f;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Episodio Interrumpido");
            if (agent1.Health > agent2.Health)
            {
                agent1Brain.AddReward(25);
                agent2Brain.AddReward(-25);
            }
            else if (agent1.Health < agent2.Health)
            {
                agent1Brain.AddReward(-25);
                agent2Brain.AddReward(25);
            }
            stepCounter = 0;
            numSum = 1;
            agent1Brain.EpisodeInterrupted();
            agent2Brain.EpisodeInterrupted();
        }

    }
    public void Spawn()
    {
        float agent1X = Random.Range(maxNegativeX, maxPositiveX);
        float agent2X;

        int agent2Side = Random.Range(0, 1);

        if (agent2Side == 1)
        {
            agent2X = agent1X + 8f;
            agent2X = Mathf.Clamp(agent2X, agent1X, maxPositiveX);
        }
        else
        {
            agent2X = agent1X - 8f;
            agent2X = Mathf.Clamp(agent2X, maxNegativeX, agent1X);
        }

        agent1.transform.localPosition = new Vector2(agent1X, -8.51f);
        agent2.transform.localPosition = new Vector2(agent2X, -8.51f);

        Debug.LogAssertion("Spawn");
    }
    private void Hurt(int entryDamage, bool whichAgent)
    {
        if (whichAgent)
        {
            Debug.Log("[Hurt] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.1f * entryDamage);
            agent2Brain.AddReward(0.1f * entryDamage);
        }
        else
        {
            Debug.Log("[Hurt] +Agente1 -Agente2");
            agent1Brain.AddReward(0.1f * entryDamage);
            agent2Brain.AddReward(-0.1f * entryDamage);
        }
    }
    private void Block(int entryDamage, bool whichAgent)
    {
        if (whichAgent)
        {
            Debug.Log("[Block] +Agente1 -Agente2");
            agent1Brain.AddReward(0.2f);
            agent2Brain.AddReward(-0.2f);
        }
        else
        {
            Debug.Log("[Block] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.2f);
            agent2Brain.AddReward(+0.2f);
        }
    }
    private void Stuned(bool whichAgent)
    {
        if (whichAgent)
        {
            Debug.Log("[Stuned] -Agente1 +Agente2");
            agent1Brain.AddReward(-0.6f);
            agent2Brain.AddReward(+0.6f);
        }
        else
        {
            Debug.Log("[Stuned] +Agente1 -Agente2");
            agent1Brain.AddReward(+0.6f);
            agent2Brain.AddReward(-0.6f);
        }
    }
    private void Win(bool whichAgent)
    {
        Debug.LogAssertion("Win");
        if (whichAgent)
        {
            Debug.Log("[Win] +Agente1 -Agente2");
            agent1Brain.AddReward(50f);
            agent2Brain.AddReward(-50f);
            stepCounter = 0;
            numSum = 1;
            agent1Brain.EndEpisode();
            agent2Brain.EndEpisode();
        }
        else
        {
            Debug.Log("[Win] -Agente1 +Agente2");
            agent1Brain.AddReward(-50f);
            agent2Brain.AddReward(+50f);
            stepCounter = 0;
            numSum = 1;
            agent1Brain.EndEpisode();
            agent2Brain.EndEpisode();
        }
    }
}