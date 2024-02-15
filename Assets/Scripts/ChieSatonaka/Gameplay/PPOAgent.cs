using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;
public class PPOAgent : Agent
{
    private Components components;
    private readonly float maxDistance = 32.10f;
    private readonly float minDistance = 1f;
    private readonly int maxSteps = 2000;
    private Dictionary<string, KeyCode> onePlayer = new Dictionary<string, KeyCode>();
    // private Dictionary<string, KeyCode> twoPlayer = new Dictionary<string, KeyCode>();
    protected override void Awake()
    {
        base.Awake();
        components = GetComponent<Components>();
        InitDictionarites();
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
        this.AddReward(-1f / maxSteps);
        // print($"Action entered   {actions.DiscreteActions[0]}   {actions.DiscreteActions[1]}");
        int motionAction = actions.DiscreteActions[0];
        int behaviourAction = actions.DiscreteActions[1];
        switch (motionAction)
        {
            case 0: // No hacer nada
                components.motion.StandUp();
                components.motion.Walk(0);
                break;
            case 1: // Caminar Right
                components.motion.Walk(1);
                break;
            case 2: // Caminar Left
                components.motion.Walk(-1);
                break;
            case 3: // Impulso normal
                if (transform.localScale.x > 0)
                    components.motion.Dash(false);
                else
                    components.motion.Dash();
                break;
            case 4: // Impulso hacía atrás
                if (transform.localScale.x > 0)
                    components.motion.DashBack();
                else
                    components.motion.DashBack(true);
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
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        if (Input.GetKey(onePlayer["Right"]) && !Input.GetKey(onePlayer["Left"]))
            actions[0] = 1;
        else if (Input.GetKey(onePlayer["Left"]) && !Input.GetKey(onePlayer["Right"]))
            actions[0] = 2;
        else if (transform.localScale.x > 0 && Input.GetKey(onePlayer["Dash"]) || transform.localScale.x < 0 && Input.GetKey(onePlayer["DashBack"]))
            actions[0] = 3;
        else if (transform.localScale.x > 0 && Input.GetKey(onePlayer["DashBack"]) || transform.localScale.x < 0 && Input.GetKey(onePlayer["Dash"]))
            actions[0] = 4;
    }
    void InitDictionarites()
    {
        onePlayer.Add("Block", KeyCode.E);
        onePlayer.Add("Crouch", KeyCode.S);
        onePlayer.Add("Right", KeyCode.D);
        onePlayer.Add("Left", KeyCode.A);
        onePlayer.Add("Dash", KeyCode.RightArrow);
        onePlayer.Add("DashBack", KeyCode.LeftArrow);
        onePlayer.Add("Jump", KeyCode.W);
        onePlayer.Add("Run", KeyCode.LeftControl);
        onePlayer.Add("HardPunch", KeyCode.Y);
        onePlayer.Add("MiddlePunch", KeyCode.T);
        onePlayer.Add("LowPunch", KeyCode.R);
        onePlayer.Add("HardKick", KeyCode.H);
        onePlayer.Add("MiddleKick", KeyCode.G);
        onePlayer.Add("LowKick", KeyCode.F);
        onePlayer.Add("SpecialPunch", KeyCode.R);
        onePlayer.Add("SpecialKick", KeyCode.F);
        // twoPlayer.Add("Block", KeyCode.P);
        // twoPlayer.Add("Crouch", KeyCode.L);
        // twoPlayer.Add("Right", KeyCode.Semicolon);
        // twoPlayer.Add("Left", KeyCode.K);
        // twoPlayer.Add("Jump", KeyCode.O);
        // twoPlayer.Add("Run", KeyCode.AltGr);
        // twoPlayer.Add("HardPunch", KeyCode.Keypad9);
        // twoPlayer.Add("MiddlePunch", KeyCode.Keypad8);
        // twoPlayer.Add("LowPunch", KeyCode.Keypad7);
        // twoPlayer.Add("HardKick", KeyCode.Keypad6);
        // twoPlayer.Add("MiddleKick", KeyCode.Keypad5);
        // twoPlayer.Add("LowKick", KeyCode.Keypad4);
        // twoPlayer.Add("SpecialPunch", KeyCode.Keypad7);
        // twoPlayer.Add("SpecialKick", KeyCode.Keypad4);

    }
}
