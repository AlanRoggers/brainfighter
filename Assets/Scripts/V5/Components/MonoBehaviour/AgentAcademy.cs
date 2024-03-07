using Unity.MLAgents;
using UnityEngine;

public class AgentAcademy : MonoBehaviour
{
    [SerializeField] private int maxSteps;
    [SerializeField] private CharacterV5 agent1;
    [SerializeField] private CharacterV5 agent2;
    [SerializeField] private int numSum;
    private readonly float maxNegativeX = -15f;
    public bool monitorRewards;
    private readonly float maxPositiveX = 0f;
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
    void FixedUpdate()
    {
        if (stepCounter < maxSteps)
        {
            stepCounter++;
            if (stepCounter % 10 == 0)
            {
                if (stepCounter > 10)
                {
                    if (lastAG1Health == agent1.Health && lastAG2Health == agent2.Health)
                    {
                        agent1Brain.AddReward(-numSum);
                        agent2Brain.AddReward(-numSum);
                        numSum += 2;
                    }
                    else
                    {
                        // if (monitorRewards)
                        //     print("Hola se reseteo");
                        lastAG1Health = agent1.Health;
                        lastAG2Health = agent2.Health;
                        numSum = 1;
                    }
                }
            }
        }
        else
        {
            if (monitorRewards)
            {
                print($"Recompensa de Chie:{agent1Brain.GetCumulativeReward()}");
                print($"Recompensa de Satonaka:{agent2Brain.GetCumulativeReward()}");
            }
            agent1Brain.EpisodeInterrupted();
            agent2Brain.EpisodeInterrupted();
        }
    }
    public void ManageEvents(AgentEvent eventReceived, bool agent1, float damage = 0)
    {
        switch (eventReceived)
        {
            case AgentEvent.DidDamage:
                if (agent1)
                    agent1Brain.AddReward(0.1f * damage);
                else
                    agent2Brain.AddReward(0.1f * damage);
                break;
            case AgentEvent.ReceivedDamage:
                if (agent1)
                    agent1Brain.AddReward(-0.1f * damage);
                else
                    agent2Brain.AddReward(-0.1f * damage);
                break;
            case AgentEvent.KickWhileBlocked:
                if (agent1)
                    agent1Brain.AddReward(-0.2f);
                else
                    agent2Brain.AddReward(-0.2f);
                break;
            case AgentEvent.AttackBlocked:
                if (agent1)
                    agent1Brain.AddReward(0.2f);
                else
                    agent2Brain.AddReward(-0.2f);
                break;
            case AgentEvent.EnemyStuned:
                if (agent1)
                {
                    agent1Brain.AddReward(0.6f);
                    agent2Brain.AddReward(-0.6f);
                }
                else
                {
                    agent1Brain.AddReward(-0.6f);
                    agent2Brain.AddReward(0.6f);
                }
                break;
            case AgentEvent.Loss:
                if (agent1)
                {
                    agent2Brain.AddReward(50f);
                    agent1Brain.AddReward(-50f);
                }
                else
                {
                    agent2Brain.AddReward(-50f);
                    agent1Brain.AddReward(50f);
                }
                if (monitorRewards)
                {
                    print($"Recompensa de Chie:{agent1Brain.GetCumulativeReward()}");
                    print($"Recompensa de Satonaka:{agent2Brain.GetCumulativeReward()}");
                }
                agent1Brain.EndEpisode();
                agent2Brain.EndEpisode();
                break;

        }
    }
    private void InitialValues()
    {

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

        agent1.transform.localPosition = new Vector2(agent1X, -4.5f);
        agent2.transform.localPosition = new Vector2(agent2X, -4.5f);

        stepCounter = 0;
        numSum = 1;
    }
}