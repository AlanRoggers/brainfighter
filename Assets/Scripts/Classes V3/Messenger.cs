public class Messenger
{
    public Messenger()
    {
        InCooldown = Hurt = Attacking = false;
        Walking = 0;
        RequestedAttack = AnimationStates.Null;
    }
    public bool Attacking;
    public bool InCooldown;
    public bool Hurt;
    public int Walking;
    public AnimationStates RequestedAttack;
}
