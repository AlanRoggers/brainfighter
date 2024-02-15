using UnityEngine;

public class EnvController : MonoBehaviour
{
    [SerializeField] private int maxSteps;
    [SerializeField] private Components chieAgent;
    [SerializeField] private Components satonakaAgent;
    private readonly float maxNegativeX = -15f;
    private readonly float maxPositiveX = 0f;
    private int stepCounter = 0;
    void FixedUpdate()
    {
        if (stepCounter < maxSteps)
            stepCounter++;
        else
        {
            chieAgent.Brain.EpisodeInterrupted();
            satonakaAgent.Brain.EpisodeInterrupted();
        }
    }
    public void ManageEvents(AgentEvents eventReceived, bool chie, float damage = 0)
    {
        Components agent = chie ? chieAgent : satonakaAgent;
        switch (eventReceived)
        {
            case AgentEvents.DidDamage:
                agent.Brain.AddReward(0.1f * damage);
                break;
            case AgentEvents.ReceivedDamage:
                agent.Brain.AddReward(-0.1f * damage);
                break;
            case AgentEvents.KickWhileBlocked:
                agent.Brain.AddReward(0.05f);
                break;
            case AgentEvents.AttackBlocked:
                agent.Brain.AddReward(0.5f);
                break;
            case AgentEvents.Loss:
                if (chie)
                    satonakaAgent.Brain.AddReward(+20f);
                else
                    chieAgent.Brain.AddReward(+20f);
                chieAgent.Brain.EndEpisode();
                satonakaAgent.Brain.EndEpisode();
                break;

        }
    }
    private void InitialValues(Components agent)
    {
        agent.phys.velocity = Vector2.zero;
        agent.states.RestartAgent();
        agent.Health.NewLife();
        agent.msng.StartValues();
    }
    public void Spawn()
    {
        InitialValues(chieAgent);
        InitialValues(satonakaAgent);

        float chieX = Random.Range(maxNegativeX, maxPositiveX);
        float satonakaX;

        int satonakaSide = Random.Range(0, 1);

        if (satonakaSide == 1)
        {
            satonakaX = chieX + 8f;
            satonakaX = Mathf.Clamp(satonakaX, chieX, maxPositiveX);
        }
        else
        {
            satonakaX = chieX - 8f;
            satonakaX = Mathf.Clamp(satonakaX, maxNegativeX, chieX);
        }

        chieAgent.transform.localPosition = new Vector2(chieX, -4.5f);
        satonakaAgent.transform.localPosition = new Vector2(satonakaX, -4.5f);

        stepCounter = 0;
    }
}
