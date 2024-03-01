using UnityEngine;

public class OverlapDetector
{
    public Collider2D AttackHit(LayerMask contactLayer, CircleCollider2D hitbox) => Physics2D.OverlapCircle(hitbox.bounds.center, hitbox.radius, contactLayer);


    public bool GroundDetection(Transform transform, Vector2 position, Vector2 size, LayerMask ground)
    {
        return Physics2D.OverlapBox((Vector2)transform.localPosition + position, size, 0f, ground) != null;
    }
}
