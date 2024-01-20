using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health;
    private Components components;
    void Awake()
    {
        components = GetComponent<Components>();
    }
    void Start()
    {
        Health = 100;
    }
    void Update()
    {
        if (Health == 0)
        {
            components.msng.Dead = true;
        }
    }

    public void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;
    }
}
