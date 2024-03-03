public class Messenger
{
    public Messenger()
    {
        InCooldown = Hurt = Attacking = false;
        Crouching = Falling = Jumping = false;
        OverlappingEnemy = Immune = false;
        Walking = ComboCount = 0;
        RequestedAttack = AnimationStates.Null;
    }
    public bool Falling;
    public bool InGround;
    public bool Attacking;
    public bool InCooldown;
    public bool Hurt;
    public bool Jumping;
    public bool Crouching;
    public bool Blocking;
    public bool DistanceForBlock;
    public bool Incapacited;
    public bool OverlappingEnemy;
    public bool Immune;
    public sbyte Walking;
    public sbyte ComboCount;
    public AnimationStates RequestedAttack;
}
