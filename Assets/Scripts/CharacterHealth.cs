using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int Health { get; private set; }
    public void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;
    }
}
