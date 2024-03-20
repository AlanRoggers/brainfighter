using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool nearestEnemy;
    public bool TrainStage;
    public float PlayersDistance { get; private set; }
    public Character Player1 { get; private set; }
    public Character Player2 { get; private set; }
    private readonly int maxSteps = 1000;
    private int H1;
    private int H2;
    private int steps = 0;
    private void Start()
    {
        Character[] chars = GetComponentsInChildren<Character>();
        Player1 = chars[0];
        Player2 = chars[1];

        PlayersDistance = UpdatePlayerDistance();

        if (TrainStage)
        {
            Player1.Agent.OnBegin += SpawnAgents;

            #region DidDamage

            Player1.States.LowPunch.OnDamaged += AgentDidDamage;
            Player2.States.LowPunch.OnDamaged += AgentDidDamage;

            Player1.States.MiddlePunch.OnDamaged += AgentDidDamage;
            Player2.States.MiddlePunch.OnDamaged += AgentDidDamage;

            Player1.States.HardPunch.OnDamaged += AgentDidDamage;
            Player2.States.HardPunch.OnDamaged += AgentDidDamage;

            Player1.States.SpecialPunch.OnDamaged += AgentDidDamage;
            Player2.States.SpecialPunch.OnDamaged += AgentDidDamage;

            Player1.States.LowKick.OnDamaged += AgentDidDamage;
            Player2.States.LowKick.OnDamaged += AgentDidDamage;

            Player1.States.MiddleKick.OnDamaged += AgentDidDamage;
            Player2.States.MiddleKick.OnDamaged += AgentDidDamage;

            Player1.States.HardKick.OnDamaged += AgentDidDamage;
            Player2.States.HardKick.OnDamaged += AgentDidDamage;

            Player1.States.SpecialKick.OnDamaged += AgentDidDamage;
            Player2.States.SpecialKick.OnDamaged += AgentDidDamage;

            #endregion

            #region AttackBlocked
            Player1.States.LowPunch.OnBlocked += AgentAttackBlocked;
            Player2.States.LowPunch.OnBlocked += AgentAttackBlocked;

            Player1.States.MiddlePunch.OnBlocked += AgentAttackBlocked;
            Player2.States.MiddlePunch.OnBlocked += AgentAttackBlocked;

            Player1.States.HardPunch.OnBlocked += AgentAttackBlocked;
            Player2.States.HardPunch.OnBlocked += AgentAttackBlocked;

            Player1.States.SpecialPunch.OnBlocked += AgentAttackBlocked;
            Player2.States.SpecialPunch.OnBlocked += AgentAttackBlocked;

            Player1.States.LowKick.OnBlocked += AgentAttackBlocked;
            Player2.States.LowKick.OnBlocked += AgentAttackBlocked;

            Player1.States.MiddleKick.OnBlocked += AgentAttackBlocked;
            Player2.States.MiddleKick.OnBlocked += AgentAttackBlocked;

            Player1.States.HardKick.OnBlocked += AgentAttackBlocked;
            Player2.States.HardKick.OnBlocked += AgentAttackBlocked;

            Player1.States.SpecialKick.OnBlocked += AgentAttackBlocked;
            Player2.States.SpecialKick.OnBlocked += AgentAttackBlocked;

            #endregion

            #region AttackCauseStun

            Player1.States.LowPunch.OnCauseStun += AgentAttackCauseStun;
            Player2.States.LowPunch.OnCauseStun += AgentAttackCauseStun;

            Player1.States.MiddlePunch.OnCauseStun += AgentAttackCauseStun;
            Player2.States.MiddlePunch.OnCauseStun += AgentAttackCauseStun;

            Player1.States.HardPunch.OnCauseStun += AgentAttackCauseStun;
            Player2.States.HardPunch.OnCauseStun += AgentAttackCauseStun;

            Player1.States.SpecialPunch.OnCauseStun += AgentAttackCauseStun;
            Player2.States.SpecialPunch.OnCauseStun += AgentAttackCauseStun;

            Player1.States.LowKick.OnCauseStun += AgentAttackCauseStun;
            Player2.States.LowKick.OnCauseStun += AgentAttackCauseStun;

            Player1.States.MiddleKick.OnCauseStun += AgentAttackCauseStun;
            Player2.States.MiddleKick.OnCauseStun += AgentAttackCauseStun;

            Player1.States.HardKick.OnCauseStun += AgentAttackCauseStun;
            Player2.States.HardKick.OnCauseStun += AgentAttackCauseStun;

            Player1.States.SpecialKick.OnCauseStun += AgentAttackCauseStun;
            Player2.States.SpecialKick.OnCauseStun += AgentAttackCauseStun;

            #endregion

            #region AttackToAir

            Player1.States.LowPunch.AttackNoHitted += AgentAttackedToAir;
            Player2.States.LowPunch.AttackNoHitted += AgentAttackedToAir;

            Player1.States.MiddlePunch.AttackNoHitted += AgentAttackedToAir;
            Player2.States.MiddlePunch.AttackNoHitted += AgentAttackedToAir;

            Player1.States.HardPunch.AttackNoHitted += AgentAttackedToAir;
            Player2.States.HardPunch.AttackNoHitted += AgentAttackedToAir;

            Player1.States.SpecialPunch.AttackNoHitted += AgentAttackedToAir;
            Player2.States.SpecialPunch.AttackNoHitted += AgentAttackedToAir;

            Player1.States.LowKick.AttackNoHitted += AgentAttackedToAir;
            Player2.States.LowKick.AttackNoHitted += AgentAttackedToAir;

            Player1.States.MiddleKick.AttackNoHitted += AgentAttackedToAir;
            Player2.States.MiddleKick.AttackNoHitted += AgentAttackedToAir;

            Player1.States.HardKick.AttackNoHitted += AgentAttackedToAir;
            Player2.States.HardKick.AttackNoHitted += AgentAttackedToAir;

            Player1.States.SpecialKick.AttackNoHitted += AgentAttackedToAir;
            Player2.States.SpecialKick.AttackNoHitted += AgentAttackedToAir;

            #endregion

            #region Win

            Player1.States.LowPunch.OnWin += AgentWin;
            Player2.States.LowPunch.OnWin += AgentWin;

            Player1.States.MiddlePunch.OnWin += AgentWin;
            Player2.States.MiddlePunch.OnWin += AgentWin;

            Player1.States.HardPunch.OnWin += AgentWin;
            Player2.States.HardPunch.OnWin += AgentWin;

            Player1.States.SpecialPunch.OnWin += AgentWin;
            Player2.States.SpecialPunch.OnWin += AgentWin;

            Player1.States.LowKick.OnWin += AgentWin;
            Player2.States.LowKick.OnWin += AgentWin;

            Player1.States.MiddleKick.OnWin += AgentWin;
            Player2.States.MiddleKick.OnWin += AgentWin;

            Player1.States.HardKick.OnWin += AgentWin;
            Player2.States.HardKick.OnWin += AgentWin;

            Player1.States.SpecialKick.OnWin += AgentWin;
            Player2.States.SpecialKick.OnWin += AgentWin;

            #endregion


            Player1.States.Hurt.OnHurt += AgentHurted;
            Player2.States.Hurt.OnHurt += AgentHurted;

            Player1.States.Block.OnBlock += AgentBlockedAttack;
            Player2.States.Block.OnBlock += AgentBlockedAttack;

            Player1.States.Stun.OnStun += AgentStuned;
            Player2.States.Stun.OnStun += AgentStuned;

            Player1.States.Dead.OnDead += AgentDead;
            Player2.States.Dead.OnDead += AgentDead;


        }


    }
    private void Update()
    {
        IgnoreCollisions();
        PlayersDistance = UpdatePlayerDistance();
        nearestEnemy = Player1.OverlapDetector.EnemyOverlapping(Player1.Body, Player1.CharacterLayer);
    }
    private void FixedUpdate()
    {
        if (TrainStage)
        {
            steps++;

            if (steps % 50 == 0)
            {
                if (H1 == Player1.Health && H2 == Player2.Health)
                {
                    Player1.Agent.AddReward(-0.01f);
                    Player2.Agent.AddReward(-0.01f);
                }
                else
                {
                    H1 = Player1.Health;
                    H2 = Player2.Health;
                }
            }

            if (steps % 10 == 0)
            {
                if (!nearestEnemy)
                {
                    Player1.Agent.AddReward(-0.001f);
                    Player2.Agent.AddReward(-0.001f);
                }
            }

            if (steps == maxSteps)
            {
                if (Player1.Health > Player2.Health)
                {
                    Player1.Agent.AddReward(50);
                    Player2.Agent.AddReward(-50);
                    Player1.Agent.EndEpisode();
                    Player2.Agent.EndEpisode();
                    return;
                }

                if (Player2.Health > Player1.Health)
                {
                    Player1.Agent.AddReward(-50);
                    Player2.Agent.AddReward(50);
                    Player1.Agent.EndEpisode();
                    Player2.Agent.EndEpisode();
                    return;
                }


                Player1.Agent.EpisodeInterrupted();
                Player2.Agent.EpisodeInterrupted();
            }
            //     Player1.Agent.AddReward(-0.000001f);
            //     Player2.Agent.AddReward(-0.000001f);
        }
    }
    private float UpdatePlayerDistance() => Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x);
    private void IgnoreCollisions()
    {
        if (Player1.CurrentState == Player1.States.Jump || Player1.CurrentState == Player1.States.Fall ||
            Player2.CurrentState == Player2.States.Jump || Player2.CurrentState == Player2.States.Fall)
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body);
        else
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body, false);
    }

    #region TrainingAI
    private void AgentDidDamage(PPOAgent agent, Attack attack)
    {
        agent.AddReward(1);
    }
    private void AgentHurted(PPOAgent agent) => agent.AddReward(-1);
    private void AgentBlockedAttack(PPOAgent agent) => agent.AddReward(1);
    private void AgentAttackBlocked(PPOAgent agent) => agent.AddReward(-0.0001f);
    private void AgentAttackCauseStun(PPOAgent agent) => agent.AddReward(10);
    private void AgentStuned(PPOAgent agent) => agent.AddReward(-10);
    private void AgentAttackedToAir(PPOAgent agent) => agent.AddReward(-0.1f);
    private void AgentWin(PPOAgent agent)
    {
        agent.AddReward(100);
        agent.EndEpisode();
    }
    private void AgentDead(PPOAgent agent)
    {
        agent.AddReward(-100);
        agent.EndEpisode();
    }
    private void SpawnAgents()
    {
        // lastAttack = null;
        //-14, 12
        steps = 0;
        float distance = 5f;
        float Player1X = Random.Range(-14f, 12f);
        float Player2X;

        if (Player1X - distance > -13f && Player1X + distance < 11)
            Player2X = Player1X + distance * (Random.Range(0, 1) == 0 ? 1 : -1);
        else if (Player1X - distance > -13f)
            Player2X = Player1X - distance;
        else
            Player2X = Player1X + distance;


        Player1.transform.localPosition = new Vector2(Player1X, Player1.Spawn.y);
        Player2.transform.localPosition = new Vector2(Player2X, Player2.Spawn.y);
    }

    #endregion
}
