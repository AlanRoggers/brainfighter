using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool ignoredCollisions;
    [SerializeField] private Character char1;
    [SerializeField] private Character char2;
    private void Update()
    {
        if ((!char1.Components.Messenger.InGround || !char2.Components.Messenger.InGround || char1.Components.Messenger.Attacking || char2.Components.Messenger.Attacking) && !ignoredCollisions)
        {
            // Debug.Log("Se ignoro");
            Physics2D.IgnoreCollision(char1.Components.CharacterColl, char2.Components.CharacterColl);
            ignoredCollisions = true;
        }
        else if (char1.Components.Messenger.InGround && char2.Components.Messenger.InGround && !char1.Components.Messenger.Attacking && !char2.Components.Messenger.Attacking)
        {
            Debug.Log("No se ignora");
            ignoredCollisions = false;
            Physics2D.IgnoreCollision(char1.Components.CharacterColl, char2.Components.CharacterColl, false);
        }
    }
}
