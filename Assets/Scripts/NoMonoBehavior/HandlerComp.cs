using UnityEngine;

public class HandlerComp
{
    public HandlerComp(
        CircleCollider2D circleHitBox, OverlapDetector collision, StateMachine machine, LayerMask contact,
        Messenger messenger, Rigidbody2D phys, Transform transform, CharacterHealth health, BoxCollider2D characterColl)
    {
        CircleHitBox = circleHitBox;
        Machine = machine;
        Messenger = messenger;
        Transform = transform;
        CharacterColl = characterColl;
        Collision = collision;
        ContactLayer = contact;
        Physics = phys;
        Health = health;
    }
    public readonly CircleCollider2D CircleHitBox;
    public readonly OverlapDetector Collision;
    public readonly StateMachine Machine;
    public readonly LayerMask ContactLayer;
    public readonly Messenger Messenger;
    public readonly Rigidbody2D Physics;
    public readonly Transform Transform;
    public readonly CharacterHealth Health;
    public readonly BoxCollider2D CharacterColl;
}
