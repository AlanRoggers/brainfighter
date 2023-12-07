using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health;
    void Start()
    {
        health = 100;
    }
    public void ReduceHealth(int damage)
    {
        if (health - damage > 0)
            health -= damage;
        else
            health = 0;
    }
}
