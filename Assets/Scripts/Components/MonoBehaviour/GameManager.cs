using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool TrainStage;
    public float PlayersDistance { get; private set; }
    public Character Player1 { get; private set; }
    public Character Player2 { get; private set; }
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

            // #region AttackBlocked
            // Player1.States.LowPunch.OnBlocked += AgentAttackBlocked;
            // Player2.States.LowPunch.OnBlocked += AgentAttackBlocked;

            // Player1.States.MiddlePunch.OnBlocked += AgentAttackBlocked;
            // Player2.States.MiddlePunch.OnBlocked += AgentAttackBlocked;

            // Player1.States.HardPunch.OnBlocked += AgentAttackBlocked;
            // Player2.States.HardPunch.OnBlocked += AgentAttackBlocked;

            // Player1.States.SpecialPunch.OnBlocked += AgentAttackBlocked;
            // Player2.States.SpecialPunch.OnBlocked += AgentAttackBlocked;

            // Player1.States.LowKick.OnBlocked += AgentAttackBlocked;
            // Player2.States.LowKick.OnBlocked += AgentAttackBlocked;

            // Player1.States.MiddleKick.OnBlocked += AgentAttackBlocked;
            // Player2.States.MiddleKick.OnBlocked += AgentAttackBlocked;

            // Player1.States.HardKick.OnBlocked += AgentAttackBlocked;
            // Player2.States.HardKick.OnBlocked += AgentAttackBlocked;

            // Player1.States.SpecialKick.OnBlocked += AgentAttackBlocked;
            // Player2.States.SpecialKick.OnBlocked += AgentAttackBlocked;

            // #endregion

            // #region AttackCauseStun

            // Player1.States.LowPunch.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.LowPunch.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.MiddlePunch.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.MiddlePunch.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.HardPunch.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.HardPunch.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.SpecialPunch.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.SpecialPunch.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.LowKick.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.LowKick.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.MiddleKick.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.MiddleKick.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.HardKick.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.HardKick.OnCauseStun += AgentAttackCauseStun;

            // Player1.States.SpecialKick.OnCauseStun += AgentAttackCauseStun;
            // Player2.States.SpecialKick.OnCauseStun += AgentAttackCauseStun;

            // #endregion

            // #region Win

            // Player1.States.LowPunch.OnWin += AgentWin;
            // Player2.States.LowPunch.OnWin += AgentWin;

            // Player1.States.MiddlePunch.OnWin += AgentWin;
            // Player2.States.MiddlePunch.OnWin += AgentWin;

            // Player1.States.HardPunch.OnWin += AgentWin;
            // Player2.States.HardPunch.OnWin += AgentWin;

            // Player1.States.SpecialPunch.OnWin += AgentWin;
            // Player2.States.SpecialPunch.OnWin += AgentWin;

            // Player1.States.LowKick.OnWin += AgentWin;
            // Player2.States.LowKick.OnWin += AgentWin;

            // Player1.States.MiddleKick.OnWin += AgentWin;
            // Player2.States.MiddleKick.OnWin += AgentWin;

            // Player1.States.HardKick.OnWin += AgentWin;
            // Player2.States.HardKick.OnWin += AgentWin;

            // Player1.States.SpecialKick.OnWin += AgentWin;
            // Player2.States.SpecialKick.OnWin += AgentWin;

            // #endregion


            // Player1.States.Hurt.OnHurt += AgentHurted;
            // Player2.States.Hurt.OnHurt += AgentHurted;

            // Player1.States.Block.OnBlock += AgentBlockedAttack;
            // Player2.States.Block.OnBlock += AgentBlockedAttack;

            // Player1.States.Stun.OnStun += AgentStuned;
            // Player2.States.Stun.OnStun += AgentStuned;

            // Player1.States.Dead.OnDead += AgentDead;
            // Player2.States.Dead.OnDead += AgentDead;


        }


    }
    private void Update()
    {
        Debug.Log($"Posicion del jugador {Player1.transform.localPosition.x}");
        IgnoreCollisions();
        PlayersDistance = UpdatePlayerDistance();
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
    private void AgentDidDamage(PPOAgent agent) { agent.AddReward(10); agent.EndEpisode(); }
    private void AgentHurted(PPOAgent agent) => agent.AddReward(-1);
    private void AgentBlockedAttack(PPOAgent agent) => agent.AddReward(1);
    private void AgentAttackBlocked(PPOAgent agent) => agent.AddReward(-1);
    private void AgentAttackCauseStun(PPOAgent agent) => agent.AddReward(10);
    private void AgentStuned(PPOAgent agent) => agent.AddReward(-10);
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
        //-14, 12
        float Player1X = Random.Range(-14f, 12f);
        float Player2X;

        if (Player1X - 10f > -13f && Player1X + 10f < 11)
            Player2X = Player1X + 10 * (Random.Range(0, 1) == 0 ? 1 : -1);
        else if (Player1X - 10f > -13f)
            Player2X = Player1X - 10;
        else
            Player2X = Player1X + 10;


        Player1.transform.localPosition = new Vector2(Player1X, Player1.Spawn.y);
        Player2.transform.localPosition = new Vector2(Player2X, Player2.Spawn.y);
    }

    #endregion
}
