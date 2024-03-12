using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character char1;
    [SerializeField] private Character char2;
    private void Update()
    {
        if (char1.CurrentState == char1.States.Jump || char1.CurrentState == char1.States.Fall ||
            char2.CurrentState == char2.States.Jump || char2.CurrentState == char2.States.Fall)
            Physics2D.IgnoreCollision(char1.Body, char2.Body);
        else
            Physics2D.IgnoreCollision(char1.Body, char2.Body, false);
    }
}
