using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
public class PPOAgent : Agent
{
    private int x = 0;
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
        components.msng.StartValues();
        components.phys.velocity = Vector2.zero;
        Spawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 distance = new Vector2(
            Mathf.Abs(transform.localPosition.x - components.msng.Enemy.transform.localPosition.x),
            Mathf.Abs(transform.localPosition.y - components.msng.Enemy.transform.localPosition.y)
        );
        bool actionsRestricted = components.msng.AttackRestricted || components.msng.IsAttacking || components.msng.IsTakingDamage; // Aca puedo meter también las de agachado dashing, saltando, entre otras

        sensor.AddObservation(distance); //Distancia entre el enemigo (Es un vector de dos dimensiones) 2 +
        sensor.AddObservation(components.msng.Enemy.GetComponent<PlayerHealth>().Health); // Vida del enemigo 1 +
        sensor.AddObservation(components.Health.Health); // Vida propia 1 +
        sensor.AddObservation(actionsRestricted); // Esta variable controla si puede o no atacar 1 +
        sensor.AddObservation(components.msng.IsOnGround); //Tocando el piso 1 +
        sensor.AddObservation(components.msng.Enemy.GetComponent<Components>().msng.Dead); //Enemigo muerto 1 +
        sensor.AddObservation(components.msng.Dead); //Me mori 1 = 8
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int motionAction = actions.DiscreteActions[0];
        int behaviourAction = actions.DiscreteActions[1];
        switch (motionAction)
        {
            case 0:
                components.motion.StandUp();
                components.motion.Walk(0);
                break;
            case 1:
                components.motion.Walk(1);
                break;
            case 2:
                components.motion.Walk(2);
                break;
            case 3:
                components.motion.Dash();
                break;
            case 4:
                components.motion.DashBack();
                break;
            case 5:
                components.motion.Crouch();
                break;
        }
        switch (behaviourAction)
        {
            case 0:
                components.motion.StopBlock();
                break;
            case 1:
                components.motion.Jump();
                break;
            case 2:
                components.attacks.LowPunch();
                break;
            case 3:
                components.attacks.MiddlePunch();
                break;
            case 4:
                components.attacks.HardPunch();
                break;
            case 5:
                components.attacks.SpecialPunch();
                break;
            case 6:
                components.attacks.LowKick();
                break;
            case 7:
                components.attacks.MiddleKick();
                break;
            case 8:
                components.attacks.HardKick();
                break;
            case 9:
                components.attacks.SpecialKick();
                break;
            case 10:
                components.motion.Block();
                break;
        }
        // x++;
        // print($"Acción recibida del primer vector al momento {x} : {actions.DiscreteActions[0]}");
        // print($"Acción recibida del segundo vector al momento {x} : {actions.DiscreteActions[1]}");
    }


    private void Spawn()
    {
        float posX = Random.Range(maxNegativeX, maxPositieX);
        transform.position = new Vector2(posX, -4.5f);
    }
}
