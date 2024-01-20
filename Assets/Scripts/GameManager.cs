using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Components player1Components;
    [SerializeField] Components player2Components;
    void Update()
    {
        if (player1Components.msng.Dead || player2Components.msng.Dead)
        {
            player1Components.Input.enabled = false;
            player2Components.Input.enabled = false;
        }
    }
}
