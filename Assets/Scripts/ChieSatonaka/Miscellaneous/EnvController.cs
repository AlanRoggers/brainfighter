using UnityEngine;

public class EnvController : MonoBehaviour
{
    [SerializeField] private int maxSteps;
    [SerializeField] private Components chieAgent;
    [SerializeField] private Components satonakaAgent;
    private readonly float maxNegativeX = -15f;
    private readonly float maxPositiveX = 0f;
    public void ManageEvents(AgentEvents eventReceived, bool chie, float damage = 0)
    {
        switch (eventReceived)
        {
            case AgentEvents.Damage:
                if (chie)
                {
                    chieAgent.Brain.AddReward(0.1f * damage);
                    satonakaAgent.Brain.AddReward(-0.1f * damage);
                }
                else
                {
                    satonakaAgent.Brain.AddReward(0.1f * damage);
                    chieAgent.Brain.AddReward(-0.1f * damage);
                }
                break;
            case AgentEvents.Loss:
                if (!chie)
                {
                    chieAgent.Brain.AddReward(+100f);
                    satonakaAgent.Brain.AddReward(-100f);
                }
                else
                {
                    satonakaAgent.Brain.AddReward(+100f);
                    chieAgent.Brain.AddReward(-100f);
                }
                chieAgent.Brain.EndEpisode();
                satonakaAgent.Brain.EndEpisode();
                break;
            case AgentEvents.Nothing:
                if (chie)
                    chieAgent.Brain.AddReward(-0.0001f);
                else
                    satonakaAgent.Brain.AddReward(-0.0001f);
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
    }
}
