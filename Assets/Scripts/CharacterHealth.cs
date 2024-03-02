public class CharacterHealth
{
    public int Health { get; private set; }
    public CharacterHealth(int health) => Health = health;
    public void ReduceHealth(int damage)
    {
        if (Health - damage > 0)
            Health -= damage;
        else
            Health = 0;
    }
}
