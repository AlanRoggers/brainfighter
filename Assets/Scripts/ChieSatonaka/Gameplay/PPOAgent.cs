using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;
public class PPOAgent : Agent
{
    [SerializeField] GameManager gameManager;
    private Components components;
    private readonly float maxDistance = 32.10f;
    private readonly float minDistance = 1f;
    protected override void Awake()
    {
        base.Awake();
        components = GetComponent<Components>();
    }

    public override void OnEpisodeBegin()
    {
        components.Academy.Spawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float currentDistanceX = Mathf.Abs(transform.localPosition.x - components.msng.Enemy.transform.localPosition.x);
        float normalizedCurrentDistanceX = Mathf.Clamp(
            (currentDistanceX - minDistance) / (maxDistance - minDistance),
            0.0f,
            1.0f
        );
        bool actionsRestricted = components.msng.AttackRestricted || components.msng.IsAttacking || components.msng.IsTakingDamage; // Aca puedo meter también las de agachado dashing, saltando, entre otras

        // Distancia normalizada entre los personajes
        sensor.AddObservation(normalizedCurrentDistanceX); //Distancia entre el enemigo (Es un vector de dos dimensiones) 1+
        // Vida el enemigo normalizada
        sensor.AddObservation(components.msng.Enemy.GetComponent<PlayerHealth>().Health / 100f); // Vida del enemigo 1 +
        // Vida normalizada
        sensor.AddObservation(components.Health.Health / 100f); // Vida propia 1 +
        // Me estan haciendo daño
        sensor.AddObservation(components.msng.IsTakingDamage); // Esta variable controla si puede o no atacar 1 +
        // Tiempo de recuperación
        sensor.AddObservation(components.msng.AttackRestricted);
        // Estoy atacando no puedo atacar más
        sensor.AddObservation(components.msng.IsAttacking);
        // Estoy en el aire
        sensor.AddObservation(components.msng.IsOnGround); //Tocando el piso 1 +
        // Enemigo esta bloquendo
        sensor.AddObservation(components.msng.Enemy.GetComponent<Components>().msng.IsBlocking);


        // sensor.AddObservation(components.msng.Enemy.GetComponent<Components>().msng.Dead); //Enemigo muerto 1 +
        // sensor.AddObservation(components.msng.Dead); //Me mori 1 = 8
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // print($"Action entered   {actions.DiscreteActions[0]}   {actions.DiscreteActions[1]}");
        int motionAction = actions.DiscreteActions[0];
        int behaviourAction = actions.DiscreteActions[1];
        switch (motionAction)
        {
            case 0: // No hacer nada
                components.motion.StandUp();
                components.motion.Walk(0);
                break;
            case 1: // Caminar 1
                components.motion.Walk(1);
                break;
            case 2: // Caminar 2
                components.motion.Walk(2);
                break;
            case 3: // Impulso normal
                components.motion.Dash();
                break;
            case 4: // Impulso hacía atrás
                components.motion.DashBack();
                break;
            case 5: // Agacharse
                components.motion.Crouch();
                break;
        }
        switch (behaviourAction)
        {
            case 0: // No hacer nada
                components.motion.StopBlock();
                break;
            case 1: // Saltar
                components.motion.Jump();
                break;
            case 2: // Golpe 1
                components.attacks.LowPunch();
                break;
            case 3: // Golpe 2
                components.attacks.MiddlePunch();
                break;
            case 4: // Golpe 3
                components.attacks.HardPunch();
                break;
            case 5: // Golpe 4
                components.attacks.SpecialPunch();
                break;
            case 6: // Patada 1
                components.attacks.LowKick();
                break;
            case 7: // Patada 2
                components.attacks.MiddleKick();
                break;
            case 8: // Patada 3
                components.attacks.HardKick();
                break;
            case 9: // Patada 4
                components.attacks.SpecialKick();
                break;
            case 10: // Bloquear
                components.motion.Block();
                break;
        }
    }

}
