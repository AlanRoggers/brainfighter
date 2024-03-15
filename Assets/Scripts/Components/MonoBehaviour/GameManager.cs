using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool TrainStage;
    public float playersDistance { get; private set; }
    public Character Player1 { get; private set; }
    public Character Player2 { get; private set; }
    public PPOAgent Agent1 { get; private set; }
    public PPOAgent Agent2 { get; private set; }
    private void Start()
    {
        Character[] chars = GetComponentsInChildren<Character>();
        Player1 = chars[0];
        Player2 = chars[1];
        playersDistance = UpdatePlayerDistance();
        if (TrainStage)
        {
            PPOAgent[] agents = GetComponentsInChildren<PPOAgent>();
            Agent1 = agents[0];
            Agent2 = agents[1];
        }
    }
    private void Update()
    {
        if (Player1.CurrentState == Player1.States.Jump || Player1.CurrentState == Player1.States.Fall ||
            Player2.CurrentState == Player2.States.Jump || Player2.CurrentState == Player2.States.Fall)
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body);
        else
            Physics2D.IgnoreCollision(Player1.Body, Player2.Body, false);

        playersDistance = UpdatePlayerDistance();

        // Debug.Log(playersDistance);
    }
    private float UpdatePlayerDistance() => Mathf.Abs(Player1.transform.localPosition.x - Player2.transform.localPosition.x);
}
