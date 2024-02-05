using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
public class PPOAgent : Agent
{
    private Components components;
    private readonly float maxNegativeX = -8.3f;
    private readonly float maxPositieX = 17.7f;

    protected override void Awake()
    {
        base.Awake();
        components = GetComponent<Components>();
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        components.msng.StartValues();
        components.phys.velocity = Vector2.zero;
        Spawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }


    private void Spawn()
    {
        float posX = Random.Range(maxNegativeX, maxPositieX);
        transform.position = new Vector2(posX, -4.5f);
    }
}
