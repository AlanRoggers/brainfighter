using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool ignoredCollisions;
    [SerializeField] private CharacterV5 char1;
    [SerializeField] private CharacterV5 char2;
    private void Update()
    {
        if (char1.currentState == char1.States.Jump || char1.currentState == char1.States.Fall ||
            char2.currentState == char2.States.Jump || char2.currentState == char2.States.Fall)
            Physics2D.IgnoreCollision(char1.Body, char2.Body);
        else
            Physics2D.IgnoreCollision(char1.Body, char2.Body, false);
    }
}
