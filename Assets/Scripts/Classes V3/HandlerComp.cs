using UnityEngine;

public class HandlerComp
{
    public CircleCollider2D CircleHitBox;
    public OverlapDetector collision;
    public StateMachine Machine;
    public LayerMask ContactLayer;
    public Messenger Messenger;
    public Rigidbody2D Physics;
}
