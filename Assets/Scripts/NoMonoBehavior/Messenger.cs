public class Messenger
{
    public Messenger()
    {
        InCooldown = Hurt = Attacking = ApplyForce = false;
        Crouching = ApplyInertia = Falling = Jumping = false;
        Walking = ComboCount = 0;
        RequestedAttack = AnimationStates.Null;
    }
    public bool Falling;
    public bool InGround;
    public bool Attacking;
    public bool InCooldown;
    public bool Hurt;
    public bool ApplyForce;
    public bool ApplyInertia;
    public bool Jumping;
    public bool Crouching;
    public sbyte Walking;
    public sbyte ComboCount;
    public AnimationStates RequestedAttack;
}
