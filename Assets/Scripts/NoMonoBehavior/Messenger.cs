public class Messenger
{
    public Messenger()
    {
        InCooldown = Hurt = Attacking = ApplyForce = ApplyInertia = Falling = Jumping = false;
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
    public short Walking;
    public short ComboCount;
    public AnimationStates RequestedAttack;
}
